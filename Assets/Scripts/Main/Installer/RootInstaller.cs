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
        [SerializeField] private CharacterParams characterParams;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InputActionAccessor>();
            builder.Register<RootInstance>(Lifetime.Singleton);

            builder.Register<UserRepository>(Lifetime.Singleton);

            builder.RegisterComponent(characterParams);
        }
    }
}