#region

using UnityEngine;

#endregion

namespace Feature.Common.Parameter
{
    [CreateAssetMenu(fileName = "Enemy1Params.asset", menuName = "Enemy1Params")]
    public class Enemy1Params : EnemyParamBase
    {
        public float attackIntervalSec = 4f;
        public float bulletSpeed = 1f;
        public float destroyDelay = 2f;
    }
}