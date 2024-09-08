using System.Collections.Generic;
using Creatures.Api;
using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;
using Creatures.api.abilities.states;

namespace Creatures
{
    public abstract class PlayableCreatureBase : Creature, IPlayable
    {        
        public ICoreInfusion CoreInfusion { get; set; }
        private readonly CooldownMediator _cooldownMediator = new CooldownMediator();
        private IEnumerable<AbilityEvent> abilityEvents = new List<AbilityEvent>
        {
            new BasicAttackEvent(),
            new CharacterAbilityEvent(),
            new ChargedAbilityEvent(),
            new InterruptChargingEvent(),
            new StartChargingEvent(),
            new CoreInfusionAbilityEvent(),
            new MobilityAbilityEvent()
        };
        
        public IEnumerable<AbilityEvent> GetAbilities()
        {
            return abilityEvents;
        }
        
        public void Subscribe(CooldownState cooldownState, AbilityIdentifier abilityIdentifier, float cooldownDuration)
        {
            _cooldownMediator.StartCooldown(cooldownState, abilityIdentifier, cooldownDuration);
        }

        public void Unsubscribe(CooldownState cooldownState, AbilityIdentifier abilityIdentifier)
        {
            _cooldownMediator.RefreshCooldown(abilityIdentifier);
        }

        public void triggerGlobalCooldown(AbilityEvent triggeringAbilityEvent)
        {
            foreach (var abilityEvent in GetAbilities())
            {
                // Skip the ability that triggered the global cooldown
                if (abilityEvent == triggeringAbilityEvent)
                {
                    continue;
                }
                abilityEvent.SetState(new GlobalCooldownState(this, abilityEvent));
            }
        }
        
        //TODO: Figure out how to handle DisabledState
        //TODO: Probably put this off to a later ticket because it's directly tied to the effects system
        public void Subscribe(DisabledState disabledState)
        {
            throw new System.NotImplementedException();
        }

        public void Unsubscribe(DisabledState disabledState)
        {
            throw new System.NotImplementedException();
        }
        
        public abstract float GetBasicAttackCooldown(bool isTriggeredByGlobalCooldown);
        public abstract float GetStartChargingCooldown(bool isTriggeredByGlobalCooldown);
        public abstract float GetCharacterAbilityCooldown(bool isTriggeredByGlobalCooldown);
        public abstract float GetMobilityAbilityCooldown(bool isTriggeredByGlobalCooldown);
        public abstract float GetCoreInfusionAbilityCooldown(bool isTriggeredByGlobalCooldown);
        public abstract AbilityExecutionResult BasicAttack(BasicAttackEvent basicAttackEvent);
        public abstract AbilityExecutionResult StartCharging(StartChargingEvent startChargingEvent);
        public abstract AbilityExecutionResult InterruptCharging(InterruptChargingEvent interruptChargingEvent);
        public abstract AbilityExecutionResult ChargedAbility(ChargedAbilityEvent chargedAbilityEvent);
        public abstract AbilityExecutionResult CharacterAbility(CharacterAbilityEvent characterAbilityEvent);
        public abstract AbilityExecutionResult MobilityAbility(MobilityAbilityEvent mobilityAbilityEvent);
        public abstract AbilityExecutionResult CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent);
    }
}