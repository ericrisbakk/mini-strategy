using System;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime.Behaviours {
    public class ViewControllerBehaviour : MonoBehaviour {

        #region Variables

        public BoardBehaviour board;

        [NonSerialized] public GameState State;
        [NonSerialized] public LinearHistory History;
        
        #endregion

        public void ApplyAction(IAction action) {
            
        }
    }
}
