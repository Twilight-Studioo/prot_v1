#region

using System;
using System.Collections.Generic;
using Core.Utilities;
using Feature.Common.Parameter;
using Feature.Interface.Presenter;
using Feature.Model;
using Feature.Presenter;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

#endregion

namespace Main.Service
{
    public class EnemySpawnService
    {
        public enum EnemyType
        {
            Enemy1,
            Enemy2,
        }

        private readonly Enemy1Params enemy1Params;
        private readonly Enemy2Params enemy2Params;

        private readonly List<IEnemyPresenter> enemies;
        private readonly PlayerModel playerModel;
        private readonly SwapItemsPresenter swapItemsPresenter;

        [Inject]
        public EnemySpawnService(
            Enemy1Params enemy1Params,
            Enemy2Params enemy2Params,
            PlayerModel playerModel,
            SwapItemsPresenter swapItemsPresenter
        )
        {
            this.enemy1Params = enemy1Params;
            this.enemy2Params = enemy2Params;
            this.playerModel = playerModel;
            this.swapItemsPresenter = swapItemsPresenter;
            enemies = new();
        }

        public void Spawn(
            EnemyType type,
            Vector3 position
        )
        {
            switch (type)
            {
                case EnemyType.Enemy1:
                    SpawnEnemy1(position);
                    break;
                case EnemyType.Enemy2:
                    // You can implement different behavior for Enemy2
                    break;
                default:
                    throw new ArgumentException($"Unsupported enemy type: {type}");
            }
        }

        private void SpawnEnemy1(Vector3 position)
        {
            if (enemy1Params.prefab.IsNull())
            {
                throw new NotImplementedException($"Not Attached {enemy1Params}");
            }

            if (enemy1Params.View().IsNull())
            {
                return;
            }

            var enemy = Object.Instantiate(enemy1Params.View(), position, Quaternion.identity);
            if (enemy.IsNull())
            {
                return;
            }

            var presenter = new Enemy1Presenter(enemy1Params, enemy, playerModel, swapItemsPresenter);
            presenter.OnDead += () =>
            {
                enemies.Remove(presenter);
                Spawn(EnemyType.Enemy1, playerModel.Position.Value);
            };
            enemies.Add(presenter);
            presenter.Spawned();
        }
    }
}