using NUnit.Framework;
using Source.Chess.Runtime;
using Source.StrategyFramework.Runtime.History;
using Color = Source.Chess.Runtime.ChessConstants.Color;
using Assert = UnityEngine.Assertions.Assert;

namespace Source.Chess.Tests.Runtime {
    public class CheckmateTests {

        [Test]
        public void TestSimpleCheck() {
            var white = "Rc2,Ke1";
            var black = "Kc6";
            
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            Assert.IsFalse(Rules.Check(state, Color.White),
                "The white king should not be in check.");
            Assert.IsTrue(Rules.Check(state, Color.Black),
                "The black king should be in check.");
        }

        [Test]
        public void TestSimpleCheckMate() {
            var white = "Kh1";
            var black = "Kh3,Bf3,Be3";
            
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            Assert.IsTrue(Rules.CheckMate(state, history, true),
                "The white king should be in checkmate.");

            state.CurrentPlayer = state.Black;
            Assert.IsFalse(Rules.CheckMate(state, history, true),
                "The black king should not be in checkmate.");
        }
    }
}