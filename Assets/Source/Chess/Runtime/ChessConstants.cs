using System;
using System.Collections.Generic;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Steps;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using static Source.Chess.Runtime.ChessRules;

namespace Source.Chess.Runtime {
    public static class ChessConstants {
        
        /// <summary>
        /// Encodes the possible values a square on the board can have. The `OutOfBounds` value is used to simplify
        /// checking whether a coordinate is out of bounds.
        /// NB: The values have been assigned because the order is set explicitly, and some methods are using that order.
        /// </summary>
        public enum PieceType {
            Empty = 0,
            OutOfBounds = 1,
            WPawn = 2,
            WKnight = 3,
            WBishop = 4,
            WRook = 5,
            WQueen = 6,
            WKing = 7,
            BPawn = 8,
            BKnight = 9,
            BBishop = 10,
            BRook = 11,
            BQueen = 12,
            BKing = 13
        }

        public enum Color {
            Unassigned = 0,
            White,
            Black,
        }
        
        public const string StandardWhite = "a2,b2,c2,d2,e2,f2,g2,h2,Ra1,Nb1,Bc1,Qd1,Ke1,Bf1,Ng1,Rh1";
        public const string StandardBlack = "a7,b7,c7,d7,e7,f7,g7,h7,Ra8,Nb8,Bc8,Qd8,Ke8,Bf8,Ng8,Rh8";
        
        public const int whiteBackRow = 2;
        public const int whitePawnRow = 3;
        public const int whitePawnDirection = 1;
        public const int blackBackRow = 9;
        public const int blackPawnRow = 8;
        public const int blackPawnDirection = -1;

        public static readonly int[][] StraightDeltas = new int[][] {
            new int[] {1, 0},
            new int[] {-1, 0},
            new int[] {0, 1},
            new int[] {0, -1},
        };

        public static readonly int[][] DiagonalDeltas = new int[][] {
            new int[] {1, 1},
            new int[] {-1, 1},
            new int[] {1, -1},
            new int[] {-1, -1},
        };

        public static readonly int[][] KnightDeltas = new int[][] {
            new int[] {1, 2},
            new int[] {-1, 2},
            new int[] {1, -2},
            new int[] {-1, -2},
            new int[] {2, 1},
            new int[] {-2, 1},
            new int[] {2, -1},
            new int[] {-2, -1},
        };
        
        public static readonly Dictionary<PieceType, Func<GameState, LinearHistory, Vector2Int, List<IAction>>> 
            PieceActionDict = new Dictionary<PieceType, Func<GameState, LinearHistory, Vector2Int, List<IAction>>>() {
                {PieceType.WPawn, GetPawnActions},
                {PieceType.BPawn, GetPawnActions},
                {PieceType.WKnight, GetKnightActions},
                {PieceType.BKnight, GetKnightActions},
                {PieceType.WRook, GetRookActions},
                {PieceType.BRook, GetRookActions},
                {PieceType.WBishop, GetBishopActions},
                {PieceType.BBishop, GetBishopActions},
                {PieceType.WQueen, GetQueenActions},
                {PieceType.BQueen, GetQueenActions},
                {PieceType.WKing, GetKingActions},
                {PieceType.BKing, GetKingActions},
            };

        public static readonly Dictionary<PieceType, Func<Move, MoveStep>> PieceStepDict =
            new Dictionary<PieceType, Func<Move, MoveStep>>() {
                {PieceType.WPawn, move => new PawnMoveStep(move)},
                {PieceType.BPawn, move => new PawnMoveStep(move)},
                {PieceType.WKnight, move => new KnightMoveStep(move)},
                {PieceType.BKnight, move => new KnightMoveStep(move)},
                {PieceType.WRook, move => new RookMoveStep(move)},
                {PieceType.BRook, move => new RookMoveStep(move)},
                {PieceType.WBishop, move => new BishopMoveStep(move)},
                {PieceType.BBishop, move => new BishopMoveStep(move)},
                {PieceType.WQueen, move => new QueenMoveStep(move)},
                {PieceType.BQueen, move => new QueenMoveStep(move)},
                {PieceType.WKing, move => new KingMoveStep(move)},
                {PieceType.BKing, move => new KingMoveStep(move)},
            };
    }
}