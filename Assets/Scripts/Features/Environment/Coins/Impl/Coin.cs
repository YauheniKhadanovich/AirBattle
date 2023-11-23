using UnityEngine;

namespace Features.Environment.Coins.Impl
{
    public class Coin : MonoBehaviour, ICoin
    {
        public void Take()
        {
            Destroy(gameObject);
        }
    }
}