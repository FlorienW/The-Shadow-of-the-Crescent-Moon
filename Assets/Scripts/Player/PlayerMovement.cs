using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //* ENCAPSULATION
        private static readonly int IsMovingB = Animator.StringToHash("Is_Moving_B");
        private static readonly int IsAttackingB = Animator.StringToHash("Is_Attacking_B");
        public float movementSpeed = 40f;
        public float jumpForce = 40f;
        public float dashForce = 40f;
        private Rigidbody2D _rb;
        private float _horizontalInput;
        private bool _isDashAvailable = true;
        public bool IsGrounded { get; set; }
        public float dashCooldown;
        private Animator _animator;
        [SerializeField]
        private BoxCollider2D[] boxColliders2D;

        private Dash _dash;

        //* ABSTRACTION
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            StartCoroutine(DashCooldown());
            _dash = GetComponent<Dash>();
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

            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                _rb.AddForceY(jumpForce,ForceMode2D.Impulse);
                IsGrounded = false;
            }

            if (Input.GetKeyDown(KeyCode.C) && _isDashAvailable)
            {
                _dash.StartCoroutine(_dash.DashCoroutine());
            }
        }
    
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsGrounded = true;
            }
        }

        public IEnumerator DashCooldown()
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
