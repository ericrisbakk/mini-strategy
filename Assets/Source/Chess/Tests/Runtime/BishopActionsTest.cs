using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using static Source.Chess.Tests.Runtime.TestUtility;
using static Source.Chess.Runtime.ChessConstants;

namespace Source.Chess.Tests.Runtime {
    public class BishopActionsTest {

        [Test]
        public void TestFreeMovement() {
            var white = "Bb2";
            var black = "Bb7";
            
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("b2",
                    GetMoves(state.CurrentPlayer, PieceType.WBishop, "b2", PieceType.Empty, 
                        "a1,c3,d4,e5,f6,g7,h8,a3,c1")),
            };
            
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);
        }
    }
}