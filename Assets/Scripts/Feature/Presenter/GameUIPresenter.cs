using Feature.Model;
using Feature.Views;
using VContainer;

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