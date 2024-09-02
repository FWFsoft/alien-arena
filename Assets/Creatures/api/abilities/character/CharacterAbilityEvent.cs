using Creatures.Api;

namespace Creatures.api.abilities.character
{
    /**
     * Character Abilities define the one button each character
     * has with no common use pattern, it has a cooldown and
     * varies greatly across characters
     */
    public class CharacterAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult Execute(IPlayable playable)
        {
            return playable.CharacterAbility(this);
        }
    }
}