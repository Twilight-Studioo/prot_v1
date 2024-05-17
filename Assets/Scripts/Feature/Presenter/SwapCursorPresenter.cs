#region

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

#endregion

namespace Feature.Presenter
{
    public class SwapCursorPresenter
    {
        private readonly SwapCursorModel model;
        private readonly SwapCursorParams param;
        private readonly PlayerModel playerModel;
        private readonly IPlayerPresenter playerPresenter;
        private readonly SwapCursorView view;

        private SwapItemViewBase beforeHighLight;
        private IReactiveProperty<Vector2> moveOnMousePosition;

        private IReactiveProperty<Vector2> movePosition;

        private IReadOnlyReactiveProperty<Vector2> playerPosition;

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

        public void Start()
        {
            playerPosition = playerModel.Position.ToReactiveProperty();
            movePosition = new ReactiveProperty<Vector2>(new());
            moveOnMousePosition = new ReactiveProperty<Vector2>(new());
            playerPosition
                .Subscribe(_ => { Moved(playerPosition.Value + movePosition.Value); })
                .AddTo(view);
            movePosition
                .DistinctUntilChanged()
                .Subscribe(_ => { Moved(playerPosition.Value + movePosition.Value); })
                .AddTo(view);
            moveOnMousePosition
                .DistinctUntilChanged()
                .Subscribe(_ => { Moved(moveOnMousePosition.Value); })
                .AddTo(view);
            model.Position = view.Position.ToReactiveProperty();
        }

        public void Select(Vector2 dir)
        {
            if (dir == Vector2.zero)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            var moveSpeed = param.moveSpeed;
            var assistPower = param.assistPower;
            var assistThreshold = 60f;

            var diff = dir * deltaTime * 4f * moveSpeed;
            var feat = (playerPosition.Value + movePosition.Value + diff).ToVector3();
            if (feat.IsInScreen())
            {
                movePosition.Value += diff;

                // direction assist
                List<GameObject> hits = new();
                var position = playerPosition.Value + movePosition.Value;
                if (RaycastEx.FindObjectWithPosition(position, param.assistedDistance, ref hits))
                {
                    hits.Sort((x, y) =>
                    {
                        var xDistance = x.transform.position.ToVector2() - position;
                        var yDistance = y.transform.position.ToVector2() - position;
                        return (int)((xDistance.magnitude - yDistance.magnitude) * 1000);
                    });

                    var nearestItem = hits
                        .Select(gameObject => gameObject.GetComponent<SwapItemViewBase>())
                        .FirstOrNull();
                    if (nearestItem.IsNull() || nearestItem == null)
                    {
                        return;
                    }

                    var itemDirection = nearestItem.transform.position.ToVector2() - position;
                    var itemAngle = Vector2.Angle(dir, itemDirection);

                    if (itemAngle <= assistThreshold)
                    {
                        var nudgeDirection = itemDirection.normalized * deltaTime *
                                             (12f / (itemDirection.magnitude + 1)) * assistPower;
                        movePosition.Value += nudgeDirection;
                    }

                    if (itemAngle >= 180f - assistThreshold)
                    {
                        movePosition.Value -= diff / (2.5f * (itemDirection.magnitude + 1));
                    }
                }
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
            if (!beforeHighLight.IsNull())
            {
                beforeHighLight.SetHighlight(false);
            }

            List<GameObject> hits = new();
            if (!RaycastEx.FindObjectWithPosition(model.Position.Value, 1f, ref hits))
            {
                return;
            }

            hits.Sort((x, y) =>
            {
                var xDistance = x.transform.position.ToVector2() - model.Position.Value;
                var yDistance = y.transform.position.ToVector2() - model.Position.Value;
                return (int)((xDistance.magnitude - yDistance.magnitude) * 1000);
            });

            var item = hits
                .Select(gameObject => gameObject.GetComponent<SwapItemViewBase>())
                .FirstOrNull(item => !item.IsNull());
            if (item.IsNull())
            {
                return;
            }

            item?.SetHighlight(true);
            beforeHighLight = item;
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
                if (item.IsNull())
                {
                    continue;
                }

                var pos = playerPresenter.GetPosition();
                playerPresenter.SetPosition(item.transform.position);
                item.SetPosition(pos);
                item.SetHighlight(false);
                break;
            }
        }
    }
}