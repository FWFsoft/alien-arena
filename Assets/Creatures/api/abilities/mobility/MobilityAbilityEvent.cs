using Creatures.Api;
using Creatures.api.abilities.states;

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
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.MobilityAbility(this);
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