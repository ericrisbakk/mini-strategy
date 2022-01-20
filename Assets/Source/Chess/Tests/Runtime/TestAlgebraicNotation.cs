using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Color = Source.Chess.Runtime.Color;

namespace Source.Chess.Tests.Runtime {
    public class TestAlgebraicNotation {
        [Test]
        public void TestPiecesOfStandardBoard() {
            var state = new GameState(Rules.StandardWhite, Rules.StandardBlack);
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
            var state = new GameState(Rules.StandardWhite, Rules.StandardBlack);
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
            var state = new GameState(Rules.StandardWhite, Rules.StandardBlack);
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

        private List<Tuple<Vector2Int, PieceType>> GetStandardExpected() {
            var whitePawnRow = Rules.GetPawnStartRow(Color.White);
            var blackPawnRow = Rules.GetPawnStartRow(Color.Black);
            var whiteBackRow = whitePawnRow - Rules.GetPawnDirection(Color.White);
            var blackBackRow = blackPawnRow - Rules.GetPawnDirection(Color.Black);
            
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
        
        private Tuple<Vector2Int, PieceType> GetTuple(int x, int y, PieceType piece)
            => new Tuple<Vector2Int, PieceType>(new Vector2Int(x, y), piece);
    }
}