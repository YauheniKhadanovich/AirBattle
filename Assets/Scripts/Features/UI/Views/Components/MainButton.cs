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

        public Button Button { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
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