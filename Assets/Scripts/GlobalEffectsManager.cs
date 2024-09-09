using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting.FullSerializer;

using UnityEngine;

// Note: Singleton that represents all effects registered by enemies in the game.
// The reason this is static is to make it so we don't have to look things up
// at runtime by tag, and can instead just rely on the scripts themselves to talk to each other
// A DI container might be preferable if we wanted to unit test things
public static class GlobalEffectsManager
{
    private static List<EffectsManagerScript> registeredEffectsManagers = new List<EffectsManagerScript>();

    public static void AddEffectsManagerReference(EffectsManagerScript effectsManagerScript)
    {
        Debug.Log("adding " + effectsManagerScript);
        registeredEffectsManagers.Add(effectsManagerScript);
    }

    public static void RemoveEffectsManagerReference(EffectsManagerScript effectsManagerScript)
    {
        registeredEffectsManagers.Remove(effectsManagerScript);
    }

    // TODO: Too tightly coupled?
    public static void doubleStacksOfFrothy()
    {
        foreach (var effectsManager in registeredEffectsManagers)
        {
            Debug.Log("Doubling stacks");
            effectsManager.doubleEffectStackIfPresent(Frothy.EFFECT_NAME);
        }
    }
}
