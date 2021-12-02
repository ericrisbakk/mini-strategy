using System.Collections.Generic;
using Source.StrategyFramework.Runtime.Representation;
using Source.TicTacToe.Runtime.Actions;
using Source.TicTacToe.Runtime.Objects;
using UnityEngine;

namespace Source.TicTacToe.Runtime {
    public static class Rules {

        #region Play

        public static GameState Apply(GameState state, IAction action, out IStep<GameState> step) {
            step = new DrawStep((Draw) action);
            step.ValidateForward(state);
            return step.Forward(state);
        }

        // TODO: The changing of the game result should be a step on its own, such that it can be reverted properly.
        public static GameState Undo(GameState state, IStep<GameState> step) {
            step.ValidateBackward(state);
            step.Backward(state);
            return state;
        }

        public static GameState CheckGameOver(GameState state, Vector2Int lastPlay) {
            state.Result = IsGameOver(state, lastPlay);
            return state;
        }

        #endregion
        
        #region Information
        /// <summary>
        /// Return a list of all possible moves.
        ///
        /// TODO: Refactor when Actions become objects one can apply (Or should I use Func?)
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<Vector2Int> GetActions(GameState state) {
            var board = state.GetBoard();
            var actions = new List<Vector2Int>();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (board[i,j].State == SquareState.Empty)
                        actions.Add(new Vector2Int(i, j));
                }
            }

            return actions;
        }

        #endregion
        
        #region GameOver

        /// <summary>
        /// Determine whether someone placed three in a row - uses the last made move to narrow down where to check.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="lastMovePosition"></param>
        /// <returns></returns>
        public static GameResult IsGameOver(GameState state, Vector2Int lastMovePosition) {
            var board = state.GetBoard();
            var x = lastMovePosition.x;
            var y = lastMovePosition.y;

            if (ThreeInRow(board, x, 0, 0, 1))
                return board[x, 0].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (ThreeInRow(board, 0, y, 1, 0))
                return board[0, y].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (x == y && ThreeInRow(board, 0, 0, 1, 1))
                return board[0, 0].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (x + y == 2 && ThreeInRow(board, 0, 2, 1, -1))
                return board[0, 2].State == SquareState.Cross ? GameResult.PlayerXWins : GameResult.Player0Wins;

            if (state.MoveCounter >= 9)
                return GameResult.Draw;

            return GameResult.Undecided;
        }

        /// <summary>
        /// Checking three in a row. (x,y) must be a position on the edge of the board, and
        /// (deltaX, deltaY) is added to (x,y) to find the other squares of the line.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Check that the position is inside the board.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool IsInBounds(Vector2Int pos) {
            if (pos.x < 0 || pos.x > 2)
                return false;
            if (pos.y < 0 || pos.y > 2)
                return false;
            return true;
        }

        /// <summary>
        /// Check whether a position is empty.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool IsEmpty(GameState state, Vector2Int pos) {
            return state.GetBoard()[pos.x, pos.y].State is SquareState.Empty;
        }

        #endregion
    }
}