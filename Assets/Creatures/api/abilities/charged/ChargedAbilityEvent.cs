using Creatures.Api;
using Creatures.api.abilities.states;

namespace Creatures.api.abilities.charged
{
    /**
     * Actually fire the charged ability.
     */
    public class ChargedAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.ChargedAbility(this);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.ChargedAbility;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state)
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
