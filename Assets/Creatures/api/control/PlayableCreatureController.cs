using UnityEngine;
using Creatures.api;
using Creatures.api.abilities;

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

        private void HandleBasicAttack()
        {
            ExecuteAbility(basicAttackEvent);
        }

        private void HandleStartCharging()
        {
            ExecuteAbility(startChargingEvent);
        }

        private void HandleChargedAbility()
        {
            ExecuteAbility(chargedAbilityEvent);
        }

        private void HandleCharacterAbility()
        {
            ExecuteAbility(characterAbilityEvent);
        }

        private void HandleMobilityAbility()
        {
            ExecuteAbility(mobilityAbilityEvent);
        }

        private void HandleCoreInfusionAbility()
        {
            ExecuteAbility(coreInfusionAbilityEvent);
        }

        private void ExecuteAbility(AbilityEvent abilityEvent)
        {
            // TODO: just logging right now, but we may want to fire VO for the character here
            // The classic "I can't do that right now."
            AbilityExecutionResult result = abilityEvent.Execute();
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

