using System.Collections.Generic;
using UnityEngine;

namespace Modules.GameController.Data.Impl
{
    [CreateAssetMenu(fileName = "BotsData", menuName = "Bots/Generate Bots Data", order = 1)]
    public class BotsScriptableObject : ScriptableObject
    {
       public List<BotTo> Bots;
    }
}