namespace Creatures.api.abilities
{
    /**
     * Returned when consumers call AbilityEvent.Execute in order to communicate
     * back if the execution was successful or if it wasn't executed and why
     */
    public enum AbilityExecutionResult
    {
        Success,
        OnCooldown,
        Disabled // Used when the character is Stunned, Silenced, or has not learned the ability yet
    }
}
