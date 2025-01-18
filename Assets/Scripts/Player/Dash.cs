using System.Collections;
using UnityEngine;

namespace Player
{
    public class Dash : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private float _dashTime = 0.25f;
        private readonly float _dashForce = 10f;
        private TrailRenderer _trail;
        private PlayerMovement _playerMovement;
        private CapsuleCollider2D _capsuleCollider;

        //* ABSTRACTION
        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _trail = transform.GetChild(1).gameObject.GetComponent<TrailRenderer>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        public IEnumerator DashCoroutine()
        {
            Debug.Log("Dashing");
            _playerMovement.enabled = false;
            float tempGravity = _rigidbody.gravityScale;
            _trail.emitting = true;
            _rigidbody.gravityScale = 0;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _capsuleCollider.enabled = false;
            _rigidbody.linearVelocity = new Vector2(-1 * transform.localScale.x * _dashForce, 0f);
            yield return new WaitForSeconds(_dashTime);
            _capsuleCollider.enabled = true;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _trail.emitting = false;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.gravityScale = tempGravity;
            _playerMovement.enabled = true;
            _playerMovement.StartCoroutine(_playerMovement.DashCooldown());
            gameObject.GetComponent<Dash>().enabled = false;
        }
    }
}
