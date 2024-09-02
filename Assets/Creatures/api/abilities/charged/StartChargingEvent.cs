using Creatures.Api;

namespace Creatures.api.abilities.charged
{
    /**
     * Begin charging up the charged ability.
     */
    public class StartChargingEvent : AbilityEvent
    {
        public override AbilityExecutionResult ExecuteAbility(IPlayable playable)
        {
            return playable.StartCharging(this);
        }
    }
}