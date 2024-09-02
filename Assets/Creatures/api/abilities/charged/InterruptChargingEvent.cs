using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Interrupts charging, stopping a character from
     * charging their attack without using it.
     *
     * NOTE: This won't necessarily occur when a player
     * presses a button and is likely an event fired
     * when a player enters a status effect where they
     * would not be allowed to charge their charged attack
     */
    public class InterruptChargingEvent : AbilityEvent
    {
        public override void Execute(IPlayable playable)
        {
            playable.InterruptCharging(this);
        }
    }
}