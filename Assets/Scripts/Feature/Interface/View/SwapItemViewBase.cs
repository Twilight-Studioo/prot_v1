#region

using System;
using Core.Utilities;
using UniRx;
using UnityEngine;

#endregion

namespace Feature.Interface.View
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public abstract class SwapItemViewBase : MonoBehaviour, IDisposable
    {
        [SerializeField] private SpriteRenderer material;

        // ReSharper disable once MemberCanBePrivate.Local
        public readonly IReactiveProperty<Vector2> Position = new ReactiveProperty<Vector2>();
        [NonSerialized] protected bool IsActive;

        protected virtual void Start()
        {
            IsActive = true;
        }

        private void Update()
        {
            Position.Value = transform.position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTrigger?.Invoke(other);
        }

        public virtual void Dispose()
        {
            OnDestroy = null;
            OnTrigger = null;
        }

        public event Action OnDestroy;

        protected event Action<Collider2D> OnTrigger;

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetHighlight(bool isHighlight)
        {
            if (material.IsNull())
            {
                return;
            }

            material.color = isHighlight ? Color.red : Color.blue;
        }

        protected void Delete()
        {
            OnDestroy?.Invoke();
            this.DestroyGameObject();
        }
    }
}