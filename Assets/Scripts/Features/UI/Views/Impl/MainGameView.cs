using System;
using Features.UI.Views.Components;
using UnityEngine;

namespace Features.UI.Views.Impl
{
    public class MainGameView : MonoBehaviour, IMainGameView
    {
        public event Action GoClicked = delegate { };

        [SerializeField] 
        private MainButton _goButton;
        [SerializeField] 
        private GameObject _headPanel;
        [SerializeField] 
        private GameObject _footPanel;
        [SerializeField] 
        private PointsField _pointsField;
        [SerializeField] 
        private LevelField _levelField;
        
        public void Awake()
        {
            _goButton.Button.onClick.AddListener(GoOnClick); 
        }
        
        private void Start()
        {
            _goButton.EnableButton();
            _headPanel.SetActive(false);
            _footPanel.SetActive(false);
        }

        public void SetPointsCount(int points)
        {
            _pointsField.Set(points);
        }
        
        public void SetLevelId(int levelId)
        {
            _levelField.SetLevelId(levelId);
        }
        
        public void SetLevelProgress(int currentPoints, int targetPoints)
        {
            _levelField.SetLevelProgress(currentPoints, targetPoints);
        }

        public void ShowRestartButton()
        {
            _goButton.DisableButton();
            _footPanel.SetActive(false);
        }

        private void GoOnClick()
        {
            GoClicked.Invoke();
            _goButton.DisableButton();
            _headPanel.SetActive(true);
            _footPanel.SetActive(true);
        }
    }
}