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
