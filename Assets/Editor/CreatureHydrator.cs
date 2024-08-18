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

                // Skip hydration if the lockfile exists and hydration is complete
                if (File.Exists(lockFilePath))
                {
                    var lockFileContent = File.ReadAllText(lockFilePath);
                    if (lockFileContent.Contains("Hydration: Complete"))
                    {
                        Debug.Log($"Skipping hydration for {unitName} because it has already been hydrated.");
                        continue;
                    }
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

                // Create or update a lockfile to indicate that this creature has been hydrated
                File.WriteAllText(lockFilePath, "Hydration: Complete\nLinked: Incomplete");
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

        // Add parameters for controlling the animations
        animatorController.AddParameter("isMoving", AnimatorControllerParameterType.Bool);
        animatorController.AddParameter("isDead", AnimatorControllerParameterType.Bool);

        // Initialize the Idle sprite
        Sprite idleSprite = null;
        
        // Initialize the Idle canvas size
        CanvasSize idleCanvasSize = null;

        // Create states for the animations
        AnimatorState idleState = null;
        AnimatorState runState = null;
        AnimatorState dieState = null;

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

            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogError($"No sprites found for animation {animation.name} in unit {unitName}");
                continue;
            }

            // Create and add AnimationClip to AnimatorController
            AnimationClip clip = CreateAnimationClip(sprites, animationSpec.frames, animation.name, animationSpec.frameRate, animationFolderPath);
            AnimatorState state = animatorController.layers[0].stateMachine.AddState(animation.name);

            if (animation.name.Equals("Idle", StringComparison.OrdinalIgnoreCase))
            {
                idleState = state;
                idleSprite = sprites[0];  // Set the idle sprite for when/if no animations are playing
                idleCanvasSize = animationSpec.canvas; // Capture the canvas size from Idle.yml
            }
            else if (animation.name.Equals("Run", StringComparison.OrdinalIgnoreCase))
            {
                runState = state;
            }
            else if (animation.name.Equals("Die", StringComparison.OrdinalIgnoreCase))
            {
                dieState = state;
            }

            state.motion = clip;
        }

        // Set Idle as the default state
        if (idleState != null)
        {
            animatorController.layers[0].stateMachine.defaultState = idleState;
        }

        // Create transitions between Idle, Run, and Die states
        if (idleState != null && runState != null)
        {
            // Idle -> Run
            AnimatorStateTransition transitionToRun = idleState.AddTransition(runState);
            transitionToRun.AddCondition(AnimatorConditionMode.If, 0, "isMoving");

            // Run -> Idle
            AnimatorStateTransition transitionToIdle = runState.AddTransition(idleState);
            transitionToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "isMoving");
        }

        if (idleState != null && dieState != null)
        {
            // Idle -> Die
            AnimatorStateTransition transitionToDieFromIdle = idleState.AddTransition(dieState);
            transitionToDieFromIdle.AddCondition(AnimatorConditionMode.If, 0, "isDead");
        }

        if (runState != null && dieState != null)
        {
            // Run -> Die
            AnimatorStateTransition transitionToDieFromRun = runState.AddTransition(dieState);
            transitionToDieFromRun.AddCondition(AnimatorConditionMode.If, 0, "isDead");
        }
        
        if (dieState != null)
        {
            // Die -> Exit
            AnimatorStateTransition transitionToExit = dieState.AddExitTransition();
            transitionToExit.hasExitTime = true;
            transitionToExit.exitTime = 1.0f; // This ensures the entire Die animation plays before exiting
        }

        // Assign the Animator Controller
        animator.runtimeAnimatorController = animatorController;

        // Set the sprite renderer to use the idle sprite by default
        if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
        
        // Add a CapsuleCollider2D component
        var collider = unitPrefab.AddComponent<CapsuleCollider2D>();
        // Set the size of the collider
        if (idleCanvasSize != null)
        {
            // Normalize the canvas size by 64 and then scale by 0.3 (this is what we had for the original dude)
            float normalizedWidth = (float)idleCanvasSize.width / 64f;
            float normalizedHeight = (float)idleCanvasSize.height / 64f;

            collider.size = new Vector2(normalizedWidth * 0.3f, normalizedHeight * 0.3f);
        }

        collider.isTrigger = true;
        collider.usedByEffector = false;
        collider.direction = CapsuleDirection2D.Vertical;

        // Add a Rigidbody2D component
        var rigidbody = unitPrefab.AddComponent<Rigidbody2D>();
        // Set Rigidbody2D properties
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.gravityScale = 0;
        rigidbody.simulated = true;
        rigidbody.useAutoMass = false;
        rigidbody.mass = 1;
        rigidbody.drag = 0;
        rigidbody.angularDrag = 0.05f;
        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
        rigidbody.interpolation = RigidbodyInterpolation2D.None;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Generate a new script extending the appropriate abstract class and place it in the unit's directory
        string scriptPath = Path.Combine(unitDir, $"{unitName}.cs");
        File.WriteAllText(scriptPath, GenerateScriptContent(unitName, generaName, unit.speed, unit.health));
        AssetDatabase.ImportAsset(scriptPath);

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
        return $@"using UnityEngine;
using Creatures.Api;

namespace Creatures.impl
{{
    public class {unitName} : {generaName}
    {{
        
        void Start()
        {{
            Speed = {speed}f;
            HealthScript = gameObject.AddComponent<HealthScript>();
            HealthScript.health = {health};

            // Add custom initialization logic here
        }}

        // Optional: Override the Update method if custom logic is needed
        protected override void Update()
        {{
            base.Update();

            // Add custom update logic here, if necessary
        }}
    }}
}}
";
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
