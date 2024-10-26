using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;

using UnityEngine;

namespace Creatures.impl.Playable.Zephyr
{
    public class Zephyr : PlayableCreatureBase
    {
        [SerializeField]
        public Vortex vortex;

        private const float BasicAttackCooldown = 1.0f;
        private const float StartChargingCooldown = 2.5f;
        private const float CharacterAbilityCooldown = 5.0f;
        private const float MobilityAbilityCooldown = 3.0f;
        private const float CoreInfusionAbilityCooldown = 7.0f;

        private const float GlobalCooldown = 0.5f;

        private float force = 2.0f;

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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent, Vector3 mousePosition)
        {
            throw new System.NotImplementedException();
        }
    }
}
