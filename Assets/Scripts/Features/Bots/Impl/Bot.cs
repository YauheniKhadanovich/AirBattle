using System;
using Features.Shared;
using Modules.GameController.Data;
using UnityEngine;

namespace Features.Bots.Impl
{
    public class Bot : CanFly, IMortal
    {
        public event Action<BotType, bool> BotDestroyed = delegate {  };
        public event Action<BotType> BotSpawned = delegate {  };

        [SerializeField] 
        private BotType _botType;
        [SerializeField]
        private ParticleSystem _destroyParticle;
        [SerializeField] 
        private int _health = 10;

        private void Start()
        {
            BotSpawned(_botType);
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
            BotDestroyed(_botType, byPlayer);
            GameObject.Destroy(gameObject);
        }
    }
}