using System;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime.Behaviours {
    public class GameBehaviour : MonoBehaviour {
        [NonSerialized] public GameState State;
        [NonSerialized] public LinearHistory History;

        public void ApplyAction(IAction action) {
            
        }
    }
}
