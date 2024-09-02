using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Actually fire the charged ability.
     */
    public class ChargedAbilityEvent : AbilityEvent
    {
        public override void Execute(IPlayable playable)
        {
            playable.ChargedAbility(this);
        }
    }
}