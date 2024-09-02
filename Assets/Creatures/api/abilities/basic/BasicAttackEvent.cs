using Creatures.Api;

namespace Creatures.api.abilities.basic
{
    public class BasicAttackEvent : AbilityEvent
    {
        public override void Execute(IPlayable playable)
        {
            playable.BasicAttack(this);
        }
    }
}