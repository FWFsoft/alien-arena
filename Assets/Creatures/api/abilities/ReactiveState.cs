using Creatures.api.abilities;
using Creatures.api.abilities.states;

namespace Creatures.Api
{
    public abstract class ReactiveState : IAbilityState
    {
        protected AbilityEvent _abilityEvent;
        
        public abstract void Enter();
        public abstract void Exit();
        public abstract AbilityExecutionResult Execute(IPlayable playable, AbilityEvent abilityEvent);

        public ReactiveState(AbilityEvent abilityEvent)
        {
            _abilityEvent = abilityEvent;
        }

        // Call this method when you want to complete the state
        public void OnComplete()
        {
            _abilityEvent.SetState(new ReadyState());
            Exit();
        }
    }
}
