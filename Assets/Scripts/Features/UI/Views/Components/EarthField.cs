using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Views.Components
{
    public class EarthField : MonoBehaviour
    {
        [SerializeField] 
        private Slider _slider;

        public void Set(float value)
        {
            _slider.value = value / 100f;
        }
    }
}