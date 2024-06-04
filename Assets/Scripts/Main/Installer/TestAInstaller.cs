#region

using VContainer;
using VContainer.Unity;

#endregion

namespace Main.Installer
{
    public class TestAInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterEntryPoint<TestAController>(Lifetime.Scoped);
        }
    }
}