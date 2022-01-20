using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using UnityEngine;
using static Source.Chess.Tests.Runtime.TestUtility;

namespace Source.Chess.Tests.Runtime {
    public class TestPawnActions {
        [Test]
        public void TestPawnStartMove() {
            var white = "b2";
            var black = "g7";
            var state = new GameState(white, black);

            var expected = new List<Tuple<Vector2Int, PieceType>>() {
                
            };
        }
    }
}