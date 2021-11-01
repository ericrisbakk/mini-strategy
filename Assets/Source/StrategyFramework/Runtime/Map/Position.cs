namespace Source.StrategyFramework.Runtime.Map {
    
    public enum PositionType {
        Face,
        Corner,
        Edge,
    }
    
    /// <summary>
    /// Data container for a specific position on the map.
    /// </summary>
    public class Position {
        public PositionType PositionType;
        public IData Data;
        #region Constructors

        public Position(PositionType type) {
            PositionType = type;
        }

        #endregion
    }
}