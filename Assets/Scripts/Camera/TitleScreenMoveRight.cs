using UnityEngine;

namespace Camera
{
    public class TitleScreenMoveRight : MonoBehaviour
    {
        public float movementSpeed;
    
        //* ABSTRACTION
        private void FixedUpdate()
        {
            transform.Translate(Vector2.right * (Time.fixedDeltaTime * movementSpeed));
        }
    }
}
