#region

using UnityEngine;

#endregion

namespace Feature.Common.Parameter
{
    [CreateAssetMenu(fileName = "CharacterParams.asset", menuName = "CharacterParams", order = 0)]
    public class CharacterParams : ScriptableObject
    {
        public float Speed = 1f;

        public float JumpPower = 1f;

        public float SwapTimeSec = 5f;

        public float CanSwapDistance = 6f;

        /// <summary>
        ///     スワップ後にTimeScaleが元に戻るまでの間隔
        /// </summary>
        public float AfterSwappedTimeSec = 0.5f;

        public float InSwapTimeScale = 0.5f;
    }
}