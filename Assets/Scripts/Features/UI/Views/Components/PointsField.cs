using TMPro;
using UnityEngine;

namespace Features.UI.Views.Components
{
    public class PointsField : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _points;

        public void Set(int point)
        {
            _points.text = point.ToString();
        }
    }
}