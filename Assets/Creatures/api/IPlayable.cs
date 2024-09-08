using System.Collections.Generic;
using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;

namespace Creatures.Api
{
    public interface IPlayable : IStateNotifier
    {
                
        /// <summary>
        /// Retrieves all abilities associated with the current playable character.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <see cref="AbilityEvent"/> objects representing all abilities 
        /// that the playable character can use.
        /// </returns>
        /// <remarks>
        /// This method returns all abilities available to the character, including those that may be affected by global cooldowns.
        /// </remarks>
        IEnumerable<AbilityEvent> GetAbilities();
        
        /// <summary>
        /// Triggers the global cooldown for all abilities except the one that is currently being used.
        /// </summary>
        /// <param name="abilityEvent">The ability that is currently being used and should not be affected by the global cooldown.</param>
        /// <remarks>
        /// This method sets all abilities, except the one provided by <paramref name="abilityEvent"/>, into a global cooldown state 
        /// if they are subject to the global cooldown.
        /// </remarks>
        void triggerGlobalCooldown(AbilityEvent abilityEvent);
        
        // Basic Attack (aka LMB):
        // - Should be unaffected by Silence
        // - Only gated by GCD (aka no specific ability cooldown)
        AbilityExecutionResult BasicAttack(BasicAttackEvent basicAttackEvent);
        
        // Charged Ability (aka RMB):
        // - Player starts charging the ability by clicking RMB
        // - Ability gains strength as the button is held down
        // - Ability is activated when the player releases RMB
        // - Effects like Stuns or Silences will interrupt charging
        // - Only gated by GCD (aka no specific ability cooldown)
        AbilityExecutionResult StartCharging(StartChargingEvent startChargingEvent);
        AbilityExecutionResult InterruptCharging(InterruptChargingEvent interruptChargingEvent);
        AbilityExecutionResult ChargedAbility(ChargedAbilityEvent chargedAbilityEvent);
        
        // Character Ability (aka Q):
        // - Unrestricted design space for characters, this should be
        // and iconic ability 
        AbilityExecutionResult CharacterAbility(CharacterAbilityEvent characterAbilityEvent);
        
        // Mobility Ability (aka LSHIFT):
        // - An ability that enhances a characters mobility
        // - For Defenders: this should be an additional Character Ability instead
        AbilityExecutionResult MobilityAbility(MobilityAbilityEvent mobilityAbilityEvent);
        
        // Core Infusion Ability (aka E):
        // - This is SET when the player equips their first infusion
        AbilityExecutionResult CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent);
        ICoreInfusion CoreInfusion { get; set; }
    }
}
