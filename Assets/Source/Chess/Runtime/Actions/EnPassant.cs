using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    public class EnPassant : IAction {
        public Player Player { get; }
        public Vector2Int Source { get; }
        public Vector2Int Target { get; }

        public EnPassant(Player player, Vector2Int source, Vector2Int target) {
            Player = player;
            Source = source;
            Target = target;
        }
    }
}