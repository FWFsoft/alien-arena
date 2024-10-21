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
        public override float GetBasicAttackCooldown(bool isTriggeredByGlobalCooldown)
        {
            throw new System.NotImplementedException();
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

        public override AbilityExecutionResult BasicAttack(BasicAttackEvent basicAttackEvent)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult StartCharging(StartChargingEvent startChargingEvent)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult InterruptCharging(InterruptChargingEvent interruptChargingEvent)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult ChargedAbility(ChargedAbilityEvent chargedAbilityEvent)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult CharacterAbility(CharacterAbilityEvent characterAbilityEvent)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult MobilityAbility(MobilityAbilityEvent mobilityAbilityEvent)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityExecutionResult CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}
