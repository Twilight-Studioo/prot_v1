#region

using Core.Utilities;
using Feature.Models;
using Feature.Views;
using Interfaces.Presenters;
using UniRx;
using UnityEngine;
using VContainer;

#endregion

namespace Feature.Presenters
{
    /// <summary>
    ///     外部からの入力を受け取り、Modelに通知、Viewに反映する
    /// </summary>
    public class PlayerPresenter : IPlayerPresenter
    {
        private readonly PlayerModel playerModel;
        private readonly PlayerView playerView;

        [Inject]
        public PlayerPresenter(
            PlayerView playerView,
            PlayerModel playerModel
        )
        {
            this.playerView = playerView;
            this.playerModel = playerModel;
            this.playerView.OnHit += OnHit;
        }


        public void OnMove(Vector2 vector)
        {
            if (playerModel.StayGround.Value)
            {
                playerView.SetVelocity(new Vector2(vector.x, 0f) * SysEx.Unity.ToDeltaTime * 8f * playerModel.Speed);
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

            playerView.AddForce(Vector2.up * SysEx.Unity.ToDeltaTime * 340f * playerModel.JumpPower);
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