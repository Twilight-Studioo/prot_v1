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
            playerModel.Position
                .DistinctUntilChanged()
                .Subscribe(pos => { gameUIView.SetCameraPosition(new(pos.x, pos.y + 2, -10)); }
                );
            gameUIView.SetHealthRange(0, param.health);
            playerModel.Health
                .DistinctUntilChanged()
                .Subscribe(x => { gameUIView.SetHealth(x); });
        }
    }
}