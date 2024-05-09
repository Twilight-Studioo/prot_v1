#region

using Core.Utilities;
using Feature.Common.Parameter;
using UniRx;
using UnityEngine;
using VContainer;

#endregion

namespace Feature.Model
{
    /// <summary>
    ///     プレイヤーの状態を管理するクラス
    /// </summary>
    public class PlayerModel
    {
        private readonly CharacterParams characterParams;

        public Vector2 Forward;

        [Inject]
        public PlayerModel(
            CharacterParams characterParams
        )
        {
            this.characterParams = characterParams;

            Health = new ReactiveProperty<int>(characterParams.health);
            StayGround = new ReactiveProperty<bool>(true);
            IsDead = Health.Select(x => x <= 0).ToReadOnlyReactiveProperty();
            Position = new ReactiveProperty<Vector2>(Vector2.zero);
        }

        public IReactiveProperty<int> Health { get; }

        public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }

        public IReactiveProperty<bool> StayGround { get; }

        public float Speed => characterParams.speed;

        public float JumpPower => characterParams.jumpPower;

        public int AttackPower => characterParams.attackPower;

        public IReactiveProperty<Vector2> Position { get; }

        public void SetStayGround(bool stayGround)
        {
            StayGround.Value = stayGround;
        }

        public void SetPosition(Vector2 position)
        {
            Position.Value = position;
        }

        public void SetDirection(Vector2 forward)
        {
            if (forward.x == 0f) return;
            Forward = forward.x > 0 ? Vector2.right : Vector2.left;
        }

        public int SetHealth(ushort health)
        {
            Health.Value = health;
            return Health.Value;
        }

        public void Damage(ushort damage)
        {
            Health.Value -= damage;
        }

        public Vector2 AttachPoint() => Position.Value + Forward * 1.5f;
    }
}