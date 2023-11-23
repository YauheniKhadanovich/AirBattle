using UnityEngine;

namespace Features.Bots.Impl
{
    public class SpaceManEffect : MonoBehaviour
    {
        private Vector3 _direction = Vector3.back;
        private float _speed = 1f;
        
        // TODO: refactoring
        private void Start()
        {
            _speed = Random.Range(1f, 4f);
            _direction = Random.Range(0, 3) switch
            {
                0 => Vector3.back,
                1 => Vector3.forward,
                2 => Vector3.left,
                3 => Vector3.right,
                _ => _direction
            };
        }

        private void Update()
        {
            transform.Rotate(_direction, _speed * Time.deltaTime, Space.Self);
        }
    }
}