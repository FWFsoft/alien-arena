using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Begin charging up the charged ability.
     */
    public class StartChargingEvent : AbilityEvent
    {
        public override void Execute(IPlayable playable)
        {
            playable.StartCharging(this);
        }
    }
}