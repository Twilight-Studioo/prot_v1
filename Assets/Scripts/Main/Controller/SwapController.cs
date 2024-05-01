#region

using System;
using Feature.Common.Parameter;
using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Presenter;
using UniRx;
using UnityEngine;
using VContainer;

#endregion

namespace Main.Controller
{
    public class SwapController : IDisposable
    {
        private readonly CharacterParams characterParams;
        private readonly GameState gameState;
        private readonly IPlayerPresenter playerPresenter;
        private readonly SwapItemsPresenter swapItemsPresenter;

        private readonly CompositeDisposable swapTimer;

        [Inject]
        public SwapController(
            GameState gameState,
            SwapItemsPresenter swapItemsPresenter,
            IPlayerPresenter playerPresenter,
            CharacterParams characterParams
        )
        {
            this.gameState = gameState;
            this.swapItemsPresenter = swapItemsPresenter;
            this.playerPresenter = playerPresenter;
            this.characterParams = characterParams;
            swapTimer = new();
        }

        public void Dispose()
        {
            swapTimer?.Dispose();
        }

        public void Start()
        {
            swapItemsPresenter.ResetSelector();
            gameState.CurrentState
                .DistinctUntilChanged()
                .Subscribe(_ =>
                {
                    if (gameState.CurrentState.Value is GameState.State.Swapped)
                    {
                        DoSwap();
                    }
                    else
                    {
                        EndSwap();
                        Observable
                            .Timer(TimeSpan.FromSeconds(characterParams.afterSwappedTimeSec * Time.timeScale))
                            .Subscribe(_ => { Time.timeScale = 1.0f; });
                    }
                });
        }

        public void SetSwap(bool isSwap)
        {
            if (!gameState.IsPlaying())
            {
                return;
            }

            if (isSwap)
            {
                if (gameState.CurrentState.Value is GameState.State.Playing)
                {
                    gameState.Swap();
                    Observable
                        .Timer(TimeSpan.FromSeconds(characterParams.swapTimeSec * Time.timeScale))
                        .Subscribe(_ =>
                        {
                            if (gameState.IsSwap())
                            {
                                gameState.EndSwap();
                            }
                        })
                        .AddTo(swapTimer);
                }
            }
            else
            {
                if (gameState.CurrentState.Value is GameState.State.Swapped)
                {
                    swapTimer?.Clear();
                    gameState.EndSwap();
                }
            }
        }

        public void Select(Vector2 direction)
        {
            swapItemsPresenter.MoveSelector(direction, playerPresenter.GetPosition());
        }

        private void DoSwap()
        {
            Time.timeScale = characterParams.inSwapTimeScale;
            swapItemsPresenter.ResetSelector();
        }

        private void EndSwap()
        {
            var item = swapItemsPresenter.SelectItem();
            if (item == null)
            {
                return;
            }

            var pos = playerPresenter.GetPosition();
            playerPresenter.SetPosition(item.transform.position);
            item.SetPosition(pos);
            item.SetHighlight(false);
        }
    }
}