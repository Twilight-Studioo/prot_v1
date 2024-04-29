#region

using System;
using Core.Input;
using Core.Utilities;
using Feature.Common.State;
using Feature.Interface.Presenter;
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

        private InputActionEvent jumpAction;

        private InputActionEvent moveAction;

        [Inject]
        public GameInputController(
            InputActionAccessor inputActionAccessor,
            GameState gameState,
            IPlayerPresenter playerPresenter
        )
        {
            this.inputActionAccessor = inputActionAccessor;
            this.gameState = gameState;
            this.playerPresenter = playerPresenter;
        }

        public void Dispose()
        {
            jumpAction.Clear();

            moveAction.Clear();

            disposables.Dispose();
        }


        public void Start()
        {
            DebugEx.LogDetailed("GameInputProvider Start");
            EnableJump();
            EnableMove();
            // moveAction.Started += _ =>
            // {
            //     DebugEx.LogDetailed("Move Started");
            // };
            // moveAction.Performed += _ =>
            // {
            //     DebugEx.LogDetailed("Move Performed");
            // };
            // moveAction.Canceled += _ =>
            // {
            //     DebugEx.LogDetailed("Move Canceled");
            // };
        }

        private void EnableJump()
        {
            jumpAction = inputActionAccessor.CreateAction(Game.Jump);

            Observable.EveryUpdate()
                .Where(_ => IsJump())
                .Subscribe(_ =>
                {
                    if (gameState.GetState == GameState.State.Playing)
                    {
                        playerPresenter.OnJump();
                    }
                })
                .AddTo(disposables);
        }


        private void EnableMove()
        {
            moveAction = inputActionAccessor.CreateAction(Game.Move);

            // UniRxのEveryUpdateを使って入力を受け取る
            Observable.EveryUpdate()
                .Where(_ => CanMove())
                .Subscribe(_ =>
                {
                    playerPresenter.OnMove(moveAction.ReadValue<Vector2>());
                })
                .AddTo(disposables);
        }

        private bool IsJump() => jumpAction.ReadValue<float>() > 0;

        private bool CanMove() => gameState.IsPlaying();
    }
}