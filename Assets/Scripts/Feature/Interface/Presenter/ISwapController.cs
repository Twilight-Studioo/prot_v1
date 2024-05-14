using UnityEngine;

namespace Feature.Interface.Presenter
{
    public interface ISwapController
    {
        public void Start();

        public void SetSwap(bool isSwap);

        public void Select(Vector2 dir, bool isMouse);
    }
}