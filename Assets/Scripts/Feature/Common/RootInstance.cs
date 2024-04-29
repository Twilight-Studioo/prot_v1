#region

using Feature.Interface;
using VContainer;

#endregion

namespace Feature.Common
{
    public class RootInstance
    {
        [Inject]
        public RootInstance()
        {
        }

        public ISceneDataModel CurrentDataModel { get; set; }

        public T GetCurrentDataModel<T>() where T : ISceneDataModel => (T)this.CurrentDataModel;
    }
}