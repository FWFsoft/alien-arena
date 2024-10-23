using UnityEngine;

using Creatures.api.abilities.states;
using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Begin charging up the charged ability.
     */
    public class StartChargingEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable, Vector3 mousePosition)
        {
            return playable.StartCharging(this, mousePosition);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.StartCharging;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown)
        {
            notifier.Subscribe(
                state,
                getId(),
                notifier.GetStartChargingCooldown(isTriggeredByGlobalCooldown)
                );
        }

        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Unsubscribe(state, this.getId());
        }
    }
}
