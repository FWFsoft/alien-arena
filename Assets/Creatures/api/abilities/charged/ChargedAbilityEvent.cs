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
            notifier.Subscribe(state, this);
        }
        
        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Unsubscribe(state, this);
        }
    }
}
