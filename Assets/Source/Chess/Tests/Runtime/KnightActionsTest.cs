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
                // TODO Wrong location for piece?
                // TODO: I can make a single method in which I list all the targets in a single string instead.
                new Tuple<string, List<IAction>>("b2", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3", PieceType.Empty, "b1"),
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3",PieceType.Empty,"a2"),
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3",PieceType.Empty,"a4"),
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3",PieceType.Empty,"b5"),
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3",PieceType.Empty,"d1"),
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3",PieceType.Empty,"e2"),
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3",PieceType.Empty,"e4"),
                    Add(state.CurrentPlayer, PieceType.WKnight, "c3",PieceType.Empty,"d5"),
                })
            };
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);
        }
    }
}