using UnityEngine;

namespace Features.Earth.Impl
{
    public class Earth : MonoBehaviour, IEarth
    {
        private float _health = 100;
        
        public void Hit(float damageValue)
        {
            _health -= damageValue;
        }
    }
}