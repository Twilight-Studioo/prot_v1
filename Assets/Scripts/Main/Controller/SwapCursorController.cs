#region

using Core.Utilities;
using Feature.Interface.Presenter;
using Feature.Model;
using Feature.Presenter;
using UnityEngine;
using VContainer;

#endregion

namespace Main.Controller
{
    public class SwapCursorController : ISwapController
    {
        private readonly PlayerModel playerModel;
        private readonly SwapCursorPresenter presenter;

        [Inject]
        public SwapCursorController(
            SwapCursorPresenter presenter,
            PlayerModel playerModel
        )
        {
            this.presenter = presenter;
            this.playerModel = playerModel;
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
                var screenPoint = dir.ToVector3();
                screenPoint.z = 10.0f;
                var pos = Camera.main.ScreenToWorldPoint(screenPoint);
                presenter.SelectOnMouse(pos);
            }
        }
    }
}