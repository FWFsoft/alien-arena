using Creatures.api.abilities.states;
using Creatures.Api;

namespace Creatures.api.abilities.basic
{
    public class BasicAttackEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.BasicAttack(this);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.BasicAttack;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown)
        {
            notifier.Subscribe(
                state,
                getId(),
                notifier.GetBasicAttackCooldown(isTriggeredByGlobalCooldown)
                );
        }

        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Unsubscribe(state, this.getId());
        }
    }
}
