using UnityEngine;

namespace Enemy
{
    public class PriestAttackAnimation : MonoBehaviour
    {
        public GameObject weaponAlignPoint;
        public GameObject fireball;

        //* ABSTRACTION
        private void ShotProjectileToThePlayer()
        {
            Instantiate(fireball, weaponAlignPoint.transform.position, Quaternion.identity);
        }
    }
}
