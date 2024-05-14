#region

using UniRx;
using UnityEngine;

#endregion

namespace Feature.Model
{
    public class SwapCursorModel
    {
        public IReadOnlyReactiveProperty<Vector2> Position = new ReactiveProperty<Vector2>(new());
    }
}