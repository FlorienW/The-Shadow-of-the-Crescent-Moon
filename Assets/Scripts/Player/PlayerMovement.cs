using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private static readonly int IsMovingB = Animator.StringToHash("Is_Moving_B");
        private static readonly int IsAttackingB = Animator.StringToHash("Is_Attacking_B");
        public float movementSpeed = 40f;
        public float jumpForce = 40f;
        public float dashForce = 40f;
        private Rigidbody2D _rb;
        private float _horizontalInput;
        private bool _isDashAvailable = true;
        [SerializeField]
        private bool isGrounded = true;
        public float dashCooldown;
        private Animator _animator;
        [SerializeField]
        private BoxCollider2D[] boxColliders2D;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            GetInput();
            FlipToMovementDirection();
            MoveWithInput();
            UpdateTheAnimator();
            Attack();
        }

        private void GetInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
        }

        private void FlipToMovementDirection()
        {
            transform.localScale = _horizontalInput switch
            {
                > 0 => new Vector3(-1, 1, 1),
                < 0 => new Vector3(1, 1, 1),
                _ => transform.localScale
            };
        }
    
        private void MoveWithInput()
        {
            if (Mathf.Abs(_horizontalInput) > 0)
            {
                _rb.linearVelocity = new Vector2(_horizontalInput * movementSpeed, _rb.linearVelocity.y);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                _rb.AddForceY(jumpForce,ForceMode2D.Impulse);
                isGrounded = false;
            }

            if (Input.GetKeyDown(KeyCode.C) && _isDashAvailable)
            {
                int horizontalDirection = _horizontalInput switch
                {
                    > 0 => 1,
                    < 0 => -1,
                    _ => transform.localScale == new Vector3(1, 1, 1) ? -1 : 1
                };
                int verticalDirection = (Input.GetKey(KeyCode.S)) ? -1 : (Input.GetKey(KeyCode.W)) ? 1 : 0;
                Vector2 dashDirection = new Vector2(horizontalDirection, verticalDirection).normalized;
                _rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
                StartCoroutine(DashCooldown());
            }
        }
    
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        IEnumerator DashCooldown()
        {
            _isDashAvailable = false;
            yield return new WaitForSeconds(dashCooldown);
            _isDashAvailable = true;
        }

        private void UpdateTheAnimator()
        {
            _animator.SetBool(IsMovingB, _horizontalInput != 0f);
        }

        private void Attack()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _animator.SetBool(IsAttackingB, true);
                boxColliders2D[0].enabled = true;
                boxColliders2D[1].enabled = true;
            }

            else if (Input.GetButtonUp("Fire1"))
            {
                _animator.SetBool(IsAttackingB, false);
                boxColliders2D[0].enabled = false;
                boxColliders2D[1].enabled = false;
            }
        }
    }
}
