using Creatures.Api;

namespace Creatures.api.abilities.mobility
{
    /**
     * An ability that improves a character's mobility.
     *
     * NOTE: For Defenders, this should be an additional
     * character ability.
     */
    public class MobilityAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult Execute(IPlayable playable)
        {
            return playable.MobilityAbility(this);
        }
    }
}