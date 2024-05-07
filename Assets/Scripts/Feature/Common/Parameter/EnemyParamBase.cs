using Feature.Interface.View;
using UnityEngine;

namespace Feature.Common.Parameter
{
    public abstract class EnemyParamBase : ScriptableObject
    {
        [Range(0, 1000)]
        public uint health = 100;
        
        [Range(0.1f, 10.0f)]
        public float speed = 1f;
        
        [Range(0.1f, 100.0f)]
        public float attackPower = 1f;
        
        public EnemyAttackType attackType = EnemyAttackType.Default;

        public GameObject prefab;
        public EnemyViewBase View() => prefab.GetComponent<EnemyViewBase>();
    }
}