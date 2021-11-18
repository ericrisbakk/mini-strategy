using System.Collections.Generic;
using NUnit.Framework;
using Source.TicTacToe.Runtime;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Source.TicTacToe.Tests.Runtime {
    public class TestTicTacToeGameOver
    {
        [Test]
        public void TestDraw() {
            var state = new GameState();
            var positions = new List<Vector2Int> {
                new Vector2Int(1, 1),
                new Vector2Int(0, 0),
                new Vector2Int(2, 0),
                new Vector2Int(0, 2),
                new Vector2Int(0, 1),
                new Vector2Int(2, 1),
                new Vector2Int(1, 0),
                new Vector2Int(1, 2),
                new Vector2Int(2, 2),
            };
            var lastMove = positions[positions.Count - 1];
            
            foreach (var pos in positions) {
                ActionsDefinition.Draw(state, pos);
                if (state.MoveCounter < 9) {
                    Assert.IsTrue(RulesDefinition.IsGameOver(state, pos) == GameResult.Undecided);
                }
            }
            
            Assert.IsTrue(RulesDefinition.IsGameOver(state, lastMove) == GameResult.Draw);
        }
    }
}
