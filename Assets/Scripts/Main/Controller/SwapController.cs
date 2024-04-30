using System;
using Core.Utilities;
using Feature.Common.State;
using UniRx;
using UnityEngine;
using VContainer;

namespace Main.Controller
{
    public class SwapController: IDisposable
    {
        private readonly GameState gameState;
        private IDisposable swap;

        [Inject]
        public SwapController(
            GameState gameState
        )
        {
            this.gameState = gameState;
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
                        .Timer(TimeSpan.FromSeconds(3))
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
            
        }

        public void Dispose()
        {
            swap?.Dispose();
        }

        public void Start()
        {
        }
    }
}