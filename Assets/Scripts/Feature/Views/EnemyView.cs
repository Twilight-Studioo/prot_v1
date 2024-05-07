using System;
using Core.Utilities;
using Feature.Interface.View;
using UnityEngine;

namespace Feature.Views
{
    public class EnemyView: EnemyViewBase
    {
        [SerializeField] private GameObject bullet;
        public override void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void Awake()
        {
            if (bullet.IsNull())
            {
                throw new NotImplementedException();
            }
        }

        public override void Dead()
        {
            Destroy(this);
        }

        public override void Spawned()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public override SwapItemViewBase GetItemInstance()
        {
            var @object = Instantiate(bullet.GetComponent<BulletSwapItemView>(), Position.Value, Quaternion.identity);
            return @object;
        }
    }
}