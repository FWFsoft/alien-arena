using Creatures.Api;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;
using Creatures.api.abilities.states;

namespace Creatures.api.abilities
{
    public interface IStateNotifier
    {
        void Subscribe(CooldownState cooldownState, BasicAttackEvent basicAttackEvent);
        void Subscribe(CooldownState cooldownState, StartChargingEvent startChargingEvent);
        void Subscribe(CooldownState cooldownState, InterruptChargingEvent interruptChargingEvent);
        void Subscribe(CooldownState cooldownState, ChargedAbilityEvent chargedAbilityEvent);
        void Subscribe(CooldownState cooldownState, CharacterAbilityEvent characterAbilityEvent);
        void Subscribe(CooldownState cooldownState, MobilityAbilityEvent mobilityAbilityEvent);
        void Subscribe(CooldownState cooldownState, CoreInfusionAbilityEvent coreInfusionAbilityEvent);
        void Unsubscribe(CooldownState cooldownState, BasicAttackEvent basicAttackEvent);
        void Unsubscribe(CooldownState cooldownState, StartChargingEvent startChargingEvent);
        void Unsubscribe(CooldownState cooldownState, InterruptChargingEvent interruptChargingEvent);
        void Unsubscribe(CooldownState cooldownState, ChargedAbilityEvent chargedAbilityEvent);
        void Unsubscribe(CooldownState cooldownState, CharacterAbilityEvent characterAbilityEvent);
        void Unsubscribe(CooldownState cooldownState, MobilityAbilityEvent mobilityAbilityEvent);
        void Unsubscribe(CooldownState cooldownState, CoreInfusionAbilityEvent coreInfusionAbilityEvent);
        void Subscribe(DisabledState disabledState);
        void Unsubscribe(DisabledState disabledState);
    }
}