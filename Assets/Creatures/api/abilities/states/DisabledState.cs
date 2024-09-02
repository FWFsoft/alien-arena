using Creatures.Api;

namespace Creatures.api.abilities.states
{
    public class DisabledState : IAbilityState
    {
        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public AbilityExecutionResult Execute(IPlayable playable, AbilityEvent abilityEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}