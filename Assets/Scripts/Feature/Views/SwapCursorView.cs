using System;
using Core.Utilities;
using UniRx;
using UnityEngine;

namespace Feature.Views
{
    public class SwapCursorView: MonoBehaviour
    {
        public IReactiveProperty<Vector2> Position;

        private void Awake()
        {
            Position = new ReactiveProperty<Vector2>();
        }

        private void FixedUpdate()
        {
            Position.Value = transform.position;
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos.ToVector3(SysEx.Unity.ZIndex.Item);
        }
    }
}