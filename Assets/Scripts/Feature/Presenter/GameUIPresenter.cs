#region

using Feature.Common.Parameter;
using Feature.Model;
using Feature.Views;
using UniRx;
using VContainer;

#endregion

namespace Feature.Presenter
{
    public class GameUIPresenter
    {
        private readonly GameUIView gameUIView;
        private readonly CharacterParams param;
        private readonly PlayerModel playerModel;

        [Inject]
        public GameUIPresenter(
            PlayerModel playerModel,
            GameUIView gameUIView,
            CharacterParams param
        )
        {
            this.playerModel = playerModel;
            this.gameUIView = gameUIView;
            this.param = param;
        }

        public void Start()
        {
            gameUIView.CameraPosition = playerModel.Position.DistinctUntilChanged().ToReactiveProperty();
            gameUIView.SetHealthRange(0, param.health);
            
            gameUIView.DoStart();
            playerModel.Health
                .DistinctUntilChanged()
                .Subscribe(x => { gameUIView.SetHealth(x); });
        }
    }
}