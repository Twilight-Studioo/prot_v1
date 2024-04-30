using System;
using Feature.Common.Parameter;
using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Presenter;
using UniRx;
using UnityEngine;
using VContainer;

namespace Main.Controller
{
    public class SwapController: IDisposable
    {
        private readonly GameState gameState;
        private readonly SwapItemsPresenter swapItemsPresenter;
        private readonly IPlayerPresenter playerPresenter;
        private readonly CharacterParams characterParams;
        
        private IDisposable swap;

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
                        Time.timeScale = characterParams.InSwapTimeScale;
                    }
                    else
                    {
                        EndSwap();
                        Observable
                            .Timer(TimeSpan.FromSeconds(characterParams.AfterSwappedTimeSec))
                            .Subscribe(_ =>
                            {
                                Time.timeScale = 1.0f;
                            });
                    }
                });
        }

        public void SetSwap(bool isSwap)
        {
            if (!gameState.IsPlaying()) return;
            if (isSwap)
            {
                if (gameState.CurrentState.Value is GameState.State.Playing)
                {
                    gameState.Swap();
                    swap = Observable
                        .Timer(TimeSpan.FromSeconds(characterParams.SwapTimeSec))
                        .Subscribe(_ =>
                        {
                            if (gameState.IsSwap())
                            {
                                gameState.EndSwap();
                            }
                        });
                }
            }
            else
            {
                if (gameState.CurrentState.Value is GameState.State.Swapped)
                {
                    swap?.Dispose();
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
            swapItemsPresenter.ResetSelector();
        }
        
        private void EndSwap()
        {
            var item = swapItemsPresenter.SelectItem();
            if (item == null) return;
            playerPresenter.SetPosition(item.transform.position);
            item.SetHighlight(false);
        }

        public void Dispose()
        {
            swap?.Dispose();
        }
    }
}