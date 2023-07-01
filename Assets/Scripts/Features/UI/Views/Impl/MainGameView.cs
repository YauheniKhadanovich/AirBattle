using System;
using Features.UI.Views.Components;
using UnityEngine;

namespace Features.UI.Views.Impl
{
    public class MainGameView : MonoBehaviour, IMainGameView
    {
        public event Action GoClicked = delegate { };
        public event Action RestartClicked = delegate { };

        [SerializeField] 
        private MainButton _goButton;
        [SerializeField] 
        private MainButton _restartButton;
        [SerializeField] 
        private GameObject _footPane;
        [SerializeField] 
        private PointsField _pointsField;
        
        public void Awake()
        {
            _goButton.Button.onClick.AddListener(GoOnClick);
            _restartButton.Button.onClick.AddListener(RestartOnClick);
        }
        
        private void Start()
        {
            _goButton.EnableButton();
            _restartButton.DisableButton();
            HideFootPanel();
        }

        public void SetPointsCount(int points)
        {
            _pointsField.Set(points);
        }
        
        public void ShowRestartButton()
        {
            _goButton.DisableButton();
            _restartButton.EnableButton();
            HideFootPanel();
        }

        private void GoOnClick()
        {
            GoClicked.Invoke();
            _goButton.DisableButton();
            ShowFootPanel();
        }
        
        private void RestartOnClick()
        {
            RestartClicked.Invoke();
            _restartButton.DisableButton();
            ShowFootPanel();
        }

        private void ShowFootPanel()
        {
            _footPane.SetActive(true);
        }
        
        private void HideFootPanel()
        {
            _footPane.SetActive(false);
        }
    }
}