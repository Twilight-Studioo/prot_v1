#region

using Core.Utilities;
using Feature.Common;
using Feature.Common.Scene;
using VContainer;
using VContainer.Unity;

#endregion

namespace Main
{
    public class TestAController : IStartable
    {
        private readonly RootInstance rootInstance;

        [Inject]
        public TestAController(
            RootInstance rootInstance
        )
        {
            this.rootInstance = rootInstance;
        }

        public void Start()
        {
            var dataModel = rootInstance.GetCurrentDataModel<TestADataModel>();
            DebugEx.LogDetailed(dataModel.Score);
        }
    }
}