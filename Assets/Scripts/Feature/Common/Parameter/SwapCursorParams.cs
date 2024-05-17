#region

using UnityEngine;

#endregion

namespace Feature.Common.Parameter
{
    [CreateAssetMenu(fileName = "SwapCursorParams", menuName = "SwapCursorParams", order = 0)]
    public class SwapCursorParams : ScriptableObject
    {
        public float moveSpeed = 1f;

        public float mouseAdSpeed = 1f;

        public float assistedDistance = 5f;

        public float assistPower = 1f;
    }
}