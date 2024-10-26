using System.Collections;
using UnityEngine;

namespace Creatures.impl.Playable.Zephyr
{
    public class Vortex : MonoBehaviour
    {
        [SerializeField]
        public GameObject vortex;

        private bool IsCharging { get; set; }
        private Animator vortexAnimator;
        private Camera mainCamera;

        public Vortex()
        {
            IsCharging = false;    
        }

        void Start()
        {
            vortexAnimator = vortex.GetComponent<Animator>();
            mainCamera = Camera.main;
            vortex.SetActive(false);
        }

        void Update()
        {
            if (IsCharging)
            {
                UpdateVortexPosition();
            }
        }

        public void startCharging()
        {
            IsCharging = true;
            vortex.SetActive(true);
            vortexAnimator.SetBool("IsCharging", true);
            Debug.Log("Charging Started ...");
        }

        public void releaseCharging()
        {
            if (IsCharging) 
            {
                vortexAnimator.SetTrigger("Release"); // Release need to occur first to avoid a race condition
                IsCharging = false;
                vortexAnimator.SetBool("IsCharging", IsCharging);

                //TODO: Make this the same duration as the release animations once I tune those
                StartCoroutine(DeactivateVortexAfterDelay(2.0f));

                Debug.Log("Charging Released ...");
            }
            else
            {
                Debug.Log("Charging was interrupted before the button was released, do nothing."); 
            }
        }

        public void interruptCharging()
        {
            if (IsCharging)
            {
                IsCharging = false;
                vortexAnimator.SetBool("IsCharging", false);
                vortex.SetActive(false);
                Debug.Log("Charging Interrupted ...");
            }
        }

        private void UpdateVortexPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10.0f;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            vortex.transform.position = worldPosition;
        }

        private IEnumerator DeactivateVortexAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            vortex.SetActive(false);
        }
    }
}
