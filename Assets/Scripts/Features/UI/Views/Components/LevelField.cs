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

        public void SetLevelProgress(int currentPoints, int targetPoints)
        {
            _progressbar.value = Mathf.Clamp01((float)currentPoints / (float)targetPoints);
        }
        
        public void SetLevelId(int levelId)
        {
            _levelNum.text = levelId.ToString();
        }
    }
}