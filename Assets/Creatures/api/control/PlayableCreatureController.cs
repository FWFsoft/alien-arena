using UnityEngine;
using Creatures.api;
using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;


namespace Creatures.control
{
    public class PlayableCreatureController : MonoBehaviour
    {
        // TODO: Need to figure out how to dependency inject this
        private PlayableCreatureBase character;

        private BasicAttackEvent basicAttackEvent;
        private StartChargingEvent startChargingEvent;
        private ChargedAbilityEvent chargedAbilityEvent;
        private CharacterAbilityEvent characterAbilityEvent;
        private MobilityAbilityEvent mobilityAbilityEvent;
        private CoreInfusionAbilityEvent coreInfusionAbilityEvent;
        
        private void Start()
        {
            character = GetComponent<PlayableCreatureBase>();

            basicAttackEvent = new BasicAttackEvent();
            startChargingEvent = new StartChargingEvent();
            chargedAbilityEvent = new ChargedAbilityEvent();
            characterAbilityEvent = new CharacterAbilityEvent();
            mobilityAbilityEvent = new MobilityAbilityEvent();
            coreInfusionAbilityEvent = new CoreInfusionAbilityEvent();

            // Subscribe to InputManager events
            InputManager.Instance.OnBasicAttack += HandleBasicAttack;
            InputManager.Instance.OnStartCharging += HandleStartCharging;
            InputManager.Instance.OnChargedAbility += HandleChargedAbility;
            InputManager.Instance.OnCharacterAbility += HandleCharacterAbility;
            InputManager.Instance.OnMobilityAbility += HandleMobilityAbility;
            InputManager.Instance.OnCoreInfusionAbility += HandleCoreInfusionAbility;
        }

        private void OnDestroy()
        {
            // Unsubscribe from InputManager events
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnBasicAttack -= HandleBasicAttack;
                InputManager.Instance.OnStartCharging -= HandleStartCharging;
                InputManager.Instance.OnChargedAbility -= HandleChargedAbility;
                InputManager.Instance.OnCharacterAbility -= HandleCharacterAbility;
                InputManager.Instance.OnMobilityAbility -= HandleMobilityAbility;
                InputManager.Instance.OnCoreInfusionAbility -= HandleCoreInfusionAbility;
            }
        }

        private void HandleBasicAttack(Vector2 mousePosition)
        {
            ExecuteAbility(basicAttackEvent, mousePosition);
        }

        private void HandleStartCharging(Vector2 mousePosition)
        {
            ExecuteAbility(startChargingEvent, mousePosition);
        }

        private void HandleChargedAbility(Vector2 mousePosition)
        {
            ExecuteAbility(chargedAbilityEvent, mousePosition);
        }

        private void HandleCharacterAbility(Vector2 mousePosition)
        {
            ExecuteAbility(characterAbilityEvent, mousePosition);
        }

        private void HandleMobilityAbility(Vector2 mousePosition)
        {
            ExecuteAbility(mobilityAbilityEvent, mousePosition);
        }

        private void HandleCoreInfusionAbility(Vector2 mousePosition)
        {
            ExecuteAbility(coreInfusionAbilityEvent, mousePosition);
        }

        private void ExecuteAbility(AbilityEvent abilityEvent, Vector2 mousePosition)
        {
            // TODO: just logging right now, but we may want to fire VO for the character here
            // The classic "I can't do that right now."
            AbilityExecutionResult result = abilityEvent.Execute(character, mousePosition);
            switch (result)
            {
                case AbilityExecutionResult.Success:
                    // Ability executed successfully
                    Debug.Log("Ability executed successfully.");
                    break;
                case AbilityExecutionResult.OnCooldown:
                    // Handle cooldown case
                    Debug.LogWarning("Ability is on cooldown.");
                    break;
                case AbilityExecutionResult.Disabled:
                    // Handle disabled case
                    Debug.LogWarning("Ability is currently disabled.");
                    break;
                default:
                    Debug.LogError("Unexpected AbilityExecutionResult.");
                    break;
            }
        }
    }
}

