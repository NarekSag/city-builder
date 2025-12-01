using Infrastructure.Grid;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameFlow : IStartable
    {
        [Inject] private GridInitializer _gridInitializer;

        public void Start()
        {
            Debug.Log("[GameFlow] Initializing game flow...");

            _gridInitializer.Initialize();

            Debug.Log("[GameFlow] Game flow initialized.");
        }
    }
}

