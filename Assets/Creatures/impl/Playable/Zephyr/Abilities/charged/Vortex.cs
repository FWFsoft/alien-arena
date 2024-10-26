using UnityEngine;

namespace Creatures.impl.Playable.Zephyr
{
    public class Vortex : MonoBehaviour
    {
        private bool IsCharging { get; set; }

        public Vortex()
        {
            
        }

        public void startCharging()
        {
            IsCharging = true;
            Debug.Log("Charging Started ...");
        }

        public void releaseCharging()
        {
            if (IsCharging) 
            {
                IsCharging = false;
                Debug.Log("Charging Released ...");
            }
            else
            {
                Debug.Log("Charging was interrupted before the button was released, do nothing."); 
            }
        }

        public void interruptCharging()
        {
            Debug.Log("Charging Interrupted ...");
        }
    }
}
