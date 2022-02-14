using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using static Source.Chess.Tests.Runtime.TestUtility;

namespace Source.Chess.Tests.Runtime {
    public class QueenActionsTest {

        [Test]
        public void FreeMovement() {
            var white = "Qb2";
            var black = "Qc7";
            
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("b2",
                    GetMoves(state.CurrentPlayer, PieceType.WQueen, "b2", PieceType.Empty, 
                        "a1,c3,d4,e5,f6,g7,h8,a3,c1,a2,c2,d2,e2,f2,g2,h2,b1,b3,b4,b5,b6,b7,b8")),
            };
            
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);
        }
    }
}