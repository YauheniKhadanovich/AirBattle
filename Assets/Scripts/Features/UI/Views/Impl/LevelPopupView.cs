using TMPro;
using UnityEngine;

namespace Features.UI.Views.Impl
{
    public class LevelPopupView : BasePopupView, ILevelPopupView
    {
        [SerializeField] 
        private TMP_Text _levelText;
        
        public void OnVewShown()
        {
            CloseView();
        }

        public void SetData(int level)
        {
            _levelText.text = $"Level {level}";
        }
    }
}