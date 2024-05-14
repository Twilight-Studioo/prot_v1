#region

using UnityEngine;

#endregion

namespace Feature.Common.Parameter
{
    [CreateAssetMenu(fileName = "SwapCursorParams", menuName = "SwapCursorParams", order = 0)]
    public class SwapCursorParams : ScriptableObject
    {
        public float moveSpeed = 1f;
    }
}