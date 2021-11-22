using System;
using System.Collections.Generic;
using Source.TicTacToe.Runtime.Actions;
using Source.TicTacToe.Runtime.Objects;
using UnityEngine;

namespace Source.TicTacToe.Runtime.Behaviours {
    public class ViewControllerBehaviour : MonoBehaviour {

        public enum GamePhase {
            Waiting,
            Ongoing,
            Ended
        }
        
        #region Variables

        [NonSerialized]
        public GameState State;
        
        public List<SquareBehaviour> squares;

        #endregion

        private void Awake() {
            State = new GameState();
            // TODO: Rewrite s.t. this is not dependent on the implicit order of `squares`.
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    var square = squares[3 * i + j];
                    square.Target = State.GetBoard()[i, j];
                    square.UpdateState();
                    
                    square.OnClickedSquare -= SquareWasClicked;
                    square.OnClickedSquare += SquareWasClicked;
                }
            }
        }
        
        /// <summary>
        /// Handle event on when a square was clicked.
        /// TODO: Consider how to improve updating the view.
        /// </summary>
        /// <param name="square"></param>
        private void SquareWasClicked(Square square) {
            var pos = square.Position;
            var player = State.GetCurrentPlayer;
            var action = new Draw(pos, player);
            
            Debug.Log("Player " + (State.Player0Turn ? "0" : "X") + "'s turn, tried to draw at " + pos);
            
            ActionsDefinition.Draw(State, pos);
            
            squares[pos.x*3 + pos.y].UpdateState();
            
            var result = RulesDefinition.IsGameOver(State, pos);
            if (result == GameResult.Undecided)
                return;

            GameOver(result);
        }

        private void GameOver(GameResult result) {
            string msg = "";
            
            switch (result) {
                case GameResult.Draw:
                    msg = "The result was a draw!";
                    break;
                case GameResult.Player0Wins:
                    msg = "Player 0 wins!";
                    break;
                case GameResult.PlayerXWins:
                    msg = "Player X wins!";
                    break;
            }
            
            Debug.Log(msg);
        }
    }
}
