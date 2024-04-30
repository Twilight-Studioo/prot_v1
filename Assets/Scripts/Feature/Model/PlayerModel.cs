#region

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
        [Inject]
        public PlayerModel(
            CharacterParams characterParams
        )
        {
            Speed = characterParams.Speed;
            JumpPower = characterParams.JumpPower;

            Health = new ReactiveProperty<ushort>(100);
            StayGround = new ReactiveProperty<bool>(true);
            IsDead = Health.Select(x => x <= 0).ToReadOnlyReactiveProperty();
            Position = new ReactiveProperty<Vector2>(Vector2.zero);
        }

        public IReactiveProperty<ushort> Health { get; }

        public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }

        public IReactiveProperty<bool> StayGround { get; }

        public float Speed { get; private set; }

        public float JumpPower { get; private set; }

        public IReactiveProperty<Vector2> Position { get; }

        public void SetStayGround(bool stayGround)
        {
            StayGround.Value = stayGround;
        }

        public void SetPosition(Vector2 position)
        {
            Position.Value = position;
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
    }
}