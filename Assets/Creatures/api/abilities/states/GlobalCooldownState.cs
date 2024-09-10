namespace Creatures.api.abilities.states
{
    public class GlobalCooldownState : CooldownState
    {
        public GlobalCooldownState(IStateNotifier cooldownStateNotifier, AbilityEvent abilityEvent) : base(cooldownStateNotifier, abilityEvent)
        {
        }
        public override void Enter()
        {
            _abilityEvent.Subscribe(cooldownStateNotifier, this, true);
        }
    }
}
