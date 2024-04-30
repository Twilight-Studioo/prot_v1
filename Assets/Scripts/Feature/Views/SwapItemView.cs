#region

using System;
using UniRx;
using UnityEngine;

#endregion

namespace Feature.Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SwapItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer material;

        public IReactiveProperty<Vector2> Position;

        private void Awake()
        {
            Position = new ReactiveProperty<Vector2>(transform.position);
        }
        
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetHighlight(bool isHighlight)
        {
            material.color = isHighlight ? Color.red : Color.blue;
        }

        private void FixedUpdate()
        {
            Position.Value = transform.position;
        }
    }
}