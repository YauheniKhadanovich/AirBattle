using UnityEngine;

namespace Features.Plane
{
    public class PlaneInputHandler : MonoBehaviour
    {
        public Vector2 MovementState => _playerInput.Player.Move.ReadValue<Vector2>();
        public bool IsFirePressed => _playerInput.Player.Fire.IsPressed();
        
        private PlayerInput _playerInput;

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