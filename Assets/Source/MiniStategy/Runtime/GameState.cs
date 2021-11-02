using System.Collections.Generic;
using Source.StrategyFramework.Runtime.Map;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.MiniStategy.Runtime {
    
    /// <summary>
    /// The game state. The game state is merely a data object and has no game related behaviour
    /// on its own.
    /// </summary>
    public class GameState {
        #region Variables

        public SquareGridMap Map;

        #endregion
    }
}
