using System.Collections;
using UnityEngine;

namespace Enemy
{
    //* INHERITANCE
    public class Priest : Enemy
    {
        //* ABSTRACTION
        //* POLYMORPHISM
        public override IEnumerator ChaseThePlayer()
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > 6f)
            {
                movementSpeed = detectedMovementSpeed;
            }
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
            
            else if (Mathf.Abs(player.transform.position.x - transform.position.x) < 6f)
            {
                StartCoroutine(AttackToPlayer());
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(ChaseThePlayer());
            }
        }

        //* POLYMORPHISM
        public override IEnumerator AttackToPlayer()
        {
            movementSpeed = 0;
            AnimatorMovementStop();
            attackIcon.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            attackIcon.SetActive(false);
            AnimatorAttack();
            yield return new WaitForSeconds(3f);
            StartCoroutine(ChaseThePlayer());
            AnimatorStopAttack();
        }

        //* POLYMORPHISM
        public override void InActivateAllEnemyColliders()
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().attachedRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
