#region

using Feature.Common;
using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Presenter;
using Feature.Repository;
using Main.Input;
using Main.Service;
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
        private readonly SwapController swapController;
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
            SwapController swapController,
            EnemySpawnService enemySpawnService
        )
        {
            this.userRepository = userRepository;
            this.gameInputController = gameInputController;
            this.gameState = gameState;
            this.playerPresenter = playerPresenter;
            this.gameUIPresenter = gameUIPresenter;
            this.swapController = swapController;
            this.enemySpawnService = enemySpawnService;
        }

        public void Start()
        {
            gameState.Initialize();
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