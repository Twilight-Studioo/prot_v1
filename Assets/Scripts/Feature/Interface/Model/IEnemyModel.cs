using System;
using UniRx;
using UnityEngine;

namespace Feature.Interface.Model
{
    public interface IEnemyModel
    {
        public IReadOnlyReactiveProperty<bool> IsDead { get; }

        public IReactiveProperty<uint> Health { get; }
        
        public IReadOnlyReactiveProperty<Vector2> Position { get; set; }
        
    }
}