using Audio;
using UnityEngine;

namespace Enemy
{
    public class SoldierAndKnightAttackAnimation : MonoBehaviour
    {
        private BoxCollider2D _weaponCollider;
        public AudioClip[] weaponSwingSoundEffects;

        //* ABSTRACTION
        private void Start()
        {
            _weaponCollider = gameObject.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        }

        private void ActivateWeaponCollider()
        {
            Debug.Log("Weapon Activated");
            _weaponCollider.enabled = true;
            int index = Random.Range(0, weaponSwingSoundEffects.Length);
            AudioController.instance.PlayEffectAtPoint(weaponSwingSoundEffects[index], transform.position);
        }

        private void DeactivateWeaponCollider()
        {
            Debug.Log("Weapon Deactivated");
            _weaponCollider.enabled = false;
        }
    }
}
