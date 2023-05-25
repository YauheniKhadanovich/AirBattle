using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Plane;
using Modules.BotSpawn.Data;
using Modules.BotSpawn.Facade;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.Spawner.Impl
{
    public class BotSpawner : MonoBehaviour, IBotSpawner, IInitializable, IDisposable
    {
        private readonly Vector3 groundPosition = Vector3.zero;

        [Inject] 
        private IBotSpawnFacade _botSpawnFacade;
        [SerializeField] 
        private Transform[] _spawnPositions;
        // TODO: move to the separate file 
        [SerializeField] 
        private List<BotPrefabsData> _botPrefabs;
        [SerializeField] 
        private PlaneInputHandler _planeInputHandler;

        public void Initialize()
        {
            _botSpawnFacade.NeedSpawnBot += Spawn;
        }

        private void Start()
        {
            // TODO: after Play Button
            _botSpawnFacade.InitBots();
        }

        public void Dispose()
        {
            _botSpawnFacade.NeedSpawnBot -= Spawn;
        }

        public void Spawn(BotInfo botInfo)
        {
            StartCoroutine(SpawnCoroutine(botInfo));
        }

        private IEnumerator SpawnCoroutine(BotInfo botInfo)
        {
            yield return new WaitForSeconds(botInfo.SpawnDelay);
            var botPrefab = _botPrefabs.First(item => item.BotType == botInfo.BotType).BotPrefab;
            var pos = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            var bot = Instantiate(botPrefab, null);
            bot.transform.position = pos;
            bot.transform.LookAt(groundPosition);
            bot.BotSpawned += _botSpawnFacade.OnBotSpawned;
            bot.BotDestroyed += _botSpawnFacade.OnBotDestroyed;
            yield return null;
        }
    }
}