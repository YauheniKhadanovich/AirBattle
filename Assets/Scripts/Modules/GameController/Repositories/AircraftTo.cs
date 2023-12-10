using System;
using Features.Aircraft.Components;

namespace Modules.GameController.Repositories
{
    [Serializable]
    public class AircraftTo
    {
        public string _name;
        public AircraftBody _prefab;
    }
}