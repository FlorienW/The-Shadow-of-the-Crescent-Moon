using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealthManager : MonoBehaviour
    {
        //* ENCAPSULATION
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        public float health = 100f;

        private Animator _animator;
        public Slider healthbarSlider;
        public Dash dash;
        public PlayerMovement playerMovement;
        public TextMeshProUGUI healthText;
        
        public AudioClip[] takeHitSounds;
        
        public GameObject defeatScreen;

        //* ABSTRACTION
        private void Start()
        {
            _animator = GetComponent<Animator>();
            health = 100f;
            dash = GetComponent<Dash>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Collided");
            if (other.CompareTag("Priest_Projectile"))
            {
                Destroy(other.gameObject);
                TakeDamage(10f);
            }
            else if (other.CompareTag("Soldier_Weapon"))
            {
                TakeDamage(30f);
            }
            else if (other.CompareTag("Knight_Weapon"))
            {
                TakeDamage(15f);
            }
        }

        private void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0f)
            {
                health = 0f;
            }
            Debug.Log($"Player Has {health} HP");
            _animator.SetTrigger(Hurt);
            SetValueOfHealthbarSlider();
            CheckIsPlayerDead();
            SetValueOfHealthbarText();
            PlayHitSoundEffect();
        }

        private void CheckIsPlayerDead()
        {
            if (health <= 0)
            {
                _animator.SetTrigger(Die);
                dash.enabled = false;
                playerMovement.enabled = false;
                ActivateTheDefeatScreen();
            }
        }

        private void SetValueOfHealthbarSlider()
        {
            healthbarSlider.value = health;
        }

        private void SetValueOfHealthbarText()
        {
            healthText.text = health.ToString();
        }

        private void PlayHitSoundEffect()
        {
            int index = Random.Range(0, takeHitSounds.Length);
            AudioController.instance.PlayEffectAtPoint(takeHitSounds[index], transform.position);
        }

        private void ActivateTheDefeatScreen()
        {
            defeatScreen.SetActive(true);
        }

        
    }
}
