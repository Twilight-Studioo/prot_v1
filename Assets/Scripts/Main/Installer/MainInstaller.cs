#region

using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Model;
using Feature.Presenter;
using Feature.Views;
using Main.Controller;
using Main.Input;
using VContainer;
using VContainer.Unity;

#endregion

namespace Main.Installer
{
    public class MainInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameController>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<PlayerView>();
            builder.Register<PlayerModel>(Lifetime.Scoped);
            builder.Register<IPlayerPresenter, PlayerPresenter>(Lifetime.Scoped)
                .WithParameter("playerView", resolver => resolver.Resolve<PlayerView>())
                .WithParameter("playerModel", resolver => resolver.Resolve<PlayerModel>());
            builder.Register<GameState>(Lifetime.Scoped);
            builder.Register<GameInputController>(Lifetime.Scoped);
        }
    }
}