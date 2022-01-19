using System;
using System.Collections.Generic;
using NUnit.Framework;
using Source.Chess.Runtime;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Source.Chess.Tests.Runtime {
    public class TestAlgebraicNotation {
        [Test]
        public void VerifyPiecesOfStandardBoard() {
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
        public void VerifyOutOfBounds() {
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
        public void VerifyStandardEmptySquares() {
            var state = new GameState(Rules.StandardWhite, Rules.StandardBlack);
            var squares = state.Squares();
            for (int i = 4; i < 8; i++) {
                for (int j = 2; j < 9; j++) {
                    Assert.IsTrue(squares[i, j] == PieceType.Empty, 
                        "squares[i, j] == PieceType.Empty");
                }
            }
        }

        private List<Tuple<Vector2Int, PieceType>> GetStandardExpected() {
            var expected = new List<Tuple<Vector2Int, PieceType>>() {
                GetTuple(9, 2, PieceType.WRook),
                GetTuple(9, 3, PieceType.WKnight),
                GetTuple(9, 4, PieceType.WBishop),
                GetTuple(9, 5, PieceType.WQueen),
                GetTuple(9, 6, PieceType.WKing),
                GetTuple(9, 7, PieceType.WBishop),
                GetTuple(9, 8, PieceType.WKnight),
                GetTuple(9, 9, PieceType.WRook),
                GetTuple(2, 2, PieceType.BRook),
                GetTuple(2, 3, PieceType.BKnight),
                GetTuple(2, 4, PieceType.BBishop),
                GetTuple(2, 5, PieceType.BQueen),
                GetTuple(2, 6, PieceType.BKing),
                GetTuple(2, 7, PieceType.BBishop),
                GetTuple(2, 8, PieceType.BKnight),
                GetTuple(2, 9, PieceType.BRook),
            };
            expected.AddRange(GetPawns(8, true));
            expected.AddRange(GetPawns(3, false));

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