using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using Source.Chess.Runtime.Actions;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using static Source.Chess.Runtime.Rules;
using static Source.Chess.Tests.Runtime.TestUtility;

namespace Source.Chess.Tests.Runtime {
    public class PawnActionsTest {

        [Test]
        public void TestPawnStartMoveGeneration() {
            var white = "b2";
            var black = "g7";
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("b2", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "b2", PieceType.Empty, "b3"),
                    Add(state.CurrentPlayer, PieceType.WPawn, "b2",PieceType.Empty,"b4")
                })
            };
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);

            state.CurrentPlayer = state.Black;
            var blackTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("g7", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.BPawn, "g7", PieceType.Empty, "g6"),
                    Add(state.CurrentPlayer, PieceType.BPawn, "g7",PieceType.Empty,"g5")
                })
            };
            CompareActions(state, history, blackTests);
            CompareAllActions(state, history, blackTests);
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
                    Add(state.CurrentPlayer, PieceType.BPawn, "d4", PieceType.Empty, "d3"),
                })
            };
            CompareActions(state, history, blackTests);
            CompareAllActions(state, history, blackTests);
        }

        [Test]
        public void TestPawnAttack() {
            var white = "b3,c3,d3";
            var black = "a4,b4,c4";
            var state = new GameState(white, black);
            var history = new LinearHistory();

            state.CurrentPlayer = state.White;
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("b3", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "b3", PieceType.BPawn, "a4"),
                    Add(state.CurrentPlayer, PieceType.WPawn, "b3", PieceType.BPawn, "c4"),
                }),
                new Tuple<string, List<IAction>>("c3", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "c3", PieceType.BPawn, "b4"),
                }),
                new Tuple<string, List<IAction>>("d3", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.WPawn, "d3", PieceType.BPawn, "c4"),
                    Add(state.CurrentPlayer, PieceType.WPawn, "d3", PieceType.Empty, "d4"),
                }),
            };
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);
            
            state.CurrentPlayer = state.Black;
            var blackTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("a4", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.BPawn, "a4", PieceType.Empty, "a3"),
                    Add(state.CurrentPlayer, PieceType.BPawn, "a4", PieceType.WPawn, "b3"),
                }),
                new Tuple<string, List<IAction>>("b4", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.BPawn, "b4", PieceType.WPawn, "c3"),
                }),
                new Tuple<string, List<IAction>>("c4", new List<IAction>() {
                    Add(state.CurrentPlayer, PieceType.BPawn, "c4", PieceType.WPawn, "b3"),
                    Add(state.CurrentPlayer, PieceType.BPawn, "c4", PieceType.WPawn, "d3"),
                }),
            };
            CompareActions(state, history, blackTests);
            CompareAllActions(state, history, blackTests);
            
        }

        [Test]
        public void TestPawnPromotion() {
            var white = "c8";
            var black = "d1";
            var state = new GameState(white, black);
            var history = new LinearHistory();
            state.PromotionNeeded = true;

            state.CurrentPlayer = state.White;
            state.PromotionTarget = ToVector2Int('8', 'c');
            var whiteTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("c8", new List<IAction>() {
                    Add(state.CurrentPlayer, "c8", PieceType.WRook),
                    Add(state.CurrentPlayer, "c8", PieceType.WKnight),
                    Add(state.CurrentPlayer, "c8", PieceType.WBishop),
                    Add(state.CurrentPlayer, "c8", PieceType.WQueen),
                })
            };
            CompareActions(state, history, whiteTests);
            CompareAllActions(state, history, whiteTests);
            
            state.CurrentPlayer = state.Black;
            state.PromotionTarget = ToVector2Int('1', 'd');
            var blackTests = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>("d1", new List<IAction>() {
                    Add(state.CurrentPlayer, "d1", PieceType.BRook),
                    Add(state.CurrentPlayer, "d1", PieceType.BKnight),
                    Add(state.CurrentPlayer, "d1", PieceType.BBishop),
                    Add(state.CurrentPlayer, "d1", PieceType.BQueen),
                })
            };
            CompareActions(state, history, blackTests);
            CompareAllActions(state, history, blackTests);
        }

        [Test]
        public void TestPawnEnPassant() {
            SingleEnPassantTest("f5", "g7", false, "g5", "f6", "g6");
            SingleEnPassantTest("f5", "e7", false, "e5", "f6", "e6");
            SingleEnPassantTest("a2", "b4", true, "a4", "b3", "a3");
            SingleEnPassantTest("c2", "b4", true, "c4", "b3", "c3");
        }

        private void SingleEnPassantTest(string white, string black, bool whiteStarts, 
            string firstMove, string secondMove, string enPassant) {
            var state = new GameState(white, black);
            var history = new LinearHistory();

            var firstPlayer = whiteStarts ? state.White : state.Black;
            var secondPlayer = GetOtherPlayer(state, firstPlayer);
            state.CurrentPlayer = firstPlayer;
            
            var firstPiece = whiteStarts ? PieceType.WPawn : PieceType.BPawn;
            var secondPiece = GetOppositePiece(firstPiece);
            var s1 = whiteStarts ? white : black;
            var s2 = whiteStarts ? black : white;
            
            var move = new Move(firstPlayer, firstPiece, ToVector2Int(s1[1], s1[0]), PieceType.Empty,
                ToVector2Int(firstMove[1], firstMove[0]));
            Apply(state, history, move, true);

            var expected = new List<Tuple<string, List<IAction>>>() {
                new Tuple<string, List<IAction>>(s2, new List<IAction>() {
                    Add(secondPlayer, secondPiece, s2, PieceType.Empty, secondMove),
                    Add(secondPlayer, s2, enPassant)
                })
            };
            
            CompareActions(state, history, expected);
            CompareAllActions(state, history, expected);
        }
    }
}