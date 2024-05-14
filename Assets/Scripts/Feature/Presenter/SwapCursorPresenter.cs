using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Feature.Common.Parameter;
using Feature.Interface.Presenter;
using Feature.Interface.View;
using Feature.Model;
using Feature.Views;
using UniRx;
using UnityEngine;
using VContainer;

namespace Feature.Presenter
{
    public class SwapCursorPresenter
    {
        private readonly SwapCursorModel model;
        private readonly SwapCursorView view;
        private readonly SwapCursorParams param;
        private readonly PlayerModel playerModel;
        private readonly IPlayerPresenter playerPresenter;

        [Inject]
        public SwapCursorPresenter(
            SwapCursorView cursorView,
            SwapCursorModel cursorModel,
            SwapCursorParams cursorParams,
            PlayerModel playerModel,
            IPlayerPresenter playerPresenter
        )
        {
            model = cursorModel;
            view = cursorView;
            param = cursorParams;
            this.playerModel = playerModel;
            this.playerPresenter = playerPresenter;
        }

        private IReadOnlyReactiveProperty<Vector2> playerPosition;

        private IReactiveProperty<Vector2> movePosition;
        private IReactiveProperty<Vector2> moveOnMousePosition;
        
        private List<SwapItemViewBase> beforeHighLights;

        public void Start()
        {
            beforeHighLights = new();
            playerPosition = playerModel.Position.ToReactiveProperty();
            movePosition = new ReactiveProperty<Vector2>(new());
            moveOnMousePosition = new ReactiveProperty<Vector2>(new());
            playerPosition
                .Subscribe(_ =>
                {
                    Moved(playerPosition.Value + movePosition.Value);
                })
                .AddTo(view);
            movePosition
                .DistinctUntilChanged()
                .Subscribe(_ =>
                {
                    Moved(playerPosition.Value + movePosition.Value);
                })
                .AddTo(view);
            moveOnMousePosition
                .DistinctUntilChanged()
                .Subscribe(_ =>
                {
                    Moved(moveOnMousePosition.Value);
                })
                .AddTo(view);
            model.Position = view.Position.ToReactiveProperty();
        }

        public void Select(Vector2 dir)
        {
            var diff = dir * Time.deltaTime * 4f * param.moveSpeed;
            var feat = (playerPosition.Value + movePosition.Value + diff).ToVector3();
            if (feat.IsInScreen())
            {
                movePosition.Value += diff;
            }
        }
        
        public void SelectOnMouse(Vector2 pos)
        {
            var feat = pos.ToVector3();
            if (feat.IsInScreen())
            {
                moveOnMousePosition.Value = pos;
            }
        }

        private void Moved(Vector2 pos)
        {
            view.SetPosition(pos);
            SetHighLight();
        }


        private void SetHighLight()
        {
            if (!beforeHighLights.Empty())
            {
                foreach (var swapItemViewBase in beforeHighLights)
                {
                    swapItemViewBase.SetHighlight(false);
                }
                beforeHighLights.Clear();
            }

            List<GameObject> hits = new();
            if (!RaycastEx.FindObjectWithPosition(model.Position.Value, 1f, ref hits))
            {
                return;
            }
            
            foreach (var item in hits.Select(gameObject => gameObject.GetComponent<SwapItemViewBase>()).Where(item => !item.IsNull()))
            {
                item.SetHighlight(true);
                beforeHighLights.Add(item);
            }
        }

        public void TrySwap()
        {
            List<GameObject> hits = new();
            if (!RaycastEx.FindObjectWithPosition(model.Position.Value, 1f, ref hits))
            {
                return;
            }

            foreach (var gameObject in hits)
            {
                var item = gameObject.GetComponent<SwapItemViewBase>();
                if (!item.IsNull())
                {
                    var pos = playerPresenter.GetPosition();
                    playerPresenter.SetPosition(item.transform.position);
                    item.SetPosition(pos);
                    item.SetHighlight(false);
                    break;
                }
            }
        }
    }
}