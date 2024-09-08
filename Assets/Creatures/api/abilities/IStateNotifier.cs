using Creatures.Api;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;
using Creatures.api.abilities.states;

namespace Creatures.api.abilities
{
    public interface IStateNotifier
    {
     
        float GetBasicAttackCooldown();
        float GetStartChargingCooldown();
        float GetCharacterAbilityCooldown();
        float GetMobilityAbilityCooldown();
        float GetCoreInfusionAbilityCooldown();

        /// <summary>
        /// Starts the cooldown for the specified ability with the given duration.
        /// </summary>
        /// <param name="cooldownState">The state object that represents the cooldown state of the ability.</param>
        /// <param name="abilityIdentifier">The unique identifier of the ability whose cooldown is being started.</param>
        /// <param name="cooldownDuration">The duration of the cooldown in seconds.</param>
        /// <remarks>
        /// The cooldown timer runs asynchronously, and once the specified <paramref name="cooldownDuration"/> elapses, 
        /// the method triggers <see cref="CooldownState.OnComplete"/> to indicate that the cooldown has finished.
        /// </remarks>
        void Subscribe(CooldownState cooldownState, AbilityIdentifier abilityIdentifier, float cooldownDuration);
        
        /// <summary>
        /// Cancels the active cooldown for the specified ability and unsubscribes from the cooldown state.
        /// </summary>
        /// <param name="cooldownState">The state object representing the cooldown state of the ability.</param>
        /// <param name="abilityIdentifier">The unique identifier of the ability whose cooldown is being canceled.</param>
        /// <remarks>
        /// This method stops the ongoing cooldown for the ability identified by <paramref name="abilityIdentifier"/> 
        /// and unsubscribes the ability from the current <paramref name="cooldownState"/>. If the ability is not 
        /// currently on cooldown, the method performs no action.
        /// </remarks>
        void Unsubscribe(CooldownState cooldownState, AbilityIdentifier abilityIdentifier);
        
        /**
         * Disabled functions more like a typical pub/sub, because disabling abilities will likely
         * span multiple abilities and begin and end all of those disabled states at the same time.
         *
         * So this Subscribe should add this state to the list of States that we'll call OnComplete
         * on when the Disabled effect ends.
         */
        void Subscribe(DisabledState disabledState);
        
        /**
         * See above Subscribe comment for more detail, this is the clean-up action that occurs once
         * all the abilities have exited the disabled state.
         */
        void Unsubscribe(DisabledState disabledState);
    }
}
