using UnityEngine;

namespace Camera
{
    public class BackgroundInfiniteParallax : MonoBehaviour
    {
        private float _startPosition;
        private float _length;
        public GameObject cam;
        public float parallaxEffect;
    
        //* ABSTRACTION
        void Start()
        {
            _startPosition = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void FixedUpdate()
        {
            float distance = cam.transform.position.x * parallaxEffect;
            float movement = cam.transform.position.x * (1 - parallaxEffect);

            transform.position =
                new Vector3(_startPosition + distance,
                    cam.transform.position.y,
                    transform.position.z);

            if (movement > _startPosition + _length)
            {
                _startPosition += _length;
            }
            else if (movement < _startPosition - _length)
            {
                _startPosition -= _length;
            }
        }
    }
}
