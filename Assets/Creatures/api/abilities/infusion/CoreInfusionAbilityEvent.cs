using Creatures.Api;
using Creatures.api.abilities.states;

namespace Creatures.api.abilities.infusion
{
    /**
     * The first infusion a player equips will grant them
     * an ability based on the genera associated with the
     * infusion.
     */
    public class CoreInfusionAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.CoreInfusionAbility(this);
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