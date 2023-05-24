using UnityEngine;

namespace Plane
{
    public class MovementController : MonoBehaviour
    {
        private PlayerInput _playerInput;

        public Vector2 MovementState => _playerInput.Player.Move.ReadValue<Vector2>();
    
        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        void Start()
        {
            _playerInput.Player.Enable();
        }
    }
}
