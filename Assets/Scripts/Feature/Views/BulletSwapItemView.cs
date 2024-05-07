using Feature.Interface.View;
using UniRx;
using UnityEngine;

namespace Feature.Views
{
    public class BulletSwapItemView: SwapItemViewBase
    {
        public void ThrowStart(Vector2 position, Vector2 direction, float speed)
        {
            transform.position = position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Observable
                .EveryFixedUpdate()
                .Where(_ => IsActive)
                .Select(_ => direction * 0.1f * speed)
                .Subscribe(moveDistance =>
                {
                    transform.position += new Vector3(moveDistance.x, moveDistance.y);
                })
                .AddTo(this);
        }
        
        
    }
}