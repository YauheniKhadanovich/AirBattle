using UnityEngine;

namespace Features.Plane.Components
{
    public class PlaneInputHandler : MonoBehaviour
    {
        public Vector2 MovementState => _playerInput.Player.Move.ReadValue<Vector2>();
        public bool IsFirePressed => _playerInput.Player.Fire.IsPressed();
        public bool IsTest => _playerInput.Player.Test.WasReleasedThisFrame();
        
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void Start()
        {
            _playerInput.Player.Enable();
        }
    }
}