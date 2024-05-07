using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Feature.Interface.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class EnemyViewBase: MonoBehaviour, IDisposable
    {
        private SpriteRenderer material;

        public readonly IReactiveProperty<Vector2> Position = new ReactiveProperty<Vector2>();

        private void Start()
        {
            material = GetComponent<SpriteRenderer>();
            material.color = Color.gray;
        }

        private void Update()
        {
            Position.Value = transform.position;
        }

        public abstract void SetPosition(Vector2 position);

        public abstract void Dead();

        public abstract void Spawned();

        public abstract void Dispose();

        private bool isColorFix = false;

        public void OnDamage()
        {
            material.color = Color.red;
            MainThreadDispatcher.StartCoroutine(ARateColor());
        }

        private IEnumerator ARateColor()
        {
            if (isColorFix)
            {
                yield break;
            }

            isColorFix = true;
            yield return new WaitForSeconds(0.5f);
            material.color = Color.gray;

        }

        public abstract SwapItemViewBase GetItemInstance();

    }
}