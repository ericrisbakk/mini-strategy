using Source.Chess.Runtime.Objects;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    public class Castle {
        public Player Player { get; }
        public Vector2Int King { get; }
        public Vector2Int Rook { get; }

        public Castle(Player player, Vector2Int king, Vector2Int rook) {
            Player = player;
            King = king;
            Rook = rook;
        }
    }
}