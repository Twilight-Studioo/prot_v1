#region

using UniRx;
using UnityEngine;

#endregion

namespace Feature.Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SwapItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer material;

        public readonly IReactiveProperty<Vector2> Position = new ReactiveProperty<Vector2>();

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
    }
}