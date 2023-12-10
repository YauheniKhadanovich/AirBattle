using UnityEngine;

namespace Features.Plane.Components
{
    public class PlaneInputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;
        public Vector2 MovementState => _playerInput.Player.Move.ReadValue<Vector2>();
        public bool IsFirePressed => _playerInput.Player.Fire.IsPressed();

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