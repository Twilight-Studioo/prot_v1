#region

using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Model;
using Feature.Presenter;
using Feature.Views;
using Main.Controller;
using Main.Input;
using Main.Service;
using VContainer;
using VContainer.Unity;

#endregion

namespace Main.Installer
{
    public class MainInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<PlayerView>();
            builder.Register<PlayerModel>(Lifetime.Scoped);
            builder.Register<IPlayerPresenter, PlayerPresenter>(Lifetime.Scoped);

            // TODO: 初期処理の挙動次第でcontrollerからinitするかEntryPointでinitするか変える
            builder.Register<SwapItemsModel>(Lifetime.Scoped);
            builder.Register<SwapItemsPresenter>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<SwapCursorView>();
            builder.Register<SwapCursorModel>(Lifetime.Scoped);
            builder.Register<SwapCursorPresenter>(Lifetime.Scoped);
            builder.Register<ISwapController, SwapCursorController>(Lifetime.Scoped);

            builder.Register<GameState>(Lifetime.Scoped);
            builder.Register<GameInputController>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<GameUIView>();
            builder.Register<GameUIPresenter>(Lifetime.Scoped);
            builder.Register<EnemySpawnService>(Lifetime.Scoped);

            builder.RegisterEntryPoint<GameController>(Lifetime.Scoped);
        }
    }
}