using Creatures.api.abilities.states;
using Creatures.Api;

using UnityEngine;

namespace Creatures.api.abilities.mobility
{
    /**
     * An ability that improves a character's mobility.
     *
     * NOTE: For Defenders, this should be an additional
     * character ability.
     */
    public class MobilityAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable, Vector3 mousePosition)
        {
            return playable.MobilityAbility(this, mousePosition);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.MobilityAbility;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown)
        {
            notifier.Subscribe(
                state,
                getId(),
                notifier.GetMobilityAbilityCooldown(isTriggeredByGlobalCooldown)
                );
        }

        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Unsubscribe(state, this.getId());
        }
    }
}
