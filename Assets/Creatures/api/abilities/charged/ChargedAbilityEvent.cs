using UnityEngine;

using Creatures.api.abilities.states;
using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Actually fire the charged ability.
     */
    public class ChargedAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable, Vector3 mousePosition)
        {
            return playable.ChargedAbility(this, mousePosition);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.ChargedAbility;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown)
        {
            // Instantly complete the cooldown, since ChargedAbility shouldn't have a CD
            state.OnComplete();
        }

        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            // No-op since this should never be on CD
        }
    }
}
