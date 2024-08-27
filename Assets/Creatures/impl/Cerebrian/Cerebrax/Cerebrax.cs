using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{
    public class Cerebrax : Cerebrian
    {
        
        void Start()
        {
            Speed = 0.5f;
            HealthScript = gameObject.AddComponent<HealthScript>();
            HealthScript.health = 100;

            // Add custom initialization logic here
        }

        // Optional: Override the Update method if custom logic is needed
        protected override void Update()
        {
            base.Update();

            // Add custom update logic here, if necessary
        }
    }
}
