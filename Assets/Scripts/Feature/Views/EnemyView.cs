#region

using System;
using Core.Utilities;
using Feature.Interface.View;
using UnityEngine;

#endregion

namespace Feature.Views
{
    public class EnemyView : EnemyViewBase
    {
        [SerializeField] private GameObject bullet;

        private void Awake()
        {
            if (bullet.IsNull())
            {
                throw new NotImplementedException();
            }
        }

        public override void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public override void Dead()
        {
            base.Dead();
            Destroy(gameObject);
        }

        public override void Spawned()
        {
        }


        public override SwapItemViewBase GetItemInstance()
        {
            var @object = Instantiate(bullet.GetComponent<BulletSwapItemView>(), Position.Value, Quaternion.identity);
            return @object;
        }
    }
}