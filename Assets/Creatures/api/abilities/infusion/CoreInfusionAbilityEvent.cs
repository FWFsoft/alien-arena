using Creatures.api.abilities.states;
using Creatures.Api;

using UnityEngine;

namespace Creatures.api.abilities.infusion
{
    /**
     * The first infusion a player equips will grant them
     * an ability based on the genera associated with the
     * infusion.
     */
    public class CoreInfusionAbilityEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable, Vector3 mousePosition)
        {
            return playable.CoreInfusionAbility(this, mousePosition);
        }

        public override AbilityIdentifier getId()
        {
            return AbilityIdentifier.CoreInfusionAbility;
        }

        public override void Subscribe(IStateNotifier notifier, CooldownState state, bool isTriggeredByGlobalCooldown)
        {
            notifier.Subscribe(
                state,
                getId(),
                notifier.GetCoreInfusionAbilityCooldown(isTriggeredByGlobalCooldown)
                );
        }

        public override void Unsubscribe(IStateNotifier notifier, CooldownState state)
        {
            notifier.Unsubscribe(state, this.getId());
        }
    }
}
