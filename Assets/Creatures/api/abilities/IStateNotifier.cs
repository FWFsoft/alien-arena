using Creatures.api.abilities.states;

namespace Creatures.api.abilities
{
    public interface IStateNotifier
    {
        /// <summary>
        /// Gets the final cooldown duration for the Basic Attack ability, considering all factors such as buffs, stats, procs, 
        /// infusions, and whether the ability's affected by global cooldowns.
        /// </summary>
        /// <param name="isTriggeredByGlobalCooldown">Indicates whether the ability's cooldown is being affected by a global cooldown.</param>
        /// <returns>
        /// The final cooldown value for the Basic Attack ability, which includes any reductions or modifications 
        /// due to buffs, stats, or other effects. If <paramref name="isTriggeredByGlobalCooldown"/> is true, the method 
        /// also considers the global cooldown.
        /// </returns>
        float GetBasicAttackCooldown(bool isTriggeredByGlobalCooldown);

        /// <summary>
        /// Gets the final cooldown duration for the Start Charging ability, considering all factors such as buffs, stats, procs, 
        /// infusions, and whether the ability's affected by global cooldowns.
        /// </summary>
        /// <param name="isTriggeredByGlobalCooldown">Indicates whether the ability's cooldown is being affected by a global cooldown.</param>
        /// <returns>
        /// The final cooldown value for the Start Charging ability, which includes any reductions or modifications 
        /// due to external factors. If <paramref name="isTriggeredByGlobalCooldown"/> is true, the method also considers the global cooldown.
        /// </returns>
        float GetStartChargingCooldown(bool isTriggeredByGlobalCooldown);

        /// <summary>
        /// Gets the final cooldown duration for the Character Ability, considering all factors such as buffs, stats, procs, 
        /// infusions, and whether the ability's affected by global cooldowns.
        /// </summary>
        /// <param name="isTriggeredByGlobalCooldown">Indicates whether the ability's cooldown is being affected by a global cooldown.</param>
        /// <returns>
        /// The final cooldown value for the Character Ability, which includes any reductions or modifications 
        /// due to external factors. If <paramref name="isTriggeredByGlobalCooldown"/> is true, the method also considers the global cooldown.
        /// </returns>
        float GetCharacterAbilityCooldown(bool isTriggeredByGlobalCooldown);

        /// <summary>
        /// Gets the final cooldown duration for the Mobility Ability, considering all factors such as buffs, stats, procs, 
        /// infusions, and whether the ability's affected by global cooldowns.
        /// </summary>
        /// <param name="isTriggeredByGlobalCooldown">Indicates whether the ability's cooldown is being affected by a global cooldown.</param>
        /// <returns>
        /// The final cooldown value for the Mobility Ability, which includes any reductions or modifications 
        /// due to external factors. If <paramref name="isTriggeredByGlobalCooldown"/> is true, the method also considers the global cooldown.
        /// </returns>
        float GetMobilityAbilityCooldown(bool isTriggeredByGlobalCooldown);

        /// <summary>
        /// Gets the final cooldown duration for the Core Infusion ability, considering all factors such as buffs, stats, procs, 
        /// infusions, and whether the ability's affected by global cooldowns.
        /// </summary>
        /// <param name="isTriggeredByGlobalCooldown">Indicates whether the ability's cooldown is being affected by a global cooldown.</param>
        /// <returns>
        /// The final cooldown value for the Core Infusion ability, which includes any reductions or modifications 
        /// due to external factors. If <paramref name="isTriggeredByGlobalCooldown"/> is true, the method also considers the global cooldown.
        /// </returns>
        float GetCoreInfusionAbilityCooldown(bool isTriggeredByGlobalCooldown);


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
