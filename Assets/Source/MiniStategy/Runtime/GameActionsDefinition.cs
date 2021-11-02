using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.MiniStategy.Runtime {
    /// <summary>
    /// Data definitions for all actions in the game. Actions are merely data objects, and have no game-related
    /// behaviour on their own.
    /// </summary>
    public class GameActionsDefinition
    {
        /// <summary>
        /// Move a single piece from A to B
        /// </summary>
        public class Move : IAction {
            public IPiece Target;
            public Vector2Int From;
            public Vector2Int To;

            public Move(IPiece target, Vector2Int from, Vector2Int to) {
                Target = target;
                From = from;
                To = to;
            }
        }
    }
}
