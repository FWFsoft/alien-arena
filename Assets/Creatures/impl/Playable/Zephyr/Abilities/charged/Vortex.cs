using System.Collections;
using UnityEngine;

namespace Creatures.impl.Playable.Zephyr
{
    public class Vortex : MonoBehaviour
    {
        [SerializeField]
        public GameObject vortex;
        [SerializeField] 
        private GameObject channelTornadoEffect;
        [SerializeField] 
        private GameObject vortexEffect;
        [SerializeField] 
        private GameObject smokeRingEffect;
       
        private bool IsCharging { get; set; }
        private Animator vortexAnimator;
        private Animator channelTornadoAnimator;
        private Animator vortexExplosionAnimator;
        private Animator smokeRingAnimator;
        private Camera mainCamera;

        public Vortex()
        {
            IsCharging = false;    
        }

        void Start()
        {

            vortexAnimator = vortex.GetComponent<Animator>();
            channelTornadoAnimator = channelTornadoEffect.GetComponent<Animator>();
            vortexExplosionAnimator = vortexEffect.GetComponent<Animator>();
            smokeRingAnimator = smokeRingEffect.GetComponent<Animator>();
            mainCamera = Camera.main;
            vortex.SetActive(false);
            channelTornadoEffect.SetActive(false);
            vortexEffect.SetActive(false);
            smokeRingEffect.SetActive(false);
            Debug.Log("Vortex set to inactive initially.");
        }

        void Update()
        {
            if (IsCharging)
            {
                Debug.Log("IsCharging is true, updating vortex position.");
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

                channelTornadoEffect.SetActive(true);
                vortexEffect.SetActive(true);
                smokeRingEffect.SetActive(true);

                channelTornadoAnimator.SetTrigger("Release");
                vortexExplosionAnimator.SetTrigger("Release");
                smokeRingAnimator.SetTrigger("Release");


                //TODO: Make this the same duration as the release animations once I tune those
                StartCoroutine(DeactivateVortexAfterDelay(0.5f));

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
            if (mainCamera == null)
            {
                Debug.LogError("mainCamera is null in UpdateVortexPosition. Cannot update position.");
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10.0f;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            if (vortex != null)
            {
                vortex.transform.position = worldPosition;
                channelTornadoEffect.transform.position = worldPosition;
                vortexEffect.transform.position = worldPosition;
                smokeRingEffect.transform.position = worldPosition;
                Debug.Log($"Vortex position updated to {worldPosition}");
            }
            else
            {
                Debug.LogError("vortex GameObject is null in UpdateVortexPosition. Cannot update position.");
            }        
        }

        private IEnumerator DeactivateVortexAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            channelTornadoEffect.SetActive(false);
            vortexEffect.SetActive(false);
            smokeRingEffect.SetActive(false);
            vortex.SetActive(false);
        }
    }
}
