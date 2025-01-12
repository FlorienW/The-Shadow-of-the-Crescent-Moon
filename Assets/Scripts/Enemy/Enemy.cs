using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        private static readonly int IsMovingB = Animator.StringToHash("Is_Moving_B");
        private static readonly int IsAttackingB = Animator.StringToHash("Is_Attacking_B");

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
        
        [SerializeField]
        private Animator _animator;
        private readonly Vector3 _lookToTheRightDirection = new(-1, 1, 1);
        private readonly Vector3 _lookToTheLeftDirection = new(1, 1, 1);
        
        
        
        
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
        
        public IEnumerator ChaseThePlayer()
        {
            if (Mathf.Approximately(Mathf.Sign(player.transform.position.x - transform.position.x), -movementDirection))
            {
                float temp = movementDirection;
                movementDirection = 0;
                AnimatorMovementStop();
                yield return new WaitForSeconds(1f);
                movementDirection = -temp;
                LookToTheOppositeDirection();
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

        public IEnumerator AttackToPlayer()
        {
            movementSpeed = 0;
            AnimatorMovementStop();
            AnimatorAttack();
            yield return new WaitForSeconds(1f);
            movementSpeed = detectedMovementSpeed;
            AnimatorMovementStart();
            StartCoroutine(ChaseThePlayer());
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
            _animator.SetBool(IsAttackingB, true);
            _animator.SetBool(IsAttackingB, false);
        }

        public void LookToTheRightDirection()
        {
            transform.localScale = _lookToTheRightDirection;
        }

        public void LookToTheLeftDirection()
        {
            transform.localScale = _lookToTheLeftDirection;
        }

        public void LookToTheOppositeDirection()
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        public void LookSpecificDirection(int direction)
        {
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }

    }
}
