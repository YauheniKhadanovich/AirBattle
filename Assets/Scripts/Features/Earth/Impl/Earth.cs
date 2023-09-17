using System;
using UnityEngine;

namespace Features.Earth.Impl
{
    public class Earth : MonoBehaviour, IEarth
    {
        public event Action<float> EarthDamaged = delegate { };  
        
        public void Hit(float damageValue)
        {
            EarthDamaged.Invoke(damageValue);
        }
    }
}