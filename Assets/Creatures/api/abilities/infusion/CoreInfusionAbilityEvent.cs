using Creatures.Api;

namespace Creatures.api.abilities.infusion
{
    /**
     * The first infusion a player equips will grant them
     * an ability based on the genera associated with the
     * infusion.
     */
    public class CoreInfusionAbilityEvent : AbilityEvent
    {
        public override void Execute(IPlayable playable)
        {
            playable.CoreInfusionAbility(this);
        }
    }
}