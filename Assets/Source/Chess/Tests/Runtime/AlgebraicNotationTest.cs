using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using UnityEngine;
using static Source.Chess.Tests.Runtime.TestUtility;
using static Source.Chess.Runtime.ChessConstants;
using Assert = UnityEngine.Assertions.Assert;
using Color = Source.Chess.Runtime.ChessConstants.Color;
using Constants = Source.Chess.Runtime.ChessConstants;
using Checks = Source.Chess.Runtime.ChessChecks;

namespace Source.Chess.Tests.Runtime {
    public class AlgebraicNotationTest {
        int whitePawnRow = Checks.GetPawnStartRow(Color.White);
        int blackPawnRow = Checks.GetPawnStartRow(Color.Black);
        int whiteBackRow = Checks.GetPawnStartRow(Color.White) - Checks.GetPawnDirection(Color.White);
        int blackBackRow = Checks.GetPawnStartRow(Color.Black) - Checks.GetPawnDirection(Color.Black);
        
        [Test]
        public void TestPiecesOfStandardBoard() {
            var state = new GameState(Constants.StandardWhite, Constants.StandardBlack);
            var squares = state.Squares();
            var expected = GetStandardExpected();

            foreach (var tuple in expected) {
                var t = tuple.Item1;
                var p = tuple.Item2;
                
                Assert.IsTrue(squares[t.x, t.y] == p,
                    $"Expected piece {p} at location {t}.");
            }
        }
        
        [Test]
        public void TestOutOfBounds() {
            var state = new GameState(Constants.StandardWhite, Constants.StandardBlack);
            var squares = state.Squares();
            Assert.IsTrue(squares.GetLength(0) == 12);
            Assert.IsTrue(squares.GetLength(1) == 12);
            for (int i = 0; i < 12; i++) {
                for (int j = 0; j < 12; j++) {
                    if (i < 2
                    || i > 9
                    || j < 2
                    || j > 9)
                        Assert.IsTrue(squares[i, j] == PieceType.OutOfBounds, 
                            "squares[i, j] == PieceType.OutOfBounds");
                    else
                        Assert.IsTrue(squares[i, j] != PieceType.OutOfBounds,
                            "squares[i, j] != PieceType.OutOfBounds");
                }
            }
        }

        [Test]
        public void TestStandardEmptySquares() {
            var state = new GameState(Constants.StandardWhite, Constants.StandardBlack);
            var squares = state.Squares();
            for (int i = 4; i < 8; i++) {
                for (int j = 2; j < 9; j++) {
                    Assert.IsTrue(squares[i, j] == PieceType.Empty, 
                        "squares[i, j] == PieceType.Empty");
                }
            }
        }

        [Test]
        public void TestZeroPieces() {
            var state = new GameState("", "");
        }

        [Test]
        public void TestAlgebraicNotationToVector2Int() {

            var comparisons = new List<Tuple<string, Vector2Int>>() {
                new Tuple<string, Vector2Int>("a1", new Vector2Int(whiteBackRow, 2)),
                new Tuple<string, Vector2Int>("h1", new Vector2Int(whiteBackRow, 9)),
                new Tuple<string, Vector2Int>("a8", new Vector2Int(blackBackRow, 2)),
                new Tuple<string, Vector2Int>("h8", new Vector2Int(blackBackRow, 9)),
            };

            foreach (var comp in comparisons) {
                var rank = comp.Item1[1];
                var file = comp.Item1[0];
                var t = Rules.ToVector2Int(rank, file);
                Assert.IsTrue(t == comp.Item2,
                    $"Algebraic notation \"{comp.Item1}\" returned {t}, but should have been {comp.Item2}.");
            }
        }

        private List<Tuple<Vector2Int, PieceType>> GetStandardExpected() {
            var expected = new List<Tuple<Vector2Int, PieceType>>() {
                GetTuple(whiteBackRow, 2, PieceType.WRook),
                GetTuple(whiteBackRow, 3, PieceType.WKnight),
                GetTuple(whiteBackRow, 4, PieceType.WBishop),
                GetTuple(whiteBackRow, 5, PieceType.WQueen),
                GetTuple(whiteBackRow, 6, PieceType.WKing),
                GetTuple(whiteBackRow, 7, PieceType.WBishop),
                GetTuple(whiteBackRow, 8, PieceType.WKnight),
                GetTuple(whiteBackRow, 9, PieceType.WRook),
                
                GetTuple(blackBackRow, 2, PieceType.BRook),
                GetTuple(blackBackRow, 3, PieceType.BKnight),
                GetTuple(blackBackRow, 4, PieceType.BBishop),
                GetTuple(blackBackRow, 5, PieceType.BQueen),
                GetTuple(blackBackRow, 6, PieceType.BKing),
                GetTuple(blackBackRow, 7, PieceType.BBishop),
                GetTuple(blackBackRow, 8, PieceType.BKnight),
                GetTuple(blackBackRow, 9, PieceType.BRook),
            };
            expected.AddRange(GetPawns(whitePawnRow, true));
            expected.AddRange(GetPawns(blackPawnRow, false));

            return expected;
        }

        private List<Tuple<Vector2Int, PieceType>> GetPawns(int row, bool isWhite) {
            var l = new List<Tuple<Vector2Int, PieceType>>();
            for (int i = 0; i < 8; i++) {
                l.Add(GetTuple(row, 2 + i, isWhite? PieceType.WPawn : PieceType.BPawn));
            }

            return l;
        }
    }
}