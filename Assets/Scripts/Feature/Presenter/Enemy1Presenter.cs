#region

using System;
using Core.Utilities;
using Feature.Common.Parameter;
using Feature.Interface.Model;
using Feature.Interface.Presenter;
using Feature.Interface.View;
using Feature.Model;
using Feature.Views;
using UniRx;

#endregion

namespace Feature.Presenter
{
    public class Enemy1Presenter : IEnemyPresenter
    {
        private readonly IEnemyModel enemyModel;
        private readonly EnemyViewBase enemyView;
        private readonly PlayerModel playerModel;
        private readonly SwapItemsPresenter swapItemsPresenter;

        private readonly CompositeDisposable disposable;

        private readonly Enemy1Params param;

        public Enemy1Presenter(
            Enemy1Params param,
            EnemyViewBase enemyView,
            PlayerModel playerModel,
            SwapItemsPresenter swapItemsPresenter
        )
        {
            this.param = param;
            enemyModel = new EnemyModel(param);
            this.enemyView = enemyView;
            this.playerModel = playerModel;
            this.swapItemsPresenter = swapItemsPresenter;
            disposable = new();
        }


        public void Spawned()
        {
            enemyView.Spawned();
            enemyModel.IsDead
                .Select(x => x)
                .Subscribe(_ => { enemyView.Dead(); })
                .AddTo(disposable);
            enemyModel.Position = enemyView.Position.ToReactiveProperty();


            Observable
                .Interval(TimeSpan.FromSeconds(param.attackIntervalSec))
                .Subscribe(_ =>
                {
                    var item = enemyView.GetItemInstance();
                    if (item is not BulletSwapItemView bullet)
                    {
                        return;
                    }

                    var dir = (playerModel.Position.Value - enemyModel.Position.Value).normalized;
                    swapItemsPresenter.AddItems(item.ToList());
                    bullet.OnDestroy += BulletOnDead;

                    bullet.ThrowStart(enemyModel.Position.Value, dir, param.bulletSpeed, param.destroyDelay);
                    return;

                    void BulletOnDead()
                    {
                        bullet.OnDestroy -= BulletOnDead;
                        swapItemsPresenter.RemoveItems(item.ToList());
                        bullet.DestroyGameObject();
                    }
                })
                .AddTo(disposable);
        }

        public void Death()
        {
            enemyModel.Health.Value = 0;
            disposable.Clear();
        }

        public void Pause()
        {
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            enemyModel.Health.Value = (uint)(enemyModel.Health.Value - damage);
            enemyView.OnDamage();
        }
    }
}