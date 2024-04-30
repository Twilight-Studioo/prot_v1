using UniRx;
using UnityEngine;

namespace Feature.Common.State
{
    public class GameState
    {
        // ステートは細かく分け、メソッドで状態を取得する
        public enum State
        {
            None,

            // 初期化処理
            Started,

            // in game
            Playing,
            
            // スワップ操作中
            Swapped,

            // pause中
            Paused,

            // 終了処理
            DoEnded,

            // 終了
            Finished,
        }
        
        
        public IReactiveProperty<State> CurrentState { get; } = new ReactiveProperty<State>(State.None);


        public void Initialize()
        {
            CurrentState.Value = State.Started;
        }

        public void Start()
        {
            CurrentState.Value = State.Playing;
        }

        public void Pause()
        {
            CurrentState.Value = State.Paused;
        }
        
        public void Swap()
        {
            CurrentState.Value = State.Swapped;
        }
        public void EndSwap()
        {
            CurrentState.Value = State.Playing;
        }
    }
}