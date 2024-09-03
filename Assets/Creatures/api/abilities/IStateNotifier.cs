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
        /**
         * Begin the cooldown for BasicAttack, and call CooldownState.OnComplete when
         * BasicAttack should enter the ReadyState (aka come off cooldown)
         */
        void Subscribe(CooldownState cooldownState, BasicAttackEvent basicAttackEvent);
        
        /**
         * Begin the cooldown for StartCharging, and call CooldownState.OnComplete when
         * StartCharging should enter the ReadyState (aka come off cooldown)
         */
        void Subscribe(CooldownState cooldownState, StartChargingEvent startChargingEvent);
        
        /**
         *  This should just immediately call CooldownState.OnComplete because being interrupted
         *  on your charge shouldn't have a cooldown.
         */
        void Subscribe(CooldownState cooldownState, InterruptChargingEvent interruptChargingEvent);
        
        /**
         *  This should just immediately call CooldownState.OnComplete because ending your charge
         *  shouldn't have a cooldown. We'll control the CD of the charged attack by having a
         *  cooldown on STARTING the charge.
         */
        void Subscribe(CooldownState cooldownState, ChargedAbilityEvent chargedAbilityEvent);
        
        /**
         * Begin the cooldown for CharacterAbility, and call CooldownState.OnComplete when
         * CharacterAbility should enter the ReadyState (aka come off cooldown)
         */
        void Subscribe(CooldownState cooldownState, CharacterAbilityEvent characterAbilityEvent);
        
        /**
         * Begin the cooldown for MobilityAbility, and call CooldownState.OnComplete when
         * MobilityAbility should enter the ReadyState (aka come off cooldown)
         */
        void Subscribe(CooldownState cooldownState, MobilityAbilityEvent mobilityAbilityEvent);
        
        /**
         * Begin the cooldown for MobilityAbility, and call CooldownState.OnComplete when
         * MobilityAbility should enter the ReadyState (aka come off cooldown)
         */
        void Subscribe(CooldownState cooldownState, CoreInfusionAbilityEvent coreInfusionAbilityEvent);
        
        /**
         * Signifies that the next BasicAttackEvent will not be blocked on the cooldown. Can
         * use this as an opportunity to reset a cooldown timer or clear out any temp state.
         */
        void Unsubscribe(CooldownState cooldownState, BasicAttackEvent basicAttackEvent);
        
        /**
         * Signifies that the next StartChargingEvent will not be blocked on the cooldown. Can
         * use this as an opportunity to reset a cooldown timer or clear out any temp state.
         */
        void Unsubscribe(CooldownState cooldownState, StartChargingEvent startChargingEvent);
        
        /**
         * This should just No-Op, unless later on we decide to have a cooldown on interrupt
         */
        void Unsubscribe(CooldownState cooldownState, InterruptChargingEvent interruptChargingEvent);
        
        /**
         * This should just No-Op, should never reject a ChargedAbilityEvent due to cooldown so we
         * shouldn't have anything from Subscribe to clean up
         */
        void Unsubscribe(CooldownState cooldownState, ChargedAbilityEvent chargedAbilityEvent);
        
        /**
         * Signifies that the next CharacterAbilityEvent will not be blocked on the cooldown. Can
         * use this as an opportunity to reset a cooldown timer or clear out any temp state.
         */
        void Unsubscribe(CooldownState cooldownState, CharacterAbilityEvent characterAbilityEvent);
        
        /**
         * Signifies that the next MobilityAbilityEvent will not be blocked on the cooldown. Can
         * use this as an opportunity to reset a cooldown timer or clear out any temp state.
         */
        void Unsubscribe(CooldownState cooldownState, MobilityAbilityEvent mobilityAbilityEvent);
        
        /**
         * Signifies that the next CoreInfusionAbilityEvent will not be blocked on the cooldown. Can
         * use this as an opportunity to reset a cooldown timer or clear out any temp state.
         */
        void Unsubscribe(CooldownState cooldownState, CoreInfusionAbilityEvent coreInfusionAbilityEvent);
        
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
