using System.Collections.Generic;
using Features.Aircraft.Components;
using UnityEngine;

namespace Modules.GameController.Repositories.Impl
{
    [CreateAssetMenu(fileName = "AircraftData", menuName = "AirBattle/Aircrafts/Generate Data", order = 1)]
    public class AircraftRepository : ScriptableObject
    {
        [SerializeField]
        private List<AircraftTo> _aircraftTos;

        private int _currentBodyIndex = 0;
        
        public List<AircraftTo> AircraftTos => _aircraftTos;

        public AircraftBody GetCurrentBody()
        {
            return _aircraftTos[_currentBodyIndex]._prefab;
        }
    }
}