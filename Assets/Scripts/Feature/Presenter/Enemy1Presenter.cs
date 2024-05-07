using System;
using Feature.Common.Parameter;
using Feature.Interface.Model;
using Feature.Interface.Presenter;
using Feature.Interface.View;
using Feature.Model;
using Feature.Views;
using UniRx;
using UnityEngine;

namespace Feature.Presenter
{
    public class Enemy1Presenter: IEnemyPresenter
    {

        private readonly IEnemyModel enemyModel;
        private readonly EnemyViewBase enemyView;
        private readonly PlayerModel playerModel;

        private CompositeDisposable disposable;

        private Enemy1Params param;
        public Enemy1Presenter(
            Enemy1Params param,
            EnemyViewBase enemyView,
            PlayerModel playerModel
        )
        {
            this.param = param;
            enemyModel = new EnemyModel(param);
            this.enemyView = enemyView;
            this.playerModel = playerModel;
            disposable = new();
        }


        public void Spawned()
        {
            enemyView.Spawned();
            enemyModel.IsDead
                .Select(x => x)
                .Subscribe(_ => {
                    enemyView.Dead();
                })
                .AddTo(disposable);
            enemyModel.Position = enemyView.Position.ToReactiveProperty();
            Observable
                .Interval(TimeSpan.FromSeconds(param.attackIntervalSec))
                .Subscribe(_ =>
                {
                    var item = enemyView.GetItemInstance();
                    if (item is BulletSwapItemView bullet)
                    {
                        var dir = (playerModel.Position.Value - enemyModel.Position.Value).normalized;
                        bullet.ThrowStart(enemyModel.Position.Value, dir, param.bulletSpeed);
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
            throw new System.NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            enemyModel.Health.Value = (uint)(enemyModel.Health.Value - damage);
            enemyView.OnDamage();
        }
    }
}