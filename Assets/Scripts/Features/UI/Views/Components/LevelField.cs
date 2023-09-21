using Modules.GameController.Models.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Views.Components
{
    public class LevelField : MonoBehaviour
    {
        [SerializeField] 
        private Slider _progressbar;

        [SerializeField] 
        private TMP_Text _levelNum;

        public void SetLevel(Level level)
        {
            _progressbar.value = level.Progress / level.MaxProgress;
            _levelNum.text = level.LevelNum.ToString();
        }
    }
}