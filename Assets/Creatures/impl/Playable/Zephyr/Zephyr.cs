using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;

using UnityEngine;
using static UnityEngine.Physics2D;

namespace Creatures.impl.Playable.Zephyr
{
    public class Zephyr : PlayableCreatureBase
    {
        [SerializeField]
        public Vortex vortex;
        [SerializeField]
        public GameObject swipe;

        private const float BasicAttackCooldown = 1.0f;
        private const float StartChargingCooldown = 2.5f;
        private const float CharacterAbilityCooldown = 5.0f;
        private const float MobilityAbilityCooldown = 3.0f;
        private const float CoreInfusionAbilityCooldown = 7.0f;

        private const float GlobalCooldown = 0.5f;

        private float force = 2.0f;
        private float dashSpeed = 50.0f;
        private float dashDuration = 0.3f;
        private float swipeRange = 5.0f; // Range of the swipe attack
        private float knockbackForce = 10.0f; // Force applied to enemies hit by the swipe attack

        public override float GetBasicAttackCooldown(bool isTriggeredByGlobalCooldown)
        {
            return isTriggeredByGlobalCooldown ? GlobalCooldown : BasicAttackCooldown;
        }

        public override float GetStartChargingCooldown(bool isTriggeredByGlobalCooldown)
        {
            return isTriggeredByGlobalCooldown ? GlobalCooldown : StartChargingCooldown;
        }

        public override float GetCharacterAbilityCooldown(bool isTriggeredByGlobalCooldown)
        {
            return isTriggeredByGlobalCooldown ? GlobalCooldown : CharacterAbilityCooldown;
        }

        public override float GetMobilityAbilityCooldown(bool isTriggeredByGlobalCooldown)
        {
            return isTriggeredByGlobalCooldown ? GlobalCooldown : MobilityAbilityCooldown;
        }

        public override float GetCoreInfusionAbilityCooldown(bool isTriggeredByGlobalCooldown)
        {
            return isTriggeredByGlobalCooldown ? GlobalCooldown : CoreInfusionAbilityCooldown;
        }

        public override AbilityExecutionResult BasicAttack(BasicAttackEvent basicAttackEvent, Vector3 mousePosition)
        {
            // Calculate direction of the swipe attack from Zephyr to mouse position
            Vector3 direction = (mousePosition - transform.position).normalized;

            // Perform a raycast in the direction of the swipe to detect enemies
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, swipeRange);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    // Apply knockback force to the enemy
                    Rigidbody2D enemyRigidbody = hit.collider.GetComponent<Rigidbody2D>();
                    if (enemyRigidbody != null)
                    {
                        Vector2 knockbackDirection = (hit.collider.transform.position - transform.position).normalized;
                        enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }

            // Enable the swipe GameObject, set its position and rotation
            swipe.SetActive(true);
            swipe.transform.position = transform.position + direction * swipeRange;

            // Rotate the swipe animation to face towards the mouse position (corrected for opposite direction)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180.0f;
            swipe.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Play the swipe animation using the Animator attached to the swipe GameObject
            Animator swipeAnimator = swipe.GetComponent<Animator>();
            if (swipeAnimator != null)
            {
                swipeAnimator.SetTrigger("SwipeAttack");
            }

            // Disable the swipe object after the animation completes
            StartCoroutine(DisableSwipeAfterAnimation(swipeAnimator));

            return AbilityExecutionResult.Success;
        }

        private System.Collections.IEnumerator DisableSwipeAfterAnimation(Animator swipeAnimator)
        {
            // Wait for the animation to complete
            yield return new WaitForSeconds(swipeAnimator.GetCurrentAnimatorStateInfo(0).length);
            swipe.SetActive(false);
        }


        public override AbilityExecutionResult StartCharging(StartChargingEvent startChargingEvent, Vector3 mousePosition)
        {
            vortex.startCharging();
            return AbilityExecutionResult.Success;
        }

        public override AbilityExecutionResult InterruptCharging(InterruptChargingEvent interruptChargingEvent, Vector3 mousePosition)
        {
            vortex.interruptCharging(); 
            return AbilityExecutionResult.Success;
        }

        public override AbilityExecutionResult ChargedAbility(ChargedAbilityEvent chargedAbilityEvent, Vector3 mousePosition)
        {
            vortex.releaseCharging();
            return AbilityExecutionResult.Success;
        }

        public override AbilityExecutionResult CharacterAbility(CharacterAbilityEvent characterAbilityEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult MobilityAbility(MobilityAbilityEvent mobilityAbilityEvent, Vector3 mousePosition)
        {
            Vector3 direction = (mousePosition - transform.position).normalized;
            StartCoroutine(Dash(direction));
            return AbilityExecutionResult.Success;
        }

        public override AbilityExecutionResult CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }

        private System.Collections.IEnumerator Dash(Vector3 direction)
        {
            float startTime = Time.time;

            while (Time.time < startTime + dashDuration)
            {
                transform.Translate(direction * dashSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
