using TMPro;
using UnityEngine;

namespace Features.UI.Views.Components
{
    public class PointsField : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _points;

        private int _pointsValue;
        
        public void Set(int point)
        {
            if (point == _pointsValue)
            {
                return;
            }

            if (point > _pointsValue)
            {
                AddPointEffect();
            }
            else if (point < _pointsValue)
            {
                RemovePointEffect();
            }

            _pointsValue = point;
            _points.text = _pointsValue.ToString();
        }

        private void AddPointEffect()
        {
            //
        }
        
        private void RemovePointEffect()
        {
            //
        }
    }
}