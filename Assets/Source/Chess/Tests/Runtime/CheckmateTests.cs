using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using static Source.Chess.Tests.Runtime.TestUtility;
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
    }
}