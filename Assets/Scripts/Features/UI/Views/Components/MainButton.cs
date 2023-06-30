using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Views.Components
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    public class MainButton : MonoBehaviour
    {
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        private Animator _animator;

        private Button _button;

        public Button Button
        {
            get
            {
                if (!_button)
                {
                    _button = GetComponent<Button>();
                }

                return _button;
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void EnableButton()
        {
            _animator.SetBool(Enabled, true);
            Button.enabled = true;
        }

        public void DisableButton()
        {
            _animator.SetBool(Enabled, false);
            Button.enabled = false;
        }
    }
}