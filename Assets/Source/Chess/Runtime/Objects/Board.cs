using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Objects {
    
    /// <summary>
    /// Pieces are represented using integer values.
    /// The board is actually 12x12, in which the borders are assigned an "OutOfBounds" value.
    /// </summary>
    public class Board {
        public PieceType[,] Squares { get; }

        public Board(string white, string black) {
            Squares = new PieceType[12, 12];
            SetupSquares();
            if (white.Length > 0) PlacePieces(white, true);
            if (black.Length > 0) PlacePieces(black, false);
        }

        private void PlacePieces(string line, bool isWhite) {
            var placements = line.Split(',');
            foreach (var placement in placements) {
                Assert.IsTrue(2 <= placement.Length && placement.Length <= 3);

                bool notPawn = placement.Length == 3;
                int pad = notPawn ? 1 : 0;
                var t = ToVector2Int(placement[1 + pad], placement[pad]);
                var pieceType = notPawn ? ToPieceType(placement[0], isWhite) : (isWhite ? PieceType.WPawn : PieceType.BPawn);
                Squares[t.x, t.y] = pieceType;
            }
        }
        
        private PieceType ToPieceType(char piece, bool isWhite) {
            switch (piece) {
                case 'K': return isWhite ? PieceType.WKing : PieceType.BKing;
                case 'Q': return isWhite ? PieceType.WQueen : PieceType.BQueen;
                case 'R': return isWhite ? PieceType.WRook : PieceType.BRook;
                case 'B': return isWhite ? PieceType.WBishop : PieceType.BBishop;
                case 'N': return isWhite ? PieceType.WKnight : PieceType.BKnight;
            }

            throw new Exception($"Algebraic notation \'{piece}\' was not recognized as a `PieceType`.");
        }

        private Vector2Int ToVector2Int(char rank, char file) {
            int x = rank - '1' + 2;
            int y = file - 'a' + 2;
            return new Vector2Int(x, y);
        }

        private void SetupSquares() {
            for (int i = 0; i < 12; i++) {
                for (int j = 0; j < 2; j++) {
                    Squares[i, j] = PieceType.OutOfBounds;
                    Squares[j, i] = PieceType.OutOfBounds;
                    Squares[11 - j, i] = PieceType.OutOfBounds;
                    Squares[i,11 - j] = PieceType.OutOfBounds;
                }
            }
        }
    }
}