#region

using System;
using Core.Input;
using Core.Utilities;
using Feature.Common.State;
using Feature.Interface.Presenter;
using Main.Controller;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

#endregion

namespace Main.Input
{
    /// <summary>
    ///     UniRxを使ってUpdateから入力を受け取るクラス
    ///     Stateを見て、それぞれのPresenterに通知する
    /// </summary>
    public class GameInputController : IStartable, IDisposable
    {
        private readonly CompositeDisposable disposables = new();
        private readonly GameState gameState;
        private readonly InputActionAccessor inputActionAccessor;
        private readonly IPlayerPresenter playerPresenter;
        private readonly ISwapController swapController;

        private InputActionEvent attackAction;

        private InputActionEvent jumpAction;

        private InputActionEvent moveAction;

        private InputActionEvent swapAction;

        private InputActionEvent swapMoveAction;
        private InputActionEvent swapMoveOnMouseAction;

        [Inject]
        public GameInputController(
            InputActionAccessor inputActionAccessor,
            GameState gameState,
            IPlayerPresenter playerPresenter,
            ISwapController swapController
        )
        {
            this.inputActionAccessor = inputActionAccessor;
            this.gameState = gameState;
            this.playerPresenter = playerPresenter;
            this.swapController = swapController;
        }

        public void Dispose()
        {
            jumpAction.Clear();

            moveAction.Clear();

            swapAction.Clear();

            swapMoveAction.Clear();

            disposables.Dispose();
        }


        public void Start()
        {
            DebugEx.LogDetailed("GameInputProvider Start");
            EnableJump();
            EnableMove();
            EnableSwap();
            EnableAttack();
        }

        private void EnableAttack()
        {
            attackAction = inputActionAccessor.CreateAction(Game.Attack);

            Observable.EveryUpdate()
                .Where(_ => gameState.CanAttack())
                .Select(_ => attackAction.ReadValue<float>() > 0f)
                .DistinctUntilChanged()
                .Subscribe(x =>
                {
                    if (x)
                    {
                        playerPresenter.AttackForward();
                    }
                });
        }

        private void EnableJump()
        {
            jumpAction = inputActionAccessor.CreateAction(Game.Jump);

            Observable.EveryUpdate()
                .Where(_ => IsJump() && gameState.CanMove())
                .Subscribe(_ => { playerPresenter.OnJump(); })
                .AddTo(disposables);
        }


        private void EnableMove()
        {
            moveAction = inputActionAccessor.CreateAction(Game.Move);

            // UniRxのEveryUpdateを使って入力を受け取る
            Observable.EveryUpdate()
                .Where(_ => gameState.CanMove())
                .Subscribe(_ => { playerPresenter.OnMove(moveAction.ReadValue<Vector2>()); })
                .AddTo(disposables);
        }

        private void EnableSwap()
        {
            swapAction = inputActionAccessor.CreateAction(Game.DoSwap);

            Observable.EveryUpdate()
                .Where(_ => gameState.CanSwap())
                .Select(_ => swapAction.ReadValue<float>())
                .DistinctUntilChanged()
                .Subscribe(x => { swapController.SetSwap(x > 0f); })
                .AddTo(disposables);

            swapMoveAction = inputActionAccessor.CreateAction(Game.SwapMove);

            Observable.EveryUpdate()
                .Select(_ => swapMoveAction.ReadValue<Vector2>())
                .Subscribe(x => { swapController.Select(x, false); })
                .AddTo(disposables);

            swapMoveOnMouseAction = inputActionAccessor.CreateAction(Game.SwapMoveOnMouse);
            
            Observable
                .EveryUpdate()
                .Select(_ => swapMoveOnMouseAction.ReadValue<Vector2>())
                .Subscribe(x => { swapController.Select(x, true); })
                .AddTo(disposables);
        }

        private bool Swap() => swapAction.ReadValue<float>() > 0;

        private bool IsJump() => jumpAction.ReadValue<float>() > 0;
    }
}