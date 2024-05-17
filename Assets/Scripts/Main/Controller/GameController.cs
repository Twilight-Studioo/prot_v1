#region

using Feature.Common;
using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Presenter;
using Feature.Repository;
using Main.Input;
using Main.Service;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

#endregion

namespace Main.Controller
{
    /// <summary>
    ///     Game Scene全体を管理するクラス
    /// </summary>
    public class GameController : IStartable
    {
        private readonly EnemySpawnService enemySpawnService;
        private readonly GameInputController gameInputController;
        private readonly GameState gameState;
        private readonly GameUIPresenter gameUIPresenter;
        private readonly IPlayerPresenter playerPresenter;
        private readonly RootInstance rootInstance;
        private readonly ISwapController swapController;
        private readonly SwapItemsPresenter swapItemsPresenter;
        private readonly UserRepository userRepository;

        [Inject]
        public GameController(
            UserRepository userRepository,
            IPlayerPresenter playerPresenter,
            RootInstance rootInstance,
            GameInputController gameInputController,
            GameState gameState,
            SwapItemsPresenter swapItemsPresenter,
            GameUIPresenter gameUIPresenter,
            ISwapController swapController,
            EnemySpawnService enemySpawnService
        )
        {
            this.userRepository = userRepository;
            this.rootInstance = rootInstance;
            this.gameInputController = gameInputController;
            this.gameState = gameState;
            this.playerPresenter = playerPresenter;
            this.swapItemsPresenter = swapItemsPresenter;
            this.gameUIPresenter = gameUIPresenter;
            this.swapController = swapController;
            this.enemySpawnService = enemySpawnService;
        }

        public void Start()
        {
            gameState.Initialize();
            gameState.CurrentState
                .DistinctUntilChanged()
                .Subscribe(x =>
                {
                    if (x is GameState.State.Playing)
                    {
                        Cursor.visible = false;
                    }
                    else if (x is GameState.State.Paused or GameState.State.Finished)
                    {
                        Cursor.visible = true;
                    }
                });
            // input action start
            gameInputController.Start();

            // 
            userRepository.Load();


            playerPresenter.Start();

            gameUIPresenter.Start();

            swapController.Start();
            enemySpawnService.Spawn(EnemySpawnService.EnemyType.Enemy1, playerPresenter.GetPosition());


            /* ここで開始処理 */
            gameState.Start();
            //
            // var dataModel = new TestADataModel();
            // dataModel.Score = 20;
            // SceneLoaderFeatures.TestA(dataModel).Bind(rootInstance).Load();
        }
    }
}