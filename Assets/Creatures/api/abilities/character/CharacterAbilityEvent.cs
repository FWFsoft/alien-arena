using UnityEngine;

using Creatures.api.abilities.states;
using Creatures.Api;

namespace Creatures.api.abilities.character
{
    /**
     * Character Abilities define the one button each character
     * has with no common use pattern, it has a cooldown and
     * varies greatly across characters
     */
    public class CharacterAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable, Vector3 mousePosition)
        {
            return playable.CharacterAbility(this, mousePosition);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.CharacterAbility;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown)
        {
            notifier.Subscribe(
                state,
                getId(),
                notifier.GetCharacterAbilityCooldown(isTriggeredByGlobalCooldown)
                );
        }

        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Unsubscribe(state, this.getId());
        }
    }
}
