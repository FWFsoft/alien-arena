using Creatures.Api;


using UnityEngine;


namespace Creatures.impl
{
    public class Serpentide : Venoxarid
    {


        void Start()
        {
            Speed = 0.35f;
            HealthScript = gameObject.AddComponent<HealthScript>();
            HealthScript.health = 300;


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
