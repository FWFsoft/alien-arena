using UnityEngine;
using Creatures.api;
using Creatures.api.abilities;

namespace Creatures.control
{
    public class PlayableCreatureController : MonoBehaviour
    {
        // TODO: Need to figure out how to dependency inject this
        private PlayableCreatureBase character;

        private void Start()
        {
            character = GetComponent<PlayableCreatureBase>();

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
            BasicAttackEvent basicAttackEvent = new BasicAttackEvent(); 
            ExecuteAbility(basicAttackEvent);
        }

        private void HandleStartCharging()
        {
            StartChargingEvent startChargingEvent = new StartChargingEvent(); 
            ExecuteAbility(startChargingEvent);
        }

        private void HandleChargedAbility()
        {
            ChargedAbilityEvent chargedAbilityEvent = new ChargedAbilityEvent(); 
            ExecuteAbility(chargedAbilityEvent);
        }

        private void HandleCharacterAbility()
        {
            CharacterAbilityEvent characterAbilityEvent = new CharacterAbilityEvent(); 
            ExecuteAbility(characterAbilityEvent);
        }

        private void HandleMobilityAbility()
        {
            MobilityAbilityEvent mobilityAbilityEvent = new MobilityAbilityEvent(); 
            ExecuteAbility(mobilityAbilityEvent);
        }

        private void HandleCoreInfusionAbility()
        {
            CoreInfusionAbilityEvent coreInfusionAbilityEvent = new CoreInfusionAbilityEvent();
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

