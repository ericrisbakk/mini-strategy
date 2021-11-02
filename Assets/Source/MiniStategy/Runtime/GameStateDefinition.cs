using System.Collections.Generic;
using Source.StrategyFramework.Runtime.Map;
using Source.StrategyFramework.Runtime.Representation;

namespace Source.MiniStategy.Runtime {
    
    /// <summary>
    /// Data definition for the game state. The game state is merely a data object and has no game related behaviour
    /// on its own.
    /// </summary>
    public class GameStateDefinition {
        public SquareGridMap Map;
    }
}
