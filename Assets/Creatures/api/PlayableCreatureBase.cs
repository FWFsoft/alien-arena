using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;
using Creatures.api.abilities.states;
using Creatures.Api;

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

        // Isometric movement vectors
        private Vector2 isoUpRight = new Vector2(1, 0.5f);
        private Vector2 isoUpLeft = new Vector2(-1, 0.5f);
        private Vector2 isoDownRight = new Vector2(1, -0.5f);
        private Vector2 isoDownLeft = new Vector2(-1, -0.5f);

        public T GetAbility<T>() where T : AbilityEvent
        {
            return abilityEvents.OfType<T>().FirstOrDefault();
        }

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
        public abstract AbilityExecutionResult BasicAttack(BasicAttackEvent basicAttackEvent, Vector3 mousePosition);
        public abstract AbilityExecutionResult StartCharging(StartChargingEvent startChargingEvent, Vector3 mousePosition);
        public abstract AbilityExecutionResult InterruptCharging(InterruptChargingEvent interruptChargingEvent, Vector3 mousePosition);
        public abstract AbilityExecutionResult ChargedAbility(ChargedAbilityEvent chargedAbilityEvent, Vector3 mousePosition);
        public abstract AbilityExecutionResult CharacterAbility(CharacterAbilityEvent characterAbilityEvent, Vector3 mousePosition);
        public abstract AbilityExecutionResult MobilityAbility(MobilityAbilityEvent mobilityAbilityEvent, Vector3 mousePosition);
        public abstract AbilityExecutionResult CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent, Vector3 mousePosition);

        public virtual void HandleMovement(Vector2 inputDirection)
        {
            // Initialize movement based on input
            Vector3 movement = Vector3.zero;

            if (inputDirection.y > 0 && inputDirection.x > 0) // W + D
            {
                movement = new Vector3(isoUpRight.x, isoUpRight.y, 0);
            }
            else if (inputDirection.y > 0 && inputDirection.x < 0) // W + A
            {
                movement = new Vector3(isoUpLeft.x, isoUpLeft.y, 0);
            }
            else if (inputDirection.y < 0 && inputDirection.x > 0) // S + D
            {
                movement = new Vector3(isoDownRight.x, isoDownRight.y, 0);
            }
            else if (inputDirection.y < 0 && inputDirection.x < 0) // S + A
            {
                movement = new Vector3(isoDownLeft.x, isoDownLeft.y, 0);
            }
            else if (inputDirection.y > 0) // W
            {
                movement = new Vector3(0, 1, 0);
            }
            else if (inputDirection.y < 0) // S
            {
                movement = new Vector3(0, -1, 0);
            } 
            else if (inputDirection.x > 0) // D
            {
                movement = new Vector3(1, 0, 0);
            }
            else if (inputDirection.x < 0) // A
            {
                movement = new Vector3(-1, 0, 0);
            }

            // Normalize movement for consistent speed and apply speed and time
            movement = movement.normalized * Speed * Time.deltaTime;

            // Move character
            transform.Translate(movement);

            // Set Animator Parameters
            Animator.SetFloat("DirectionX", inputDirection.x);
            Animator.SetFloat("DirectionY", inputDirection.y);
        }
    }
}
