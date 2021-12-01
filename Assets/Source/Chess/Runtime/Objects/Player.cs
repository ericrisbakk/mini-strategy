using System.Collections.Generic;

namespace Source.Chess.Runtime.Objects {
    public class Player {
        public bool IsWhite { get; }
        public Dictionary<int, int> Captures { get; }

        public Player(bool isWhite) {
            IsWhite = isWhite;
            Captures = new Dictionary<int, int>();
        }
    }
}