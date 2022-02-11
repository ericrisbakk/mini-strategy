using System;
using System.Linq;
using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Steps {
    public static class StepValidation {

        public static readonly PieceType[] Pawns = {PieceType.WPawn, PieceType.BPawn};
        public static readonly PieceType[] Kings = {PieceType.WKing, PieceType.BKing};
        
        public static void ActionCountValid(GameState state, LinearHistory history) {
            Assert.IsTrue(0 <= state.ActionCount && state.ActionCount <= history.Events.Count,
                "0 <= state.ActionCount && state.ActionCount <= history.Events.Count");
        }
        
        /// <summary>
        /// Asserts that `player.Color` is not value `Color.Unassigned`.
        /// </summary>
        public static void PlayerColorAssigned(Player player) {
            Assert.AreNotEqual(player.Color, Color.Unassigned, 
                "Player must have a color.");
        }

        public static void PlayerIsColor(Player player, Color color) {
            Assert.IsTrue(player.Color == color,
                $"Player color was not value {color}.");
        }

        /// <summary>
        /// Asserts that `piece` belongs to the `player`.
        /// </summary>
        public static void OwnsPiece(Player player, PieceType piece) {
            Assert.IsTrue(Rules.OwnsPiece(player, piece),
                $"Player {player.Color} and piece color {Rules.ColorOfPiece(piece)} do not match.");
        }

        /// <summary>
        /// Asserts that the given `piece` value is not `PieceType.OutOfBounds`.
        /// </summary>
        public static void InBounds(PieceType piece, string target) {
            Assert.IsTrue(piece != PieceType.OutOfBounds,
                $"{target} value should never be {PieceType.OutOfBounds}");
        }

        public static void InBounds(GameState state, Vector2Int target) {
            Assert.IsTrue(state.Square(target) != PieceType.OutOfBounds,
                $"Target {target} value should never be {PieceType.OutOfBounds}.");
        }

        /// <summary>
        /// Asserts that `p1` is `Color.White` and  `p2` is `Color.Black`, or the other way around.
        /// </summary>
        public static void OpposingPieces(PieceType p1, PieceType p2) {
            var c1 = Rules.ColorOfPiece(p1);
            var c2 = Rules.GetOtherColor(c1);
            Assert.IsTrue(c1 == Color.White && c2 == Color.Black || c1 == Color.Black && c2 == Color.White,
                $"Pieces {p1} and {p2} should be opposing pieces.");
        }

        public static void PositionIsPiece(PieceType[,] squares, Vector2Int pos, PieceType piece) {
            Assert.IsTrue(squares[pos.x, pos.y] == piece,
                $"Position ({pos.x}, {pos.y}) is supposed to be of value {piece}.");
        }
        
        public static void PositionIsPiece(PieceType[,] squares, Vector2Int pos, PieceType[] pieces) {
            Assert.IsTrue( pieces.Contains(squares[pos.x, pos.y]),
                $"Position ({pos.x}, {pos.y}) is supposed to be of value in {pieces}.");
        }

        public static void PositionIsNotPiece(PieceType[,] squares, Vector2Int pos, PieceType[] pieces) {
            Assert.IsFalse( pieces.Contains(squares[pos.x, pos.y]),
                $"Position ({pos.x}, {pos.y}) is *not* supposed to be a value of {pieces}.");
        }

        public static void PieceIs(PieceType p1, PieceType p2) {
            Assert.IsTrue(p1 == p2, 
                $"Piece {p1} must be of type {p2}");
        }
        
        public static void PieceIs(PieceType piece, PieceType[] pieces) {
            Assert.IsTrue(pieces.Contains(piece), 
                $"{piece} was not found among candidates in {pieces}");
        }

        public static void PieceIsNot(PieceType piece, PieceType[] pieces) {
            Assert.IsTrue(!pieces.Contains(piece),
                $"{piece} was found among candidates in {pieces}");
        }

        public static void StepIs(IStep step, Type type) {
            Assert.IsTrue(step.GetType() == type,
                $"Given step {step} should be of type {type}.");
        }

        public static void StepIsSubclassOf(IStep step, Type type) {
            Assert.IsTrue(step.GetType().IsSubclassOf(type),
                $"Given step {step} should be a subclass of type {type}");
        }

        public static void AssertEmptyLine(GameState state, Vector2Int source, Vector2Int target) {
            Assert.IsTrue(Rules.EmptyLine(state, source, target),
                $"Squares between {source} and {target} must be empty.");
        }

        public static void AssertStraightMovement(Vector2Int source, Vector2Int target) {
            var dx = target.x - source.x;
            var dy = target.y - source.y;
            Assert.IsTrue((dx == 0 && dy != 0) || (dx != 0 && dy == 0),
                $"Movement form {source} to {target} must be straight (horizontal or vertical).");
        }
        
        public static void AssertDiagonalMovement(Vector2Int source, Vector2Int target) {
            var dx = target.x - source.x;
            var dy = target.y - source.y;
            Assert.IsTrue(Math.Abs(dx) == Math.Abs(dy),
                $"Movement form {source} to {target} must be straight diagonal (45 degrees).");
        }
    }
}