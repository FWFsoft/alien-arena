using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Creatures.api.abilities.states;

namespace Creatures.api.abilities
{
    public class CooldownMediator
    {
        private readonly Dictionary<AbilityIdentifier, CancellationTokenSource> _activeCooldowns = new();

        /// <summary>
        /// Starts a cooldown for the specified ability, ensuring that the ability remains on cooldown for 
        /// the specified duration. If an existing cooldown is active for the ability, it will be canceled first.
        /// </summary>
        /// <param name="cooldownState">The current state of the ability, which will transition once the cooldown ends.</param>
        /// <param name="abilityId">The unique identifier of the ability for which the cooldown is being started.</param>
        /// <param name="cooldownDuration">The duration of the cooldown in seconds.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. This Task will complete when the cooldown
        /// either finishes normally or is canceled.
        /// </returns>
        /// <remarks>
        /// The method ensures that:
        /// <list type="number">
        /// <item><description>Any existing cooldown for the ability is canceled.</description></item>
        /// <item><description>The cooldown timer runs asynchronously, and the cooldown state is notified when it completes.</description></item>
        /// <item><description>The cooldown entry is removed from the tracking dictionary upon completion or cancellation.</description></item>
        /// </list>
        /// </remarks>
        public async Task StartCooldown(CooldownState cooldownState, AbilityIdentifier abilityId, float cooldownDuration)
        {
            if (_activeCooldowns.TryGetValue(abilityId, out var existingCancellationToken))
            {
                existingCancellationToken.Cancel(); // Cancel any existing cooldown
            }
            
            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                _activeCooldowns[abilityId] = cancellationTokenSource;
                await Task.Delay(TimeSpan.FromSeconds(cooldownDuration), cancellationTokenSource.Token);
            }
            finally
            {
                cooldownState.OnComplete();
                _activeCooldowns.Remove(abilityId);
            }
        }

        
        /// <summary>
        /// Cancels the active cooldown for the specified ability, causing the cooldown to complete immediately.
        /// </summary>
        /// <param name="abilityId">The unique identifier of the ability whose cooldown should be canceled.</param>
        /// <remarks>
        /// This method cancels the ongoing cooldown for the ability identified by <paramref name="abilityId"/>.
        /// If no cooldown is active for the given ability, the method does nothing.
        /// </remarks>
        public void RefreshCooldown(AbilityIdentifier abilityId)
        {
            if (_activeCooldowns.TryGetValue(abilityId, out var cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
            }
        }

        
        /// <summary>
        /// Cancels all active cooldowns, causing all abilities to complete their cooldowns immediately.
        /// </summary>
        /// <remarks>
        /// This method iterates through all active cooldowns and cancels them. Each ability with an active cooldown 
        /// will have its cooldown timer canceled, triggering the cooldown completion process.
        /// </remarks>
        public void RefreshAllCooldowns()
        {
            foreach (var cts in _activeCooldowns.Values)
            {
                cts.Cancel();
            }
        }
    }
}
