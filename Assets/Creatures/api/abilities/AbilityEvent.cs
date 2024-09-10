using Creatures.api.abilities.states;
using Creatures.Api;

namespace Creatures.api.abilities
{
    /**
     * Signals that ability event occured, e.g. when a button
     * corresponding to an ability is pressed.
     *
     * This uses double dispatch so the caller doesn't need to
     * do any if checks or instanceOf checks to know which
     * ability event method to call on IPlayable.
     */
    public abstract class AbilityEvent
    {
        protected IAbilityState currentState = new ReadyState();

        public AbilityExecutionResult Execute(IPlayable playable)
        {
            return currentState.Execute(playable, this);
        }

        public abstract AbilityIdentifier getId();

        public abstract void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown);
        public abstract void Unsubscribe(IStateNotifier notifier, CooldownState state);

        public abstract AbilityExecutionResult ExecuteAbility(IPlayable playable);
        public void SetState(IAbilityState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
