using UnityEngine;

namespace Player
{
    public class BetterJumps : MonoBehaviour
    {
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        private Rigidbody2D _rb;
    
        //* ABSTRACTION
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (_rb.linearVelocity.y < 0)
            {
                _rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            else if (_rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                _rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
            }
        }
    }
}
