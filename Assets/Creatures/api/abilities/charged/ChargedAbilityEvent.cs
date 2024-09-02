using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Actually fire the charged ability.
     */
    public class ChargedAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.ChargedAbility(this);
        }
    }
}