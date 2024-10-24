using Creatures.Api;

using UnityEngine;

namespace Creatures.api.abilities
{
    /**
     * Tracks the state of an ability, and understands if it's executable or not,
     * also defines what occurs after it's been executed.
     */
    public interface IAbilityState
    {
        void Enter();
        void Exit();
        AbilityExecutionResult Execute(IPlayable playable, AbilityEvent abilityEvent, Vector3 mousePosition);
    }
}
