using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;

namespace Creatures.Api
{
    public interface IPlayable
    {
        // Basic Attack (aka LMB):
        // - Should be unaffected by Silence
        // - Only gated by GCD (aka no specific ability cooldown)
        void BasicAttack(BasicAttackEvent basicAttackEvent);
        
        // Charged Ability (aka RMB):
        // - Player starts charging the ability by clicking RMB
        // - Ability gains strength as the button is held down
        // - Ability is activated when the player releases RMB
        // - Effects like Stuns or Silences will interrupt charging
        // - Only gated by GCD (aka no specific ability cooldown)
        void StartCharging(StartChargingEvent startChargingEvent);
        void InterruptCharging(InterruptChargingEvent interruptChargingEvent);
        void ChargedAbility(ChargedAbilityEvent chargedAbilityEvent);
        
        // Character Ability (aka Q):
        // - Unrestricted design space for characters, this should be
        // and iconic ability 
        void CharacterAbility(CharacterAbilityEvent characterAbilityEvent);
        
        // Mobility Ability (aka LSHIFT):
        // - An ability that enhances a characters mobility
        // - For Defenders: this should be an additional Character Ability instead
        void MobilityAbility(MobilityAbilityEvent mobilityAbilityEvent);
        
        // Core Infusion Ability (aka E):
        // - This is SET when the player equips their first infusion
        void CoreInfusionAbility(CoreInfusionAbilityEvent coreInfusionAbilityEvent);
    }
}