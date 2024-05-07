using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feature.Interface.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class SwapItemViewBase: MonoBehaviour
    {
        [NonSerialized] protected bool IsActive;
    
        [SerializeField] private SpriteRenderer material;

        public readonly IReactiveProperty<Vector2> Position = new ReactiveProperty<Vector2>();

        public event Action OnDestroy;

        private void Start()
        {
            IsActive = true;
        }

        private void Update()
        {
            Position.Value = transform.position;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetHighlight(bool isHighlight)
        {
            material.color = isHighlight ? Color.red : Color.blue;
        }

        public void Delete()
        {
            OnDestroy?.Invoke();
            Destroy(this);
        }
    }
}