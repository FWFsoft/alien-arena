using System;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;

public class CreatureLinker : Editor
{
    [MenuItem("Tools/Link Creature Scripts")]
    public static void LinkCreatureScripts()
    {
        string rootGeneraPath = "Assets/Creatures/impl";
        string[] generaDirectories = Directory.GetDirectories(rootGeneraPath);

        foreach (var generaDir in generaDirectories)
        {
            string[] unitDirectories = Directory.GetDirectories(generaDir);

            foreach (var unitDir in unitDirectories)
            {
                LinkUnit(unitDir);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void LinkUnit(string unitDir)
    {
        string unitName = Path.GetFileName(unitDir);
        string scriptPath = Path.Combine(unitDir, $"{unitName}.cs");
        string prefabPath = Path.Combine(unitDir, $"{unitName}.prefab");

        // Check if the script and prefab exist
        if (!File.Exists(scriptPath))
        {
            Debug.LogError($"Script for {unitName} not found at {scriptPath}");
            return;
        }
        if (!File.Exists(prefabPath))
        {
            Debug.LogError($"Prefab for {unitName} not found at {prefabPath}");
            return;
        }

        // Load the prefab
        GameObject unitPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (unitPrefab == null)
        {
            Debug.LogError($"Failed to load prefab for {unitName} at {prefabPath}");
            return;
        }

        // Load the script
        MonoScript newScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
        Type scriptType = newScript?.GetClass();
        if (scriptType == null)
        {
            Debug.LogError($"Failed to load script type for {unitName}");
            return;
        }

        // Add the script component to the prefab
        var scriptComponent = unitPrefab.GetComponent(scriptType);
        if (scriptComponent == null)
        {
            scriptComponent = unitPrefab.AddComponent(scriptType);
            Debug.Log($"CreatureScript added to {unitName}.");
        }

        // Set the Animator field and link the controller
        var animatorField = scriptType.GetProperty("Animator");
        Animator animator = unitPrefab.GetComponent<Animator>();

        if (animatorField != null && animatorField.CanWrite)
        {
            if (animator != null)
            {
                animatorField.SetValue(scriptComponent, animator);
                var setAnimator = animatorField.GetValue(scriptComponent) as Animator;
                if (setAnimator != null)
                {
                    Debug.Log($"Animator is successfully set for {unitName}. Animator name: {setAnimator.name}");
                }
                else
                {
                    Debug.LogWarning($"Animator was not set correctly for {unitName}.");
                }
            }
            else
            {
                Debug.LogWarning($"Animator component not found on {unitName} prefab.");
            }
        }

        // Set other properties (Speed, Health, etc.)
        var speedField = scriptType.GetProperty("Speed");
        if (speedField != null && speedField.CanWrite)
        {
            speedField.SetValue(scriptComponent, speedField.GetValue(scriptComponent));
        }

        var healthField = scriptType.GetProperty("Health");
        if (healthField != null && healthField.CanWrite)
        {
            healthField.SetValue(scriptComponent, healthField.GetValue(scriptComponent));
        }

        // Add HealthScript if not already added
        var healthScript = unitPrefab.GetComponent<HealthScript>();
        if (healthScript == null)
        {
            healthScript = unitPrefab.AddComponent<HealthScript>();

            // Set the HealthScript field on the scriptComponent
            var healthScriptFieldScriptComponent = scriptType.GetProperty("HealthScript");
            if (healthScriptFieldScriptComponent != null && healthScriptFieldScriptComponent.CanWrite)
            {
                healthScriptFieldScriptComponent.SetValue(scriptComponent, healthScript);
            }

            var healthAnimatorField = healthScript.GetType().GetField("enemyAnimator");
            if (healthAnimatorField != null)
            {
                healthAnimatorField.SetValue(healthScript, unitPrefab.GetComponent<Animator>());
            }

            var healthBarField = healthScript.GetType().GetField("healthBar");
            if (healthBarField != null)
            {

                string healthBarPath = "Assets/AlienScifiCharacter/InnerHealthBar.prefab";
                GameObject healthBarPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(healthBarPath);

                healthBarField.SetValue(healthScript, healthBarPrefab);
            }
        }

        // Set the health value on the HealthScript
        if (healthField != null && healthField.CanWrite)
        {
            float healthValue = (float)healthField.GetValue(scriptComponent);
            healthScript.health = healthValue;
        }

        // Add EffectsManagerScript if not already added
        var effectsManagerScript = unitPrefab.GetComponent<EffectsManagerScript>();
        if (effectsManagerScript == null)
        {
            effectsManagerScript = unitPrefab.AddComponent<EffectsManagerScript>();
        }

        // Link HealthScript and Animator to EffectsManagerScript
        if (effectsManagerScript != null)
        {
            var healthScriptField = effectsManagerScript.GetType().GetField("healthScript");
            if (healthScriptField != null)
            {
                healthScriptField.SetValue(effectsManagerScript, healthScript);
            }

            var effectsAnimatorField = effectsManagerScript.GetType().GetField("effectsAnimator");
            if (effectsAnimatorField != null)
            {

                var dotControllerPath = "Assets/AlienScifiCharacter/DotController.prefab";
                GameObject dotControllerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(dotControllerPath);
                effectsAnimatorField.SetValue(effectsManagerScript, dotControllerPrefab.GetComponent<Animator>());
            }

            // Optionally call the SetupInstance method
            effectsManagerScript.SetupInstance();
        }

        // Calculate and set the death animation length
        AnimationClip deathClip = animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name.Contains("Die"));
        if (deathClip != null)
        {
            var deathAnimationLengthField = healthScript.GetType().GetField("deathAnimationLength");
            if (deathAnimationLengthField != null)
            {
                deathAnimationLengthField.SetValue(healthScript, deathClip.length);
                Debug.Log($"Set deathAnimationLength to {deathClip.length} for {unitName}.");
            }
        }
        else
        {
            Debug.LogWarning($"Death animation clip not found for {unitName}. Ensure the animation is correctly named and assigned.");
        }

        // Save the updated prefab
        PrefabUtility.SaveAsPrefabAsset(unitPrefab, prefabPath);

        Debug.Log($"{unitName} script linked successfully.");
    }
}
