#region

using Core.Utilities;
using Feature.Interface.Presenter;
using Feature.Model;
using Feature.Views;
using UniRx;
using UnityEngine;
using VContainer;

#endregion

namespace Feature.Presenter
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
                playerView.SetVelocity(new Vector2(vector.x, 0f) * 5f * playerModel.Speed);
            }
            else
            {
                playerView.SetVelocity(new Vector2(vector.x, 0f) * 2f * playerModel.Speed);
            }
        }

        public void OnJump()
        {
            if (!playerModel.StayGround.Value)
            {
                return;
            }

            playerView.AddForce(Vector2.up * 500f * playerModel.JumpPower);
            playerModel.SetStayGround(false);
        }

        public void Start()
        {
            playerView.Position
                .DistinctUntilChanged()
                .Subscribe(x => { playerModel.SetPosition(x); });
        }

        public Vector2 GetPosition() => playerModel.Position.Value;

        public void SetPosition(Vector2 position)
        {
            playerView.SetPosition(position);
        }

        public void Attack()
        {
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