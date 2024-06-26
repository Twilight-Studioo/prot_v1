#region

using System;
using Core.Utilities;
using UniRx;
using UnityEngine;

#endregion

namespace Feature.Views
{
    /// <summary>
    ///     プレイヤーのView
    ///     具体的な操作はPresenterに任せる(ここでは何もしない)
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerView : MonoBehaviour
    {
        private CompositeDisposable highLightDisposable;
        private Vector2 pendingForce = Vector2.zero;

        //    private Vector2? pendingPosition = null;
        private Vector2? pendingVelocity;
        private Rigidbody2D rigidBody2d;
        private SpriteRenderer spriteRenderer;

        public IReactiveProperty<Vector2> Position { get; private set; }

        private void Awake()
        {
            Position = new ReactiveProperty<Vector2>(transform.position);
            highLightDisposable = new();
        }

        private void Start()
        {
            rigidBody2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white;
        }

        private void Update()
        {
            Position.Value = transform.position;
        }

        private void FixedUpdate()
        {
            // 物理的な変更を適用
            if (pendingForce != Vector2.zero)
            {
                rigidBody2d.AddForce(pendingForce, ForceMode2D.Force);
                pendingForce = Vector2.zero; // 力を適用した後はリセット
            }

            if (pendingVelocity.HasValue)
            {
                rigidBody2d.velocity = new(pendingVelocity.Value.x, rigidBody2d.velocity.y);
                pendingVelocity = null;
            }

            // if (pendingPosition.HasValue)
            // {
            //     rigidBody2d.MovePosition(pendingPosition.Value);
            //     pendingPosition = null;
            // }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            OnHit?.Invoke(other.collider);
        }

        private void OnDrawGizmos()
        {
            // Gizmos 用の描画を呼び出す
            RaycastEx.DrawGizmos();
        }

        /// <summary>
        ///     colliderに当たった時のイベント通知
        /// </summary>
        public event Action<Collider2D> OnHit;


        /// <summary>
        ///     指定方向に力を加える
        /// </summary>
        /// <param name="direction">方向</param>
        public void AddForce(Vector2 direction)
        {
            pendingForce += direction;
        }

        /// <summary>
        ///     速度を直接 設定する
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocity(Vector2 velocity)
        {
            pendingVelocity = new Vector2(velocity.x, rigidBody2d.velocity.y + velocity.y);
        }

        /// <summary>
        ///     位置を直接 設定する
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void MovePosition(Vector2 position)
        {
            rigidBody2d.MovePosition(position);
        }

        public void StartHighLight(float delaySec)
        {
            highLightDisposable.Clear();
            spriteRenderer.color = Color.red;
            Observable.Timer(TimeSpan.FromSeconds(delaySec))
                .Subscribe(__ => { spriteRenderer.color = Color.white; })
                .AddTo(highLightDisposable);
        }
    }
}