using Core.Utilities;
using Feature.Common.State;
using Feature.Interface.Presenter;
using Feature.Model;
using Feature.Views;
using UniRx;
using UnityEngine;
using VContainer;

namespace Feature.Presenter
{
    public class PlayerPresenterV2: IPlayerPresenter
    {
        private readonly PlayerModel playerModel;
        private readonly GameState gameState;
        private readonly PlayerView playerView;

        [Inject]
        public PlayerPresenterV2(
            PlayerView playerView,
            PlayerModel playerModel,
            GameState gameState
        )
        {
            this.playerView = playerView;
            this.playerModel = playerModel;
            this.gameState = gameState;
            this.playerView.OnHit += OnHit;
        }


        public void OnMove(Vector2 vector)
        {
            if (playerModel.StayGround.Value)
            {
                playerView.SetVelocity(new Vector2(vector.x, 0f) * SysEx.Unity.ToDeltaTime * 8f * playerModel.Speed * 3f);
            }
            else
            {
                playerView.SetVelocity(new Vector2(vector.x, 0f) * SysEx.Unity.ToDeltaTime * 4f * playerModel.Speed);
            }
        }

        public void OnJump()
        {
            if (!playerModel.StayGround.Value)
            {
                return;
            }
            gameState.Pause();
            playerView.AddForce(Vector2.up * SysEx.Unity.ToDeltaTime * 340f * playerModel.JumpPower * 10f);
            playerModel.SetStayGround(false);
        }

        public void Start()
        {
            playerView.Position
                .Where(p => p != playerModel.Position.Value)
                .Subscribe(x =>
                {
                    playerModel.SetPosition(x);
                });
        }

        private void OnHit(Collider2D collider)
        {
            if (collider.CompareTag("Ground"))
            {
                DebugEx.LogDetailed("Grounded");
                playerModel.SetStayGround(true);
            }
        }
    }
}