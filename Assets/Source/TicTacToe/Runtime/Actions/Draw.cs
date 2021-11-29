using Source.StrategyFramework.Runtime.Representation;
using Source.TicTacToe.Runtime.Objects;
using UnityEngine;

namespace Source.TicTacToe.Runtime.Actions {
    /// <summary>
    /// A specific player tries to draw their symbol in the given position.
    /// </summary>
    public class Draw : IAction {
        public Vector2Int Position;
        public Player Player;

        public Draw(Vector2Int position, Player player) {
            Position = position;
            Player = player;
        }
    }
}
