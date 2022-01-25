using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using static Source.Chess.Runtime.Rules;
using Assert = UnityEngine.Assertions.Assert;

namespace Source.Chess.Tests.Runtime {
    public class PawnActionsTest {
        private Move Add(Player player, PieceType piece, string source, PieceType capture, string target)
            => new Move(player, piece, ToVector2Int(source[1], source[0]), capture, ToVector2Int(target[1], target[0]));

        [Test]
        public void TestPawnStartMoveGeneration() {
            var white = "b2";
            var black = "g7";
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            var expected = new List<IAction>() {
                Add(state.CurrentPlayer, PieceType.WPawn, "b2", PieceType.Empty, "b3"),
                Add(state.CurrentPlayer, PieceType.WPawn, "b2",PieceType.Empty,"b4")
            };
            var actions = GetActions(state, history, ToVector2Int('2', 'b'));
            AssertActionsEqualAndUnique(actions, expected);

            state.CurrentPlayer = state.Black;
            expected = new List<IAction>() {
                Add(state.CurrentPlayer, PieceType.WPawn, "g7", PieceType.Empty, "g6"),
                Add(state.CurrentPlayer, PieceType.WPawn, "g7",PieceType.Empty,"g5")
            };
            actions = GetActions(state, history, ToVector2Int('7', 'g'));
            AssertActionsEqualAndUnique(actions, expected);
        }

        [Test]
        public void TestPawnMoveBlocked() {
            var white = "b2,c4,d2,f2,f3,g2,g4";
            var black = "b3,c5,d4";
            var state = new GameState(white, black);
            var history = new LinearHistory();
            var empty = new List<IAction>() { };

            state.CurrentPlayer = state.White;
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("b2", empty),
                new Tuple<string, List<IAction>>("c4", empty),
                new Tuple<string, List<IAction>>("d2", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "d2", PieceType.Empty, "d3"),
                }),
                new Tuple<string, List<IAction>>("f2", empty),
                new Tuple<string, List<IAction>>("f3", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "f3", PieceType.Empty, "f4"),
                }),
                new Tuple<string, List<IAction>>("g2", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "g2", PieceType.Empty, "g3"),
                }),
                new Tuple<string, List<IAction>>("g4", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "g4", PieceType.Empty, "g5"),
                })
            };

            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);

            state.CurrentPlayer = state.Black;
            var blackTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("b3", empty),
                new Tuple<string, List<IAction>>("c5", empty),
                new Tuple<string, List<IAction>>("d4", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "d4", PieceType.Empty, "d3"),
                })
            };
            
            CompareActions(state, history, blackTests);
            CompareAllActions(state, history, blackTests);
        }

        private void CompareActions(GameState state, LinearHistory history, List<Tuple<string, List<IAction>>> tests) {
            foreach (var t in tests) {
                var rank = t.Item1[1];
                var file = t.Item1[0];
                var actions = GetActions(state, history, ToVector2Int(rank, file));
                AssertActionsEqualAndUnique(actions, t.Item2);
            }
        }

        private void CompareAllActions(GameState state, LinearHistory history,
            List<Tuple<string, List<IAction>>> tests) {
            var expected = new List<IAction>();
            foreach (var tuple in tests) {
                expected.AddRange(tuple.Item2);
            }

            var allActions = GetAllActions(state, history);
            AssertActionsEqualAndUnique(allActions, expected);
        }

        
        private void AssertActionsEqualAndUnique(List<IAction> actions, List<IAction> expected) {
            Assert.IsTrue(actions.Count == expected.Count,
                "Number of actions generated does not match number of expected actions.");
            
            int[] counts = new int[actions.Count];
            
            for (int i = 0; i < actions.Count; i++) {
                for (int j = 0; j < expected.Count; j++) {
                    if (Equals(actions[i], expected[j]))
                        counts[i] += 1;
                }
            }
            
            for (int i = 0; i < actions.Count; i++) {
                var c = counts[i];
                var a = actions[i];
                Assert.IsTrue(c == 1,
                    $"Generated action {i}: {a} had {c} matches, but there should only be one match.");
            }
        }

        private bool Equals(IAction a1, IAction a2) {
            if (a1.GetType() != a2.GetType())
                return false;
            if (a1 is Move m && m.Equals((Move) a2))
                return true;
            if (a1 is EnPassant ep && ep.Equals((EnPassant) a2))
                return true;
            if (a1 is Promote p && p.Equals((Promote) a2))
                return true;
            return false;
        }
    }
}