using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Views.Components
{
    public class PointsField : MonoBehaviour
    {
        [SerializeField] 
        private Image _balloonImage;
        [SerializeField] 
        private TextMeshProUGUI _points;
        [SerializeField] 
        private Animator _ballonAnimationPrefab;

        public void Set(int point)
        {
            _points.text = point.ToString();
            SpawnBalloonAnimation();
        }

        private void SpawnBalloonAnimation()
        {
            var g = Instantiate(_ballonAnimationPrefab, _balloonImage.transform);
            var transform1 = g.transform;
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
            Destroy(g.gameObject,3f);
        }
    }
}