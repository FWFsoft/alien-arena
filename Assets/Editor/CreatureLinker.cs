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
                string unitName = Path.GetFileName(unitDir);
                string scriptPath = Path.Combine(unitDir, $"{unitName}.cs");
                string prefabPath = Path.Combine(unitDir, $"{unitName}.prefab");
                string lockFilePath = Path.Combine(unitDir, $"{unitName}.lock");

                // Check if the script and prefab exist
                if (!File.Exists(scriptPath))
                {
                    Debug.LogError($"Script for {unitName} not found at {scriptPath}");
                    continue;
                }
                if (!File.Exists(prefabPath))
                {
                    Debug.LogError($"Prefab for {unitName} not found at {prefabPath}");
                    continue;
                }

                // Skip linking if the lockfile indicates that linking is complete
                if (File.Exists(lockFilePath))
                {
                    var lockFileContent = File.ReadAllText(lockFilePath);
                    if (lockFileContent.Contains("Linked: Complete"))
                    {
                        Debug.Log($"Skipping linking for {unitName} because it has already been linked.");
                        continue;
                    }
                }

                // Load the prefab
                GameObject unitPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (unitPrefab == null)
                {
                    Debug.LogError($"Failed to load prefab for {unitName} at {prefabPath}");
                    continue;
                }

                // Load the script
                MonoScript newScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
                Type scriptType = newScript?.GetClass();
                if (scriptType == null)
                {
                    Debug.LogError($"Failed to load script type for {unitName}");
                    continue;
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

                // Save the updated prefab
                PrefabUtility.SaveAsPrefabAsset(unitPrefab, prefabPath);

                // Update the lockfile to indicate that linking is complete
                File.WriteAllText(lockFilePath, "Hydration: Complete\nLinked: Complete");

                Debug.Log($"{unitName} script linked successfully.");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
