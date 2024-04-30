using Feature.Model;
using Feature.Views;
using UniRx;
using UnityEngine;
using VContainer;

namespace Feature.Presenter
{
    public class GameUIPresenter
    {
        private readonly GameUIView gameUIView;
        private readonly PlayerModel playerModel;
        [Inject]
        public GameUIPresenter(
            PlayerModel playerModel,
            GameUIView gameUIView
        )
        {
            this.playerModel = playerModel;
            this.gameUIView = gameUIView;
        }

        public void Start()
        {
            playerModel.Position
                .DistinctUntilChanged()
                .Subscribe(_ =>
                    {
                        var pos = playerModel.Position.Value;
                        gameUIView.SetCameraPosition(new(pos.x, pos.y + 2, -10));
                    }
                );
        }
    }
}