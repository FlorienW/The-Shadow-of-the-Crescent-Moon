using Audio;
using UnityEngine;

namespace Player
{
    public class PlayerAudioEffects : MonoBehaviour
    {
        public AudioClip[] walkOnTheGrass;
        public AudioClip[] walkOnTheWood;
        private Rigidbody2D _rigidbody2D;
        
        public AudioClip[] daggerSwings;
        public PlayerMovement playerMovement;

        //* ABSTRACTION
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            playerMovement = GetComponent<PlayerMovement>();
        }


        public void PlayFootstepAudioEffect()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

            if (hit.collider == null || !playerMovement.IsGrounded) return;
            if (hit.collider.CompareTag("Ground"))
            {
                int index = Random.Range(0, walkOnTheGrass.Length);
                AudioController.instance.PlayEffectAtPoint(walkOnTheGrass[index], transform.position);
            }
            else if (hit.collider.CompareTag("Wood_Ground"))
            {
                int index = Random.Range(0, walkOnTheWood.Length);
                AudioController.instance.PlayEffectAtPoint(walkOnTheWood[index], transform.position);
            }
        }

        public void PlayDaggerSwingAudioEffect()
        {
            int index = Random.Range(0, daggerSwings.Length);
            
            AudioController.instance.PlayEffectAtPoint(daggerSwings[index], transform.position);
        }
    }
}
