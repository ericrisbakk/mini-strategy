using System.Collections.Generic;
using UnityEngine.Assertions;
using static Source.Chess.Runtime.ChessConstants;

namespace Source.Chess.Runtime.Objects {
    public class Player {
        public Color Color { get; }
        public Dictionary<PieceType, int> Captures { get; }

        public Player(Color color) {
            Assert.AreNotEqual(color, Color.Unassigned, 
                "Player must be assigned a color.");
            Color = color;
            Captures = new Dictionary<PieceType, int>();
        }
    }
}