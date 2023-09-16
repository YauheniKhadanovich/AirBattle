using System;
using Features.Shared;
using UnityEngine;

namespace Features.Bots.Impl
{
    public class Bot : CanFly, IMortal
    {
        public event Action<string, bool> BotDestroyed = delegate {  };
        
        [SerializeField]
        private ParticleSystem _destroyParticle;
        [SerializeField] 
        private int _health = 10;

        private string _botId;

        public void SetData(string botId)
        {
            _botId = botId;
        }
        
        public void Damage(int damageValue, bool byPlayer)
        {
            _health -= damageValue;
            if (_health <= 0)
            {
                Destroy(byPlayer);
            }
        }

        public void FullDamage()
        {
            Destroy(false);
        }

        private void Destroy(bool byPlayer)
        {
            var particle = Instantiate(_destroyParticle, null);
            particle.transform.position = transform.position;
            BotDestroyed(_botId, byPlayer);
            GameObject.Destroy(gameObject);
        }
    }
}