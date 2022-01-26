using System;
using System.Collections.Generic;
using Source.Chess.Runtime;
using Source.Chess.Runtime.Actions;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using UnityEngine.Assertions;
using static Source.Chess.Runtime.Rules;

namespace Source.Chess.Tests.Runtime {
    public static class TestUtility {
        public static Tuple<Vector2Int, PieceType> GetTuple(int x, int y, PieceType piece)
            => new Tuple<Vector2Int, PieceType>(new Vector2Int(x, y), piece);
        
        public static Tuple<Vector2Int, PieceType> GetTuple(char rank, char file, PieceType piece)
            => new Tuple<Vector2Int, PieceType>(Rules.ToVector2Int(rank, file), piece);
        
        public static void CompareActions(GameState state, LinearHistory history, List<Tuple<string, 
            List<IAction>>> tests) {
            foreach (var t in tests) {
                var rank = t.Item1[1];
                var file = t.Item1[0];
                var actions = GetActions(state, history, ToVector2Int(rank, file));
                AssertActionsEqualAndUnique(actions, t.Item2);
            }
        }

        public static void CompareAllActions(GameState state, LinearHistory history,
            List<Tuple<string, List<IAction>>> tests) {
            var expected = new List<IAction>();
            foreach (var tuple in tests) {
                expected.AddRange(tuple.Item2);
            }

            var allActions = GetAllActions(state, history);
            AssertActionsEqualAndUnique(allActions, expected);
        }

        
        public static void AssertActionsEqualAndUnique(List<IAction> actions, List<IAction> expected) {
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

        public static bool Equals(IAction a1, IAction a2) {
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