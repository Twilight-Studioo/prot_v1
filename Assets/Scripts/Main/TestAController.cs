using Core.Utilities;
using Feature.Common.Scene;
using VContainer;
using VContainer.Unity;

namespace Main
{
    public class TestAController: IStartable
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
            TestADataModel dataModel = rootInstance.GetCurrentDataModel<TestADataModel>();
            DebugEx.LogDetailed(dataModel.Score);
        }
    }
}