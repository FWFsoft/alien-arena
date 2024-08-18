using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CreatureHydrator : Editor
{
    [MenuItem("Tools/Hydrate Creatures")]
    public static void HydrateCreatures()
    {
        string rootGeneraPath = "Assets/Creatures/impl";
        string[] generaDirectories = Directory.GetDirectories(rootGeneraPath);

        foreach (var generaDir in generaDirectories)
        {
            string generaName = Path.GetFileName(generaDir);
            string abstractClassPath = Path.Combine(generaDir, $"{generaName}.cs");

            if (!File.Exists(abstractClassPath))
            {
                Debug.LogError($"Abstract class for {generaName} not found at {abstractClassPath}");
                continue;
            }

            string[] unitDirectories = Directory.GetDirectories(generaDir);

            foreach (var unitDir in unitDirectories)
            {
                string unitName = Path.GetFileName(unitDir);
                string unitYamlPath = Path.Combine(unitDir, $"{unitName}.yml");
                string lockFilePath = Path.Combine(unitDir, $"{unitName}.lock");

                // Skip hydration if the lockfile exists
                if (File.Exists(lockFilePath))
                {
                    Debug.Log($"Skipping {unitName} because it has already been hydrated (lockfile exists).");
                    continue;
                }

                if (!File.Exists(unitYamlPath))
                {
                    Debug.LogError($"YAML file for unit {unitName} not found at {unitYamlPath}");
                    continue;
                }

                string yamlContent = File.ReadAllText(unitYamlPath);
                UnitDefinition unit = DeserializeYaml<UnitDefinition>(yamlContent);

                // Hydrate the creature using the spec and directory
                HydrateCreature(unit, generaName, unitName, unitDir);

                // Create a lockfile to indicate that this creature has been hydrated
                File.WriteAllText(lockFilePath, "Hydrated on: " + DateTime.Now.ToString());
            }
        }
    }

    private static T DeserializeYaml<T>(string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<T>(yamlContent);
    }

    private static void HydrateCreature(UnitDefinition unit, string generaName, string unitName, string unitDir)
    {
        // Create the Prefab
        GameObject unitPrefab = new GameObject(unitName);
        SpriteRenderer spriteRenderer = unitPrefab.AddComponent<SpriteRenderer>();
        var animator = unitPrefab.AddComponent<Animator>();

        // Create an Animator Controller within the unit's directory
        string animatorPath = Path.Combine(unitDir, $"{unitName}Controller.controller");
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animatorPath);

        Sprite idleSprite = null;

        // Process each animation
        foreach (var animation in unit.animations)
        {
            string animationFolderPath = Path.Combine(unitDir, animation.name);
            string animationYamlPath = Path.Combine(animationFolderPath, $"{animation.name}.yml");
            string animationYamlContent = File.ReadAllText(animationYamlPath);
            AnimationSpec animationSpec = DeserializeYaml<AnimationSpec>(animationYamlContent);

            string spriteSheetPath = Path.Combine(animationFolderPath, $"Art/{unitName}_{animation.name}.png");
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spriteSheetPath);

            // Slice the sprite sheet
            Sprite[] sprites = SliceSpriteSheet(texture, animationSpec);

            if (animation.name.Equals("Idle", StringComparison.OrdinalIgnoreCase) && sprites.Length > 0)
            {
                idleSprite = sprites[0];
            }

            // Create and add AnimationClip to AnimatorController
            AnimationClip clip = CreateAnimationClip(sprites, animationSpec.frames, animation.name, animationSpec.frameRate, animationFolderPath);
            animatorController.AddMotion(clip);
        }

        // Assign the Animator Controller
        animator.runtimeAnimatorController = animatorController;

        // Set the sprite renderer to use the idle sprite by default
        if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }

        // Generate a new script extending the appropriate abstract class and place it in the unit's directory
        string scriptPath = Path.Combine(unitDir, $"{unitName}.cs");
        File.WriteAllText(scriptPath, GenerateScriptContent(unitName, generaName, unit.speed, unit.health));
        AssetDatabase.ImportAsset(scriptPath);
        var newScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
        unitPrefab.AddComponent(newScript.GetClass());

        // Save the prefab in the unit's directory
        string prefabPath = Path.Combine(unitDir, $"{unitName}.prefab");
        PrefabUtility.SaveAsPrefabAsset(unitPrefab, prefabPath);
        GameObject.DestroyImmediate(unitPrefab);
    }

    private static Sprite[] SliceSpriteSheet(Texture2D texture, AnimationSpec animationSpec)
    {
        int spriteWidth = animationSpec.canvas.width;
        int spriteHeight = animationSpec.canvas.height;
        int frames = animationSpec.frames;

        int columns = texture.width / spriteWidth;
        int rows = texture.height / spriteHeight;

        if (columns * rows < frames)
        {
            Debug.LogError("The sprite sheet does not contain enough frames for the specified animation.");
            return null;
        }

        List<Sprite> sprites = new List<Sprite>();

        SpriteMetaData[] metaData = new SpriteMetaData[frames];

        for (int i = 0; i < frames; i++)
        {
            int x = (i % columns) * spriteWidth;
            int y = texture.height - spriteHeight - (i / columns) * spriteHeight;

            metaData[i] = new SpriteMetaData
            {
                alignment = (int)SpriteAlignment.Center,
                border = new Vector4(0, 0, 0, 0),
                name = $"{texture.name}_{i}",
                pivot = new Vector2(0.5f, 0.5f),
                rect = new Rect(x, y, spriteWidth, spriteHeight)
            };
        }

        string assetPath = AssetDatabase.GetAssetPath(texture);
        TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

        if (textureImporter != null)
        {
            textureImporter.isReadable = true;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            textureImporter.spritesheet = metaData;

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            sprites.AddRange(AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>());
        }
        else
        {
            Debug.LogError("Failed to load TextureImporter.");
        }

        return sprites.ToArray();
    }

    private static AnimationClip CreateAnimationClip(Sprite[] sprites, int frames, string animationName, int frameRate, string animationFolderPath)
    {
        AnimationClip clip = new AnimationClip();
        EditorCurveBinding curveBinding = new EditorCurveBinding
        {
            path = "",
            propertyName = "m_Sprite",
            type = typeof(SpriteRenderer)
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[frames];
        float frameTime = 1f / frameRate;

        for (int i = 0; i < frames; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * frameTime,
                value = sprites[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);
        clip.name = animationName;
        string animFilePath = Path.Combine(animationFolderPath, $"{animationName}.anim");
        AssetDatabase.CreateAsset(clip, animFilePath);
        
        return clip;
    }           

    private static string GenerateScriptContent(string unitName, string generaName, float speed, int health)
    {
        return $@"
            using UnityEngine;
            using Creatures.Api;
            
            namespace Creatures.impl
            {{
                public class {unitName}Script : Creature, {generaName}
                {{
                    public string GeneraType => ""{generaName}"";
                    
                    void Start()
                    {{
                        Speed = {speed}f;
                        HealthScript = new HealthScript({health});
                        
                        // Add custom initialization logic here
                    }}

                    // Optional: Override the Update method if custom logic is needed
                    protected override void Update()
                    {{
                        base.Update();

                        // Add custom update logic here, if necessary
                    }}
                }}
            }}";
    }

    [System.Serializable]
    public class UnitDefinition
    {
        public string unitName;
        public float speed;
        public int health;
        public List<AnimationDefinition> animations;
    }

    [System.Serializable]
    public class AnimationDefinition
    {
        public string name;
        public string path;
    }

    [System.Serializable]
    public class AnimationSpec
    {
        public CanvasSize canvas;
        public int frames;
        public int frameRate;
    }
    [System.Serializable]
    public class CanvasSize
    {
        public int height;
        public int width;
    }
}
