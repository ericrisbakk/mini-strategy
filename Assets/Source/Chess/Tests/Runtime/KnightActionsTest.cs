using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using static Source.Chess.Tests.Runtime.TestUtility;

namespace Source.Chess.Tests.Runtime {
    public class KnightActionsTest {
        
        [Test]
        public void TestFreeMovement() {
            var white = "Nc3";
            var black = "Nf6";
            
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("c3",
                    GetMoves(state.CurrentPlayer, PieceType.WKnight, "c3", PieceType.Empty, 
                        "b1,a2,a4,b5,d1,e2,e4,d5")),
            };
            
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);
        }
    }
}