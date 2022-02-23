using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using static Source.Chess.Tests.Runtime.TestUtility;
using static Source.Chess.Runtime.ChessConstants;

namespace Source.Chess.Tests.Runtime {
    public class RookActionsTest {

        [Test]
        public void TestFreeMovement() {
            var white = "Rd4";
            var black = "Re5";
            
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("d4",
                    GetMoves(state.CurrentPlayer, PieceType.WRook, "d4", PieceType.Empty, 
                        "d1,d2,d3,d5,d6,d7,d8,a4,b4,c4,e4,f4,g4,h4")),
            };
            
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);
        }
    }
}