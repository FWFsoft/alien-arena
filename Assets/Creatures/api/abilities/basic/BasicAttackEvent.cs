using Creatures.Api;

namespace Creatures.api.abilities.basic
{
    public class BasicAttackEvent : AbilityEvent
    {
        public override AbilityExecutionResult Execute(IPlayable playable)
        {
            return playable.BasicAttack(this);
        }
    }
}