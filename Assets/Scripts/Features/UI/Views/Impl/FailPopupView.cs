using System;
using Features.UI.Views.Components;
using UnityEngine;

namespace Features.UI.Views.Impl
{
    public class FailPopupView : BasePopupView, IFailPopupView
    {
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        public event Action RestartClicked = delegate { };
        
        [SerializeField] 
        private Animator _viewAnimator;
        [SerializeField] 
        private MainButton _restartButton;

        private void Awake()
        {
            _restartButton.Button.onClick.AddListener(RestartOnClick);
        }

        private void Start()
        {
            EnableView();
        }

        private void RestartOnClick()
        {
            RestartClicked.Invoke();
            CloseView();
        }

        private void EnableView()
        {
            _viewAnimator.SetBool(Enabled, true);
            _restartButton.EnableButton();
        }
    }
}