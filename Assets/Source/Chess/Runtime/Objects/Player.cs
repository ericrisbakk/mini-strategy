using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Objects {
    public class Player {
        public PlayerType Color { get; }
        public Dictionary<PieceType, int> Captures { get; }

        public Player(PlayerType color) {
            Assert.AreNotEqual(color, PlayerType.Unassigned, 
                "Player must be assigned a color.");
            Color = color;
            Captures = new Dictionary<PieceType, int>();
        }
    }
}