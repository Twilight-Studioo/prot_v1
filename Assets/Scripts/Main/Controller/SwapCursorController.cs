#region

using Feature.Common.Parameter;
using Feature.Interface.Presenter;
using Feature.Presenter;
using UnityEngine;
using VContainer;

#endregion

namespace Main.Controller
{
    public class SwapCursorController : ISwapController
    {
        private readonly SwapCursorParams param;
        private readonly SwapCursorPresenter presenter;

        [Inject]
        public SwapCursorController(
            SwapCursorPresenter presenter,
            SwapCursorParams param
        )
        {
            this.presenter = presenter;
            this.param = param;
        }

        public void Start()
        {
            presenter.Start();
        }

        public void SetSwap(bool isSwap)
        {
            if (isSwap)
            {
                presenter.TrySwap();
            }
        }

        public void Select(Vector2 dir, bool isMouse)
        {
            if (!isMouse)
            {
                presenter.Select(dir);
            }
            else
            {
                presenter.Select(dir * 6f * param.mouseAdSpeed);
                // var screenPoint = dir.ToVector3();
                // screenPoint.z = 10.0f;
                // var pos = Camera.main.ScreenToWorldPoint(screenPoint);
                // presenter.SelectOnMouse(pos);
            }
        }
    }
}