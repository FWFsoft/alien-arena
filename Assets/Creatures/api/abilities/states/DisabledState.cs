using UnityEngine;

using Creatures.Api;

namespace Creatures.api.abilities.states
{
    public class DisabledState : ReactiveState, IAbilityState
    {
        private readonly IStateNotifier disabledStateNotifier;

        public DisabledState(IStateNotifier stateNotifier, AbilityEvent abilityEvent)
            : base(abilityEvent)
        {
            this.disabledStateNotifier = stateNotifier;
        }

        public override void Enter()
        {
            disabledStateNotifier.Subscribe(this);
        }

        public override void Exit()
        {
            disabledStateNotifier.Unsubscribe(this);
        }

        public override AbilityExecutionResult Execute(IPlayable playable, AbilityEvent abilityEvent, Vector2 mousePosition)
        {
            return AbilityExecutionResult.Disabled;
        }
    }
}
