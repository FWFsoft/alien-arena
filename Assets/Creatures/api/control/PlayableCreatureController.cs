using Creatures.api;
using Creatures.api.abilities;
using Creatures.api.abilities.basic;
using Creatures.api.abilities.character;
using Creatures.api.abilities.charged;
using Creatures.api.abilities.infusion;
using Creatures.api.abilities.mobility;

using UnityEngine;


namespace Creatures.control
{
    public class PlayableCreatureController : MonoBehaviour
    {
        //When we implement character selection, this will need to be dynamically set
        [SerializeField]
        private PlayableCreatureBase character;

        private BasicAttackEvent basicAttackEvent;
        private StartChargingEvent startChargingEvent;
        private ChargedAbilityEvent chargedAbilityEvent;
        private CharacterAbilityEvent characterAbilityEvent;
        private MobilityAbilityEvent mobilityAbilityEvent;
        private CoreInfusionAbilityEvent coreInfusionAbilityEvent;

        private void Start()
        {
            // Subscribe to InputManager events
            InputManager.Instance.OnBasicAttack += HandleBasicAttack;
            InputManager.Instance.OnStartCharging += HandleStartCharging;
            InputManager.Instance.OnChargedAbility += HandleChargedAbility;
            InputManager.Instance.OnCharacterAbility += HandleCharacterAbility;
            InputManager.Instance.OnMobilityAbility += HandleMobilityAbility;
            InputManager.Instance.OnCoreInfusionAbility += HandleCoreInfusionAbility;
            InputManager.Instance.OnMove += HandleMove;
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
                InputManager.Instance.OnMove -= HandleMove;
            }
        }

        private void HandleMove(Vector2 inputDirection)
        {
            character.HandleMovement(inputDirection);
        }

        private void HandleBasicAttack(Vector3 mousePosition)
        {
            ExecuteAbility(character.GetAbility<BasicAttackEvent>(), mousePosition);
        }

        private void HandleStartCharging(Vector3 mousePosition)
        {
            ExecuteAbility(character.GetAbility<StartChargingEvent>(), mousePosition);
        }

        private void HandleChargedAbility(Vector3 mousePosition)
        {
            ExecuteAbility(character.GetAbility<ChargedAbilityEvent>(), mousePosition);
        }

        private void HandleCharacterAbility(Vector3 mousePosition)
        {
            ExecuteAbility(character.GetAbility<CharacterAbilityEvent>(), mousePosition);
        }

        private void HandleMobilityAbility(Vector3 mousePosition)
        {
            ExecuteAbility(character.GetAbility<MobilityAbilityEvent>(), mousePosition);
        }

        private void HandleCoreInfusionAbility(Vector3 mousePosition)
        {
            ExecuteAbility(character.GetAbility<CoreInfusionAbilityEvent>(), mousePosition);
        }

        private void ExecuteAbility(AbilityEvent abilityEvent, Vector3 mousePosition)
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

