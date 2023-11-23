using System;
using Features.Environment.Coins.Impl;
using Features.Shared;
using UnityEngine;

namespace Features.Bots.Impl
{
    public class Bot : CanFly, IMortal
    {
        public event Action<string, bool> BotDestroyed = delegate {  };

        [SerializeField]
        private Coin _coin;
        [SerializeField]
        private ParticleSystem _destroyParticle;
        [SerializeField]
        private ParticleSystem _smokeParticle;
        [SerializeField] 
        private int _health = 10;

        private int _currentHealth;
        private string _botId;

        protected bool IsAlive => _currentHealth > 0;

        public void SetData(string botId)
        {
            _botId = botId;
            _currentHealth = _health;
        }
        
        public void Damage(int damageValue, bool byPlayer)
        {
            _currentHealth -= damageValue;
            if (!IsAlive)
            {
                Destroy(byPlayer);
            }
            else
            {
                if (_smokeParticle && !_smokeParticle.isPlaying)
                {
                    _smokeParticle.Play();
                }
            }
        }

        public void FullDamage()
        {
            Destroy(false);
        }

        private void Destroy(bool byPlayer)
        {
            if (_coin && byPlayer)
            {
                var coin = Instantiate(_coin, null);
                coin.transform.position = transform.position;
            }
            var particle = Instantiate(_destroyParticle, null);
            particle.transform.position = transform.position;
            BotDestroyed(_botId, byPlayer);
            GameObject.Destroy(gameObject);
        }
    }
}