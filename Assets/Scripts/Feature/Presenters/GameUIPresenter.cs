using Feature.Models;
using Feature.Views;
using VContainer;
using VContainer.Unity;

namespace Feature.Presenters
{
    public class GameUIPresenter
    {
        [Inject]
        public GameUIPresenter(
            PlayerModel playerModel,
            GameUIView gameUIView
        )
        {
            // playerModel.Health
            //     .Subscribe(x =>
            //     {
            //         gameUIView.SetHP(x);
            //         x;
            //     });
        }
    }
}