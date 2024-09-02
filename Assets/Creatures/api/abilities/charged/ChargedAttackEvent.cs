using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Actually fire the charged attack.
     */
    public class ChargedAttackEvent : AbilityEvent
    {
        public override void Execute(IPlayable playable)
        {
            playable.ChargedAttack(this);
        }
    }
}