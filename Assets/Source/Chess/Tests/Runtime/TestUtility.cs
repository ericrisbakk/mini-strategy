using System;
using Source.Chess.Runtime;
using Source.Chess.Runtime.Objects;
using UnityEngine;

namespace Source.Chess.Tests.Runtime {
    public static class TestUtility {
        public static Tuple<Vector2Int, PieceType> GetTuple(int x, int y, PieceType piece)
            => new Tuple<Vector2Int, PieceType>(new Vector2Int(x, y), piece);
        
        public static Tuple<Vector2Int, PieceType> GetTuple(char rank, char file, PieceType piece)
            => new Tuple<Vector2Int, PieceType>(Rules.ToVector2Int(rank, file), piece);
    }
}