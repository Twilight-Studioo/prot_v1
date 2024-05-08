#region

using System;
using Feature.Interface.View;
using UniRx;
using UnityEngine;

#endregion

namespace Feature.Views
{
    public class BulletSwapItemView : SwapItemViewBase
    {
        private readonly CompositeDisposable disposable = new();

        protected override void Start()
        {
            base.Start();
            OnTrigger += OnTriggered;
        }

        private void OnTriggered(Collider2D other)
        {
            if (other.CompareTag("Ground"))
            {
                Observable
                    .Timer(TimeSpan.FromSeconds(0.3f))
                    .Subscribe(_ => { Despawn(); })
                    .AddTo(this);
            }
        }

        public void ThrowStart(Vector2 position, Vector2 direction, float speed, float delay)
        {
            disposable.Clear();
            transform.position = position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Observable
                .EveryFixedUpdate()
                .Where(_ => IsActive)
                .Select(_ => direction * 0.1f * speed)
                .Subscribe(moveDistance => { transform.position += new Vector3(moveDistance.x, moveDistance.y); })
                .AddTo(disposable);
            Observable
                .Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(_ => { Despawn(); })
                .AddTo(this);
        }

        private void Despawn()
        {
            disposable.Clear();
            Delete();
        }


        public override void Dispose()
        {
            base.Dispose();
            disposable.Dispose();
        }
    }
}