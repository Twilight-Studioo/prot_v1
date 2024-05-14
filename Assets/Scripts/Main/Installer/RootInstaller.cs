#region

using Core.Input;
using Feature.Common;
using Feature.Common.Parameter;
using Feature.Repository;
using UnityEngine;
using VContainer;
using VContainer.Unity;

#endregion

namespace Main.Installer
{
    public class RootInstaller : LifetimeScope
    {
        [SerializeField] private SwapCursorParams swapCursorParams;
        [SerializeField] private CharacterParams characterParams;
        [SerializeField] private Enemy1Params enemy1Params;
        [SerializeField] private Enemy2Params enemy2Params;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InputActionAccessor>();
            builder.Register<RootInstance>(Lifetime.Singleton);

            builder.Register<UserRepository>(Lifetime.Singleton);

            builder.RegisterComponent(characterParams);
            builder.RegisterComponent(enemy1Params);
            builder.RegisterComponent(enemy2Params);
            builder.RegisterComponent(swapCursorParams);
        }
    }
}