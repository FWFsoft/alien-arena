using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Creatures.Api;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class CreatureHydrator : Editor
{
    [MenuItem("Tools/Hydrate Creatures")]
    public static void HydrateCreatures()
    {
        string rootGeneraPath = "Assets/Creatures/impl/NonPlayable";
        string[] generaDirectories = Directory.GetDirectories(rootGeneraPath);

        foreach (var generaDir in generaDirectories)
        {
            HydrateAllUnitsForGenera(generaDir);
        }
    }

    private static void HydrateAllUnitsForGenera(string generaDir)
    {
        string generaName = Path.GetFileName(generaDir);
        string abstractClassPath = Path.Combine(generaDir, $"{generaName}.cs");

        if (!File.Exists(abstractClassPath))
        {
            Debug.LogError($"Abstract class for {generaName} not found at {abstractClassPath}");
            return;
        }

        string[] unitDirectories = Directory.GetDirectories(generaDir);

        foreach (var unitDir in unitDirectories)
        {
            HydrateGeneraUnit(unitDir, generaName);
        }
    }

    private static void HydrateGeneraUnit(string unitDir, string generaName)
    {
        string unitName = Path.GetFileName(unitDir);
        string unitYamlPath = Path.Combine(unitDir, $"{unitName}.yml");

        if (!File.Exists(unitYamlPath))
        {
            Debug.LogError($"YAML file for unit {unitName} not found at {unitYamlPath}");
            return;
        }

        string yamlContent = File.ReadAllText(unitYamlPath);
        UnitDefinition unit = DeserializeYaml<UnitDefinition>(yamlContent);

        // Hydrate or update the creature using the spec and directory
        HydrateOrUpdateCreature(unit, generaName, unitName, unitDir);
    }

    private static T DeserializeYaml<T>(string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<T>(yamlContent);
    }

    private static void HydrateOrUpdateCreature(UnitDefinition unit, string generaName, string unitName, string unitDir)
    {
        string prefabPath = Path.Combine(unitDir, $"{unitName}.prefab");
        GameObject unitPrefab;

        // Check if the prefab already exists
        if (File.Exists(prefabPath))
        {
            // Load the existing prefab
            unitPrefab = PrefabUtility.LoadPrefabContents(prefabPath);
        }
        else
        {
            // Create a new prefab
            unitPrefab = new GameObject(unitName);
        }

        // Add or update SpriteRenderer
        SpriteRenderer spriteRenderer = unitPrefab.GetComponent<SpriteRenderer>() ?? unitPrefab.AddComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;

        // Add or update Animator
        Animator animator = unitPrefab.GetComponent<Animator>() ?? unitPrefab.AddComponent<Animator>();

        // Create or load the Animator Controller within the unit's directory
        string animatorPath = Path.Combine(unitDir, $"{unitName}Controller.controller");
        AnimatorController animatorController;

        if (File.Exists(animatorPath))
        {
            // Load the existing Animator Controller
            animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(animatorPath);
        }
        else
        {
            // Create a new Animator Controller
            animatorController = AnimatorController.CreateAnimatorControllerAtPath(animatorPath);
        }

        // Ensure 'isMoving' and 'isDead' parameters are present before setting transitions
        AddAnimatorParameterIfNotExists(animatorController, "isMoving", AnimatorControllerParameterType.Bool);
        AddAnimatorParameterIfNotExists(animatorController, "isDead", AnimatorControllerParameterType.Bool);

        // Initialize the Idle sprite
        Sprite idleSprite = null;

        // Initialize the Idle canvas size
        CanvasSize idleCanvasSize = null;

        // Create or update states for the animations
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

            // Create and add or update AnimationClip to AnimatorController
            AnimationClip clip = CreateOrUpdateAnimationClip(sprites, animationSpec.frames, animation.name, animationSpec.frameRate, animationFolderPath);
            AnimatorState state = GetOrCreateAnimatorState(animatorController, animation.name);

            BaseAnimation baseAnimation = (BaseAnimation)Enum.Parse(typeof(BaseAnimation), animation.name, true);

            switch (baseAnimation)
            {
                case BaseAnimation.Run:
                    runState = state;
                    break;
                case BaseAnimation.Idle:
                    idleState = state;
                    idleSprite = sprites[0];  // Set the idle sprite for when/if no animations are playing
                    idleCanvasSize = animationSpec.canvas; // Capture the canvas size from Idle.yml
                    break;
                case BaseAnimation.Die:
                    dieState = state;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            state.motion = clip;
        }

        // Set Idle as the default state
        if (idleState != null)
        {
            animatorController.layers[0].stateMachine.defaultState = idleState;
        }

        // Create or update transitions between Idle, Run, and Die states
        CreateOrUpdateTransitions(animatorController, idleState, runState, dieState);

        // Assign the Animator Controller
        animator.runtimeAnimatorController = animatorController;

        // Set the sprite renderer to use the idle sprite by default
        if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }

        // Add or update a CapsuleCollider2D component
        UpdateOrAddCollider(unitPrefab, idleCanvasSize);

        // Add or update a Rigidbody2D component
        UpdateOrAddRigidbody(unitPrefab);

        // Generate a new script extending the appropriate abstract class and place it in the unit's directory
        string scriptPath = Path.Combine(unitDir, $"{unitName}.cs");
        File.WriteAllText(scriptPath, GenerateScriptContent(unitName, generaName, unit.speed, unit.health));
        AssetDatabase.ImportAsset(scriptPath);

        // Save the prefab in the unit's directory
        PrefabUtility.SaveAsPrefabAsset(unitPrefab, prefabPath);
        PrefabUtility.UnloadPrefabContents(unitPrefab);
    }

    private static void CreateOrUpdateTransitions(AnimatorController controller, AnimatorState idleState, AnimatorState runState, AnimatorState dieState)
    {
        // Ensure parameters exist before creating transitions
        var isMovingParam = controller.parameters.FirstOrDefault(p => p.name == "isMoving");
        var isDeadParam = controller.parameters.FirstOrDefault(p => p.name == "isDead");

        if (isMovingParam != null && idleState != null && runState != null)
        {
            // Idle -> Run
            CreateOrUpdateTransition(idleState, runState, isMovingParam.name, true);

            // Run -> Idle
            CreateOrUpdateTransition(runState, idleState, isMovingParam.name, false);
        }
        else
        {
            Debug.LogWarning("Missing 'isMoving' parameter or states when creating transitions between Idle and Run.");
        }

        if (isDeadParam != null && idleState != null && dieState != null)
        {
            // Idle -> Die
            CreateOrUpdateTransition(idleState, dieState, isDeadParam.name, true);
        }

        if (isDeadParam != null && runState != null && dieState != null)
        {
            // Run -> Die
            CreateOrUpdateTransition(runState, dieState, isDeadParam.name, true);
        }

        if (dieState != null)
        {
            // Die -> Exit
            var transitionToExit = dieState.AddExitTransition();
            transitionToExit.hasExitTime = true;
            transitionToExit.exitTime = 1.0f; // This ensures the entire Die animation plays before exiting
        }
    }


    private static void AddAnimatorParameterIfNotExists(AnimatorController animatorController, string parameterName, AnimatorControllerParameterType type)
    {
        if (!animatorController.parameters.Any(p => p.name == parameterName))
        {
            animatorController.AddParameter(parameterName, type);
        }
    }

    private static AnimatorState GetOrCreateAnimatorState(AnimatorController controller, string stateName)
    {
        var stateMachine = controller.layers[0].stateMachine;
        var state = stateMachine.states.FirstOrDefault(s => s.state.name == stateName).state;

        if (state == null)
        {
            state = stateMachine.AddState(stateName);
        }

        return state;
    }

    private static void CreateOrUpdateTransition(AnimatorState fromState, AnimatorState toState, string conditionName, bool conditionValue)
    {
        // Check if transition already exists
        var existingTransition = fromState.transitions.FirstOrDefault(t => t.destinationState == toState);

        if (existingTransition == null)
        {
            // Create a new transition if it doesn't exist
            existingTransition = fromState.AddTransition(toState);
        }

        // Clear existing conditions to avoid any leftover references
        existingTransition.conditions = null;

        // Set the condition with an explicit reference to the parameter by name
        existingTransition.AddCondition(conditionValue ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot, 0, conditionName);

        // Debug to verify that the transition is set correctly
        Debug.Log($"Transition from '{fromState.name}' to '{toState.name}' using parameter '{conditionName}' set to '{conditionValue}'");
    }

    private static void UpdateOrAddCollider(GameObject unitPrefab, CanvasSize idleCanvasSize)
    {
        // Add or update CapsuleCollider2D component
        var collider = unitPrefab.GetComponent<CapsuleCollider2D>() ?? unitPrefab.AddComponent<CapsuleCollider2D>();

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
    }

    private static void UpdateOrAddRigidbody(GameObject unitPrefab)
    {
        // Add or update Rigidbody2D component
        var rigidbody = unitPrefab.GetComponent<Rigidbody2D>() ?? unitPrefab.AddComponent<Rigidbody2D>();

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

    private static AnimationClip CreateOrUpdateAnimationClip(Sprite[] sprites, int frames, string animationName, int frameRate, string animationFolderPath)
    {
        string animFilePath = Path.Combine(animationFolderPath, $"{animationName}.anim");
        AnimationClip clip;

        // Check if the animation clip already exists
        if (File.Exists(animFilePath))
        {
            // Load existing animation clip
            clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animFilePath);

            // Clear existing curve before updating
            AnimationUtility.SetObjectReferenceCurve(clip, new EditorCurveBinding
            {
                path = "",
                propertyName = "m_Sprite",
                type = typeof(SpriteRenderer)
            }, null);
        }
        else
        {
            // Create a new animation clip
            clip = new AnimationClip();
        }

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

        // Set the animation to loop
        clip.wrapMode = WrapMode.Loop;
        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        // Save the animation clip if it's newly created
        if (!File.Exists(animFilePath))
        {
            AssetDatabase.CreateAsset(clip, animFilePath);
        }
        else
        {
            EditorUtility.SetDirty(clip);
        }

        return clip;
    }

    private static string GenerateScriptContent(string unitName, string generaName, float speed, int health)
    {
        return $@"using Creatures.Api;


using UnityEngine;


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
