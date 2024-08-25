using UnityEngine;
using UnityEditor;
using System;
using System.IO;

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
        string lockFilePath = Path.Combine(unitDir, $"{unitName}.lock");

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

        // Skip linking if the lockfile indicates that linking is complete
        if (File.Exists(lockFilePath))
        {
            var lockFileContent = File.ReadAllText(lockFilePath);
            if (lockFileContent.Contains("Linked: Complete"))
            {
                Debug.Log($"Skipping linking for {unitName} because it has already been linked.");
                return;
            }
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
        var scriptComponent = unitPrefab.AddComponent(scriptType);

        // Set the Animator field
        var animatorField = scriptType.GetProperty("Animator");
        if (animatorField != null && animatorField.CanWrite)
        {
            Animator animator = unitPrefab.GetComponent<Animator>();
            animatorField.SetValue(scriptComponent, animator);
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
                    
            var healthAnimatorField = healthScript.GetType().GetField("enemyAnimator");
            if (healthAnimatorField != null)
            {
                healthAnimatorField.SetValue(healthScript, unitPrefab.GetComponent<Animator>());
            }

            var healthBarField = healthScript.GetType().GetField("healthBar");
            if (healthBarField != null)
            {
                // TODO: Make this not shitty and fragile
                string healthBarPath = "Assets/AlienScifiCharacter/InnerHealthBar.prefab";
                GameObject healthBarPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(healthBarPath);
                healthBarField.SetValue(healthScript, healthBarPrefab.GetComponent<GameObject>());
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
                // TODO: Make this not shitty and fragile
                var dotControllerPath = "Assets/AlienScifiCharacter/DotController.prefab";
                GameObject dotControllerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(dotControllerPath);
                effectsAnimatorField.SetValue(effectsManagerScript, dotControllerPrefab.GetComponent<Animator>());
            }

            // Optionally call the SetupInstance method
            effectsManagerScript.SetupInstance();
        }

        // Save the updated prefab
        PrefabUtility.SaveAsPrefabAsset(unitPrefab, prefabPath);

        // Update the lockfile to indicate that linking is complete
        File.WriteAllText(lockFilePath, "Hydration: Complete\nLinked: Complete");

        Debug.Log($"{unitName} script linked successfully.");
    }
}
