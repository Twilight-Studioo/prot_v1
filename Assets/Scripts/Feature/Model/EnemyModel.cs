#region

using Feature.Common.Parameter;
using Feature.Interface.Model;
using UniRx;
using UnityEngine;

#endregion

namespace Feature.Model
{
    public static class EnemyModelFactory
    {
        public static EnemyModel Create(this Enemy1Params param) => new(param);
    }

    public class EnemyModel : IEnemyModel
    {
        public EnemyModel(
            Enemy1Params param
        )
        {
            Health = new ReactiveProperty<uint>(param.health);
            IsDead = Health.Select(x => x <= 0).ToReactiveProperty();
        }

        public IReactiveProperty<uint> Health { get; }
        public IReadOnlyReactiveProperty<Vector2> Position { get; set; }

        public IReadOnlyReactiveProperty<bool> IsDead { get; }
    }
}