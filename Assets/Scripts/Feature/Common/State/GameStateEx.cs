namespace Feature.Common.State
{
    public static class GameStateEx
    {
        // ゲーム画面内で操作可能かのフラグ(メニュー含む)
        public static bool IsPlaying(this GameState state) =>
            state.CurrentState.Value is GameState.State.Playing or GameState.State.Swapped;

        public static bool CanMove(this GameState state) =>
            state.CurrentState.Value is GameState.State.Playing or GameState.State.Swapped;

        public static bool CanAttack(this GameState state) =>
            state.CurrentState.Value is GameState.State.Playing;

        public static bool IsSwap(this GameState state) => state.CurrentState.Value is GameState.State.Swapped;

        public static bool CanSwap(this GameState state) =>
            state.CurrentState.Value is GameState.State.Playing or GameState.State.Swapped;
    }
}