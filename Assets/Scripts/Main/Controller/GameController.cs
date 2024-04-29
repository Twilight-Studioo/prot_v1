#region

using Feature.Common;
using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Repository;
using Main.Input;
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
        private readonly GameInputController gameInputController;
        private readonly GameState gameState;
        private readonly IPlayerPresenter playerPresenter;
        private readonly RootInstance rootInstance;
        private readonly UserRepository userRepository;

        [Inject]
        public GameController(
            UserRepository userRepository,
            IPlayerPresenter playerPresenter,
            RootInstance rootInstance,
            GameInputController gameInputController,
            GameState gameState
        )
        {
            this.userRepository = userRepository;
            this.rootInstance = rootInstance;
            this.gameInputController = gameInputController;
            this.gameState = gameState;
            this.playerPresenter = playerPresenter;
        }

        public void Start()
        {
            gameState.Initialize();
            // input action start
            gameInputController.Start();
            
            // 
            userRepository.Load();
            
            
            playerPresenter.Start();
            
            
            /* ここで開始処理 */
            gameState.Start();
            //
            // var dataModel = new TestADataModel();
            // dataModel.Score = 20;
            // SceneLoaderFeatures.TestA(dataModel).Bind(rootInstance).Load();
        }
    }
}