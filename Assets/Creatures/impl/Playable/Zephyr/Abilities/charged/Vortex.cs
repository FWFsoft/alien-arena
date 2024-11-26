using System.Collections;

using UnityEngine;

namespace Creatures.impl.Playable.Zephyr
{
    public class Vortex : MonoBehaviour
    {
        [SerializeField]
        public GameObject vortex;
        [SerializeField]
        private GameObject dissipateEffect;
        [SerializeField]
        private GameObject explosionEffect;
        [SerializeField]
        private GameObject smokeRingEffect;
        [SerializeField]
        private float vortexScaleFactor = 1.0f;
        [SerializeField]
        private float smokeRingScaleFactor = 1.0f;
        [SerializeField]
        private float followSpeed = 1.0f;

        private bool IsCharging { get; set; }
        private Animator vortexAnimator;
        private Animator dissipateAnimator;
        private Animator explosionAnimator;
        private Animator smokeRingAnimator;
        private Camera mainCamera;
        private float positionYAdjustment = 0.445f;
        private float positionXAdjustment = 0.05f;

        public Vortex()
        {
            IsCharging = false;
        }

        void Start()
        {

            vortexAnimator = vortex.GetComponent<Animator>();
            dissipateAnimator = dissipateEffect.GetComponent<Animator>();
            explosionAnimator = explosionEffect.GetComponent<Animator>();
            smokeRingAnimator = smokeRingEffect.GetComponent<Animator>();
            mainCamera = Camera.main;
            vortex.SetActive(false);
            dissipateEffect.SetActive(false);
            explosionEffect.SetActive(false);
            smokeRingEffect.SetActive(false);
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
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.y += positionYAdjustment;
            worldPosition.x += positionXAdjustment;
            vortex.transform.position = worldPosition;
            Debug.Log("Charging Started ...");
        }

        public void releaseCharging()
        {
            if (IsCharging)
            {
                vortexAnimator.SetTrigger("Release"); // Release need to occur first to avoid a race condition
                IsCharging = false;
                vortexAnimator.SetBool("IsCharging", IsCharging);

                dissipateEffect.SetActive(true);
                explosionEffect.SetActive(true);
                smokeRingEffect.SetActive(true);

                explosionEffect.transform.localScale = Vector3.one * vortexScaleFactor;
                smokeRingEffect.transform.localScale = Vector3.one * smokeRingScaleFactor;

                dissipateAnimator.SetTrigger("Release");
                explosionAnimator.SetTrigger("Release");
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
            Vector3 adjustedPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            adjustedPosition.y += positionYAdjustment;
            adjustedPosition.x += positionXAdjustment;

            // Make the tornado lag behind the cursor a bit
            Vector3 smoothPosition = Vector3.MoveTowards(vortex.transform.position, adjustedPosition, followSpeed * Time.deltaTime);

            if (vortex != null)
            {
                vortex.transform.position = smoothPosition;
                dissipateEffect.transform.position = smoothPosition;
                explosionEffect.transform.position = smoothPosition;
                smokeRingEffect.transform.position = smoothPosition;
                Debug.Log($"Vortex position updated to {smoothPosition}");
            }
            else
            {
                Debug.LogError("vortex GameObject is null in UpdateVortexPosition. Cannot update position.");
            }
        }

        private IEnumerator DeactivateVortexAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            dissipateEffect.SetActive(false);
            explosionEffect.SetActive(false);
            smokeRingEffect.SetActive(false);
            vortex.SetActive(false);
        }
    }
}
