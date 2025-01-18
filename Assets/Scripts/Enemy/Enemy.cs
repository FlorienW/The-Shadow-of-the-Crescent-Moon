using System.Collections;
using Audio;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        //* ENCAPSULATION
        private static readonly int IsMovingB = Animator.StringToHash("Is_Moving_B");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Die = Animator.StringToHash("Die");
        
        [SerializeField]
        private Rigidbody2D _rb;
        [SerializeField]
        private GameObject aPoint;
        [SerializeField]
        private GameObject bPoint;

        private Vector2 _patrolStartPosition;
        private Vector2 _patrolEndPosition;
        
        public float movementSpeed;
        public float detectedMovementSpeed;
        public float undetectedMovementSpeed;
        public float movementDirection;
        public float minimunLoopWaitingTime = 0.5f;
        public float maximunLoopWaitingTime = 0.9f;
        
        public GameObject player;
        public GameObject alertIcon;
        public GameObject attackIcon;
        public GameObject bloodParticle;
        
        [SerializeField]
        private Animator _animator;
        private readonly Vector3 _lookToTheRightDirection = new(-1, 1, 1);
        private readonly Vector3 _lookToTheLeftDirection = new(1, 1, 1);

        public float enemyHealthPoint = 40;

        public int debugCounter = 1;
        
        
        public AudioClip[] daggerSlideAudioEffects;
        
        //* ABSTRACTION
        public void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _patrolStartPosition = aPoint.transform.position;
            _patrolEndPosition = bPoint.transform.position;
            aPoint.transform.parent = null;
            bPoint.transform.parent = null;
            PatrollingLoop();
            StartCoroutine(CheckPlayerDistance());
            _animator = GetComponent<Animator>();

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(aPoint.transform.position, 0.5f);
            Gizmos.DrawSphere(bPoint.transform.position, 0.5f);
            Gizmos.DrawLine(aPoint.transform.position, bPoint.transform.position);
        }

        private void Update()
        {
            MoveEnemy();
        }

        public void PatrollingLoop()
        { 
            movementDirection = -1 * Mathf.Sign(transform.position.x - _patrolStartPosition.x);
            LookToTheRightDirection();
            _animator.SetBool(IsMovingB, true);
            StartCoroutine(GoToAPoint());
        }
        
        public IEnumerator GoToAPoint()
        {
            if (Vector2.Distance(transform.position, _patrolStartPosition) < 0.5f)
            {
                movementDirection = 0;
                AnimatorMovementStop();
                yield return new WaitForSeconds(Random.Range(minimunLoopWaitingTime, maximunLoopWaitingTime));
                movementDirection = -1;
                LookToTheLeftDirection();
                AnimatorMovementStart();
                StartCoroutine(GoToBPoint());
                
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(GoToAPoint());
            }

        }

        public IEnumerator GoToBPoint()
        {
            
            if (Vector2.Distance(transform.position, _patrolEndPosition) < 0.5f)
            {
                movementDirection = 0;
                AnimatorMovementStop();
                yield return new WaitForSeconds(Random.Range(minimunLoopWaitingTime, maximunLoopWaitingTime));
                movementDirection = 1;
                LookToTheRightDirection();
                AnimatorMovementStart();
                StartCoroutine(GoToAPoint());

            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(GoToBPoint());
            }
        }


        private void MoveEnemy()
        { 
            _rb.linearVelocityX = movementDirection * movementSpeed;
        }

        public IEnumerator CheckPlayerDistance()
        {
            if (Vector2.Distance(transform.position, player.transform.position) < 5f)
            {
                movementDirection = 0;
                AnimatorMovementStop();
                StopCoroutine(GoToAPoint());
                StopCoroutine(GoToBPoint());
                StartCoroutine(DetectThePlayer());
            }
            else
            {
                yield return new WaitForSeconds(0.75f);
                StartCoroutine(CheckPlayerDistance());
            }
        }

        public IEnumerator DetectThePlayer()
        {
            alertIcon.SetActive(true);
            movementSpeed = 0;
            AnimatorMovementStop();
            movementDirection = Mathf.Sign(player.transform.position.x - transform.position.x);
            LookSpecificDirection((int)movementDirection * -1);
            yield return new WaitForSeconds(0.5f);
            movementSpeed = detectedMovementSpeed;
            AnimatorMovementStart();
            alertIcon.SetActive(false);
            StartCoroutine(ChaseThePlayer());

        }
        
        public virtual IEnumerator ChaseThePlayer()
        {
            if (Mathf.Approximately(Mathf.Sign(player.transform.position.x - transform.position.x), -movementDirection))
            {
                float temp = movementDirection;
                movementDirection = 0;
                AnimatorMovementStop();
                yield return new WaitForSeconds(1f);
                movementDirection = -temp;
                LookSpecificDirection((int)temp);
                AnimatorMovementStart();
                StartCoroutine(ChaseThePlayer());
            }
            
            else if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1f)
            {
                StartCoroutine(AttackToPlayer());
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(ChaseThePlayer());
            }
        }

        public virtual IEnumerator AttackToPlayer()
        {
            movementSpeed = 0;
            attackIcon.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            attackIcon.SetActive(false);
            AnimatorMovementStop();
            AnimatorAttack();
            yield return new WaitForSeconds(1f);
            movementSpeed = detectedMovementSpeed;
            AnimatorMovementStart();
            StartCoroutine(ChaseThePlayer());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player_Weapon"))
            {
                Debug.Log($"Player hit {debugCounter++} times.");
                PlayDaggerSliceSoundEffect();
                TakeDamage(5f);
                CallBloodParticles
                (
                    (int)Mathf.Sign(other.transform.position.x - transform.position.x),
                    other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position)
                );
            }
        }

        public void TakeDamage(float damage)
        {
            enemyHealthPoint -= damage;
            AnimatorTakeDamage();
            HealthCheck();
        }

        public void CallBloodParticles(int direction, Vector2 destination)
        {
            Instantiate(bloodParticle, destination,
                direction == 1 ? Quaternion.Euler(0f, 90, 0f) : Quaternion.Euler(0f, -90, 0f));
        }

        private void HealthCheck()
        {
            Debug.Log($"Enemy Has Left {enemyHealthPoint} HP.");
            if (enemyHealthPoint < 1)
            {
                StopAllCoroutines();
                InactivateAllIcons();
                movementSpeed = 0;
                AnimatorMovementStop();
                InActivateAllEnemyColliders();
                AnimatorDie();
            }
        }

        public void InactivateAllIcons()
        {
            alertIcon.SetActive(false);
            attackIcon.SetActive(false);
        }

        public virtual void InActivateAllEnemyColliders()
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().attachedRigidbody.bodyType = RigidbodyType2D.Kinematic;
            gameObject.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        
        public void AnimatorMovementStop()
        {
            _animator.SetBool(IsMovingB, false);
        }

        public void AnimatorMovementStart()
        {
            _animator.SetBool(IsMovingB, true);
        }

        public void AnimatorAttack()
        {
            _animator.SetTrigger(Attack);
        }

        public void AnimatorStopAttack()
        {
            _animator.ResetTrigger(Attack);
        }

        public void LookToTheRightDirection()
        {
            transform.localScale = _lookToTheRightDirection;
        }

        public void LookToTheLeftDirection()
        {
            transform.localScale = _lookToTheLeftDirection;
        }

        public void LookSpecificDirection(int direction)
        {
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }

        public void AnimatorTakeDamage()
        {
            _animator.SetTrigger(Hurt);
        }

        public void AnimatorDie()
        {
            _animator.SetTrigger(Die);
        }

        public void PlayDaggerSliceSoundEffect()
        {
            int index = Random.Range(0, daggerSlideAudioEffects.Length);
            AudioController.instance.PlayEffectAtPoint(daggerSlideAudioEffects[index], transform.position);
        }
        

    }
}
