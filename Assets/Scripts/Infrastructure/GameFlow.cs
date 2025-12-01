using UnityEngine;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameFlow : IStartable
    {
        public void Start()
        {
            Debug.Log("[GameFlow] Initializing game flow...");
        }
    }
}

