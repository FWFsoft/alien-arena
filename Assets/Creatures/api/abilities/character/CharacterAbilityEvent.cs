using Creatures.Api;
using Creatures.api.abilities.states;

namespace Creatures.api.abilities.character
{
    /**
     * Character Abilities define the one button each character
     * has with no common use pattern, it has a cooldown and
     * varies greatly across characters
     */
    public class CharacterAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.CharacterAbility(this);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.CharacterAbility;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Subscribe(state, this.getId(), notifier.GetCharacterAbilityCooldown());
        }
        
        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Unsubscribe(state, this.getId());
        }
    }
}
