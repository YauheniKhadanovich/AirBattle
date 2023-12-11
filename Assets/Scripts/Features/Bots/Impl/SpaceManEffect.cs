using UnityEngine;

namespace Features.Bots.Impl
{
    public class SpaceManEffect : MonoBehaviour
    {
        private Vector3 _direction = Vector3.back;
        private float _speed = 1f;
        
        private void Start()
        {
            _speed = Random.Range(1f, 4f);
            Vector3[] possibleDirections = { Vector3.back, Vector3.forward, Vector3.left, Vector3.right };
            _direction = possibleDirections[Random.Range(0, possibleDirections.Length)];
        }

        private void Update()
        {
            transform.Rotate(_direction, _speed * Time.deltaTime, Space.Self);
        }
    }
}