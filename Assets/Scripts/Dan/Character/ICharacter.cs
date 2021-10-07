using System;

namespace Dan.Character
{
    public interface ICharacter
    {
        int MaxHitPoints { get; }
        int HitPoints { get; }
        float MoveSpeed { get; }
        bool IsDead { get; }

        event Action OnCharacterDeath;
        event Action OnCharacterHit;

        void Hit();
        void Die();
    }
}