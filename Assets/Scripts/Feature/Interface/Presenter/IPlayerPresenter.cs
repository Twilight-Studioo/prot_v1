#region

using UnityEngine;

#endregion

namespace Feature.Interface.Presenter
{
    public interface IPlayerPresenter
    {
        /// <summary>
        ///     移動入力を受け取り、Viewに反映する
        /// </summary>
        /// <param name="vector">入力 (0.0~1.0)</param>
        public void OnMove(Vector2 vector);


        /// <summary>
        ///     Jumpを受け取り、Viewに(固定値)を反映する
        /// </summary>
        public void OnJump();

        public void Start();

        public Vector2 GetPosition();

        public void SetPosition(Vector2 position);

        public void AttackForward();
    }
}