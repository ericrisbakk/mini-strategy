using Source.TicTacToe.Runtime.Objects;
using UnityEngine;

namespace Source.TicTacToe.Runtime {
    public class RulesDefinition {

        #region GameOver

        public static GameResult IsGameOver(GameState state, Vector2Int lastMovePosition) {
            var board = state.GetBoard();
            var x = lastMovePosition.x;
            var y = lastMovePosition.y;

            if (ThreeInRow(board, x, 0, 0, 1))
                return board[x, 0].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (ThreeInRow(board, 0, y, 1, 0))
                return board[x, 0].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (x == y && ThreeInRow(board, 0, 0, 1, 1))
                return board[x, 0].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (x + y == 2 && ThreeInRow(board, 0, 2, 1, -1))
                return board[x, 0].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (state.MoveCounter >= 9)
                return GameResult.Draw;

            return GameResult.Undecided;
        }

        private static bool ThreeInRow(Square[,] board, int x, int y, int deltaX, int deltaY) {
            SquareState mark1 = board[x, y].State;
            SquareState mark2 = board[x + deltaX, y + deltaY].State;
            SquareState mark3 = board[x + 2*deltaX, y + 2*deltaY].State;

            if (mark1 == SquareState.Empty || mark2 == SquareState.Empty || mark3 == SquareState.Empty)
                return false;

            if (mark1 == mark2 && mark2 == mark3)
                return true;

            return false;
        }
        
        #endregion
        
        #region Validity
        
        public static bool IsInBounds(Vector2Int pos) {
            if (pos.x < 0 || pos.x > 2)
                return false;
            if (pos.y < 0 || pos.y > 2)
                return false;
            return true;
        }

        public static bool IsEmpty(GameState state, Vector2Int pos) {
            return state.GetBoard()[pos.x, pos.y].State is SquareState.Empty;
        }

        #endregion
    }
}