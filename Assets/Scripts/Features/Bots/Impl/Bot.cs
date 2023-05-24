using UnityEngine;

namespace Features.Bots.Impl
{
    public class Bot : CanFlyController, IMortal
    {
        [SerializeField] 
        private int _health = 10;

        public void Damage(int value)
        {
            _health -= value;
            if (_health <= 0)
            {
                // TODO fix
                Destroy(gameObject);
            }
        }
    }
}