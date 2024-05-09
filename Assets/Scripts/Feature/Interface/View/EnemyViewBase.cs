#region

using System;
using UniRx;
using UnityEngine;

#endregion

namespace Feature.Interface.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class EnemyViewBase : MonoBehaviour, IDisposable
    {
        public readonly IReactiveProperty<Vector2> Position = new ReactiveProperty<Vector2>();
        private CompositeDisposable damageDisposable;

        private SpriteRenderer material;

        private void Start()
        {
            material = GetComponent<SpriteRenderer>();
            material.color = Color.gray;
            damageDisposable = new();
        }

        protected virtual void Update()
        {
            Position.Value = transform.position;
        }

        public virtual void Dispose()
        {
            damageDisposable.Dispose();
        }

        public event Action<int> OnDamage;

        public abstract void SetPosition(Vector2 position);

        public virtual void Dead()
        {
            damageDisposable.Dispose();
        }

        public abstract void Spawned();


        public abstract SwapItemViewBase GetItemInstance();

        public void OnTakeDamage(int damage)
        {
            damageDisposable.Clear();
            material.color = Color.red;
            Observable
                .Timer(TimeSpan.FromSeconds(0.3f))
                .Subscribe(_ => { material.color = Color.gray; })
                .AddTo(damageDisposable);
            OnDamage?.Invoke(damage);
        }
    }
}