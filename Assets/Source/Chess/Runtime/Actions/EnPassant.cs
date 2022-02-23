using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using Checks = Source.Chess.Runtime.ChessChecks;

namespace Source.Chess.Runtime.Actions {
    public class EnPassant : IAction {
        public Player Player { get; }
        public Vector2Int Source { get; }
        public Vector2Int Target { get; }

        public Vector2Int Capture => new Vector2Int(Target.x - Checks.GetPawnDirection(Player.Color), Target.y);
        
        public EnPassant(Player player, Vector2Int source, Vector2Int target) {
            Player = player;
            Source = source;
            Target = target;
        }
        
        /// <summary>
        /// `source` and `target` converted to `Vector2Int` using `Rules.ToVector2Int()`
        /// </summary>
        public EnPassant(Player player, string source, string target) {
            Player = player;
            Source = ChessRules.ToVector2Int(source[1], source[0]);
            Target = ChessRules.ToVector2Int(target[1], target[0]);
        }

        public bool Equals(EnPassant other) {
            return (Equals(Player, other.Player)
                    && Source == other.Source
                    && Target == other.Target);
        }
    }
}