using UnityEngine;

namespace Enemy
{
    public class PriestAttackProjectile : MonoBehaviour
    {
        //* ENCAPSULATION
        [SerializeField]
        private GameObject player;
        private Rigidbody2D _rigidbody2D;
        public float projectileSpeed;

        //* ABSTRACTION
        private void Start()
        {
            player = GameObject.Find("Player");
            Vector2 shootingVector2 = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y + 1.2f);
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.AddForce(shootingVector2.normalized * projectileSpeed, ForceMode2D.Impulse);
            Debug.Log(shootingVector2.normalized);
            Debug.Log(projectileSpeed);
        }
    }
}
