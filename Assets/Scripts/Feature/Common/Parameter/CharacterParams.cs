#region

using UnityEngine;

#endregion

namespace Feature.Common.Parameter
{
    [CreateAssetMenu(fileName = "CharacterParams.asset", menuName = "CharacterParams", order = 0)]
    public class CharacterParams : ScriptableObject
    {
        public int health = 100;

        public float speed = 1f;

        public float jumpPower = 1f;

        public float swapTimeSec = 3f;

        public float canSwapDistance = 40f;

        /// <summary>
        ///     スワップ後にTimeScaleが元に戻るまでの間隔
        /// </summary>
        public float afterSwappedTimeSec = 0.5f;

        public float inSwapTimeScale = 0.3f;

        public int attackPower = 10;
    }
}