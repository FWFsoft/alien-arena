

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// All effects have to implement this interface
public interface IEffect
{
    void addEffect(EffectsManagerScript effectsManagerScript, int stacks);
    void applyEffect(EffectsManagerScript effectsManagerScript, int stacks);
    void removeEffect(EffectsManagerScript effectsManagerScript);
    int getMaxTicks();
    int getMaxStacks();
    string getEffectName();
}

// TODO: Is this the source of truth for effects? Amplifier?
public class Frothy : IEffect
{
    public const string EFFECT_NAME = "Frothy";

    public int getMaxStacks()
    {
        return 5;
    }

    public int getMaxTicks()
    {
        return 10;
    }

    public void addEffect(EffectsManagerScript effectsManagerScript, int stacks)
    {
        effectsManagerScript.effectsAnimator.SetBool("isFrothy", true);
        effectsManagerScript.effectsAnimator.speed = stacks / 7.5f; // Some factor to not make it super fast
    }
    public void applyEffect(EffectsManagerScript effectsManagerScript, int stacks)
    {
        var damage = 5 * stacks;
        var healthScript = effectsManagerScript.healthScript;
        healthScript.UpdateHealth(-damage);
    }

    public void removeEffect(EffectsManagerScript effectsManagerScript)
    {
        effectsManagerScript.effectsAnimator.SetBool("isFrothy", false);
    }

    public string getEffectName()
    {
        return EFFECT_NAME;
    }
}

// some clunky DI
public static class EffectFactory
{
    static List<IEffect> effects = new List<IEffect>() { new Frothy() };
    public static IEffect createEffect(string effectName)
    {
        var effect = effects.First(effect => effectName.Equals(effect.getEffectName(), System.StringComparison.InvariantCultureIgnoreCase));
        if(effect == null)
        {
            throw new System.Exception("Effect Name Invalid");
        }
        return effect;
    }
}
