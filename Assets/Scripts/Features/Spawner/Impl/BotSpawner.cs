using System.Collections;
using Features.Bots;
using Features.Bots.Impl;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Spawner.Impl
{
    public class BotSpawner : MonoBehaviour
    {
        private readonly Vector3 groundPosition = Vector3.zero;
        
        [SerializeField] 
        private Transform[] _spawnPositions;
        [SerializeField] 
        private AerostatController _greenAerostat;
        
        private void Start()
        {
            StartCoroutine(nameof(SpawnCoroutine));
        }

        private IEnumerator SpawnCoroutine()
        {
            yield return new WaitForSeconds(3f);
            Spawn();
        }
        
        private void Spawn()
        {
            var pos = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            var bot = GameObject.Instantiate(_greenAerostat, null);
            bot.transform.position = pos;
            bot.transform.LookAt(groundPosition);
        }
    }
}