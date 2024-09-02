using Creatures.api.abilities;
using Creatures.api.abilities.basic;

namespace Creatures.Api
{
    public interface IPlayable
    {
        void BasicAttack(BasicAttackEvent action);
    }
}