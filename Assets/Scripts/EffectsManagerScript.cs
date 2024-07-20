
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;

class EffectInstance
{
    public int ticks;
    private int stacks;
    private IEffect effect;

    public EffectInstance(IEffect effect)
    {
        // Manage stacks and ticks outside constructor
        ticks = 0;
        stacks = 0;
        this.effect = effect;
    }
    public bool IsEffectFinished()
    {
        return ticks <= 0;
    }

    public void tickDown()
    {
        ticks--;
    }

    public void incrementStackAndTicks()
    {
        if(effect.getMaxStacks() > stacks)
        {
            stacks++;
        }
        // If we're at max stacks, still refresh the ticks
        ticks = effect.getMaxTicks();
        
    }

    public int getStacks()
    {
        return stacks;
    }

    public IEffect getEffect()
    {
        return effect;
    }

    public void doubleStacks()
    {
        stacks = Mathf.Min(2 * stacks, effect.getMaxStacks());
        Debug.Log("stacks " + stacks);
        ticks = effect.getMaxTicks();
    }
}
public class EffectsManagerScript : MonoBehaviour
{
    private ConcurrentDictionary<string, EffectInstance> effectInstances;
    [SerializeField]
    public HealthScript healthScript;
    [SerializeField]
    public Animator effectsAnimator;

    public void SetupInstance()
    {
        effectInstances = new ConcurrentDictionary<string, EffectInstance>();
        GlobalEffectsManager.AddEffectsManagerReference(this);
    }
    private void OnDestroy()
    {
        GlobalEffectsManager.RemoveEffectsManagerReference(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: reusable for other characters
        if(collision.gameObject.TryGetComponent<EffectHolderScript>(out var effectHolder))
        {
            foreach(var effectType in effectHolder.effectTypes) {
                // Join like effects
                var effect = EffectFactory.createEffect(effectType);
                var effectName = effect.getEffectName();
                if (!effectInstances.ContainsKey(effectName))
                {
                    effectInstances.TryAdd(effectName, new EffectInstance(effect));
                }
                var effectInstance = effectInstances.GetValueOrDefault(effectName);
                effectInstance.incrementStackAndTicks();
                effect.addEffect(this, effectInstance.ticks);
            }
            StopCoroutine("ApplyEffects");
            StartCoroutine("ApplyEffects");
        }
    }

    public void doubleEffectStackIfPresent(string effectName)
    {
        if(effectInstances.ContainsKey(effectName))
        {
            effectInstances.TryGetValue(effectName, out var effectInstance);
            effectInstance.doubleStacks();
        }
    }

    IEnumerator ApplyEffects()
    {
        while (effectInstances.Count > 0)
        {
            // Process effects
            foreach (var entry in effectInstances)
            {
                var effectInstance = entry.Value;
                effectInstance.getEffect().applyEffect(this, effectInstance.getStacks());
                effectInstance.tickDown();
                print(effectInstance.ticks);
                if (effectInstance.IsEffectFinished())
                {
                    effectInstance.getEffect().removeEffect(this);
                    effectInstances.Remove(entry.Key, out _);
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
