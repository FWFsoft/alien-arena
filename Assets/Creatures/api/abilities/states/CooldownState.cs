using Creatures.Api;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Creatures.api.abilities.states
{
    public class CooldownState : ReactiveState, IAbilityState
    {
        protected readonly IStateNotifier cooldownStateNotifier;
        private float startTime;

        public CooldownState(IStateNotifier cooldownStateNotifier, AbilityEvent abilityEvent)
            : base(abilityEvent)
        {
            this.cooldownStateNotifier = cooldownStateNotifier;
        }
        
        public override void Enter()
        {
            _abilityEvent.Subscribe(cooldownStateNotifier, this, false);
        }

        public override void Exit()
        {
            _abilityEvent.Unsubscribe(cooldownStateNotifier, this);
        }

        public override AbilityExecutionResult Execute(IPlayable playable, AbilityEvent abilityEvent)
        {
            return AbilityExecutionResult.OnCooldown;
        }
    }
}
