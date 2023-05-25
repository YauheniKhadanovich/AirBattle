using UnityEngine;

namespace Features
{
    public class CanFly : MonoBehaviour
    {
        private readonly Vector3 _groundPosition = Vector3.zero;
        
        [SerializeField] 
        protected Transform _bodyDirection; 
        [SerializeField]
        public float _speed = 30f;
        
        protected void MoveForward()
        {
            transform.RotateAround(_groundPosition, _bodyDirection.forward, Time.deltaTime * _speed);
        }
    }
}