using System;
using UnityEngine;

namespace Source.StrategyFramework.Runtime.Map {
    /// <summary>
    /// Keeps track of spatial relationship between Edges, Corners, and Faces of the grid.
    /// </summary>
    public class SquareGridMap {

        #region Constants

        public enum CornerDirection {
            NorthEast,
            SouthEast,
            SouthWest,
            NorthWest
        }

        public enum EdgeDirection {
            North,
            East,
            South,
            West
        }

        #endregion

        #region Variables

        public IData Data;
        
        private Position[,] _positions;

        #endregion
        
        public SquareGridMap(Vector2Int size) {
            _positions = new Position[size.x, size.y];
            for (var r = 0; r < size.x; ++r) {
                for (var c = 0; c < size.y; ++c) {
                    PositionType type;
                    if (r % 2 == 0)
                        type = c % 2 == 0 ? PositionType.Corner : PositionType.Edge;
                    else
                        type = c % 2 == 0 ? PositionType.Edge : PositionType.Face;

                    _positions[r, c] = new Position(type);
                }
            }
        }

        public Position GetFace(Vector2Int pos) {
            return _positions[2 * pos.x + 1, 2 * pos.y + 1];
        }
    
        public static Position GetCornerOfFace(Position face, CornerDirection corner) {
            throw new NotImplementedException();
        }
    
        public static Position GetEdgeOfFace(Position face, EdgeDirection edge) {
            throw new NotImplementedException();
        }
    }
}
