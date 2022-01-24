using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    public class EnPassant : IAction {
        public Player Player { get; }
        public Vector2Int Source { get; }
        public Vector2Int Target { get; }

        public Vector2Int Capture => new Vector2Int(Target.x - Rules.GetPawnDirection(Player.Color), Target.y);
        
        public EnPassant(Player player, Vector2Int source, Vector2Int target) {
            Player = player;
            Source = source;
            Target = target;
        }

        public bool Equals(EnPassant other) {
            return (Equals(Player, other.Player)
                    && Source == other.Source
                    && Target == other.Target);
        }
    }
}