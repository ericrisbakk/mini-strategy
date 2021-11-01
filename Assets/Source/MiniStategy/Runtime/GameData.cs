using System.Collections.Generic;

namespace Source.MiniStategy.Runtime {
    /// <summary>
    /// Contains game data definitions.
    /// </summary>
    public class GameData
    {
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

        public struct TileData {
            public TileType Tile;
            public Dictionary<int, IPiece> Pieces;
            public Dictionary<int, IEffect> Effects;
        }
    }
}
