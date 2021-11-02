using System.Collections.Generic;
using Source.StrategyFramework.Runtime;
using UnityEngine.Assertions;

namespace Source.MiniStategy.Runtime {
#region Constants

    public enum TileType {
        Empty,
        Field,
        Forest,
        Mountain,
        Swamp,
        Desert,
        City
    }

    #endregion

    public class GameTileData : IData {
        public TileType Tile;
        public Dictionary<int, Piece> Pieces;
        public Dictionary<int, IEffect> Effects;

        public void AddPiece(Piece piece) {
            Assert.IsTrue(!Pieces.ContainsKey(piece.ID));
            Pieces[piece.ID] = piece;
        }

        public void RemovePiece(Piece piece) {
            Assert.IsTrue(Pieces.ContainsKey(piece.ID));
            Pieces.Remove(piece.ID);
        }
    }
}
