#region

using Interfaces;
using VContainer;

#endregion

namespace Main
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