using Creatures.api.abilities.states;
using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Interrupts charging, stopping a character from
     * charging their ability without using it.
     *
     * NOTE: This won't necessarily occur when a player
     * presses a button and is likely an event fired
     * when a player enters a status effect where they
     * would not be allowed to charge their charged ability
     */
    public class InterruptChargingEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.InterruptCharging(this);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.InterruptCharging;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown)
        {
            // Instantly complete the cooldown, since InterruptCharging shouldn't have a CD
            state.OnComplete();
        }

        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            // No-op since this should never be on CD
        }
    }
}
