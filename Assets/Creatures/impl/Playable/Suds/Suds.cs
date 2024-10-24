using UnityEngine;

using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;

namespace Creatures.impl.Playable.Suds
{
public class Suds : PlayableCreatureBase
    {
        [SerializeField]
        public GameObject bullet;
        [SerializeField]
        public Transform bulletExitTransform;

        private const float BasicAttackCooldown = 1.0f;
        private float force = 0.1f;

        public override float GetBasicAttackCooldown(bool isTriggeredByGlobalCooldown)
        {
            return BasicAttackCooldown;
        }

        public override float GetStartChargingCooldown(bool isTriggeredByGlobalCooldown)
        {
            throw new System.NotImplementedException();
        }

        public override float GetCharacterAbilityCooldown(bool isTriggeredByGlobalCooldown)
        {
            throw new System.NotImplementedException();
        }

        public override float GetMobilityAbilityCooldown(bool isTriggeredByGlobalCooldown)
        {
            throw new System.NotImplementedException();
        }

        public override float GetCoreInfusionAbilityCooldown(bool isTriggeredByGlobalCooldown)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult BasicAttack(BasicAttackEvent basicAttackEvent, Vector3 mousePosition)
        {
            var bulletInstance = Instantiate(bullet, bulletExitTransform.position, Quaternion.identity);
            var projectileBehavior = bulletInstance.GetComponent<ProjectileBehavior>();
            projectileBehavior.explosionPosition = mousePosition;

            var rigidBody = projectileBehavior.rigidBody;

            Vector3 direction = mousePosition - transform.position;
            rigidBody.velocity = new Vector2(direction.x, direction.y).normalized * force;

            return AbilityExecutionResult.Success;
        }

        public override AbilityExecutionResult StartCharging(StartChargingEvent startChargingEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult InterruptCharging(InterruptChargingEvent interruptChargingEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult ChargedAbility(ChargedAbilityEvent chargedAbilityEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult CharacterAbility(CharacterAbilityEvent characterAbilityEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult MobilityAbility(MobilityAbilityEvent mobilityAbilityEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }
    }
}
