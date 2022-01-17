using System;
using System.Collections.Generic;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
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

        private LinearHistory _history;
        
        #endregion

        private void Awake() {
            State = new GameState();
            _history = new LinearHistory();
            
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

        public void Undo() {
            if (State.MoveCounter > 0) {
                var lastStep = (IStep<GameState, LinearHistory>) _history.LastStep;
                Rules.Undo(State, _history, lastStep);
                _history.Backtrack();
                var action = ((DrawStep) lastStep).Action;
                var isPlayer0 = action.Player.IsPlayer0;
                var pos = action.Position;
                squares[pos.x*3 + pos.y].UpdateState();
                Debug.Log("Undo move "+ (State.MoveCounter + 1) + "Player " + (isPlayer0 ? "0" : "X") 
                                        + "'s turn, tried to draw at " + pos);
            }
            else
                Debug.Log("Could not undo. Board is empty.");
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

            Rules.Apply(State, _history, action, out var step);
            _history.Add(action, new List<IStep>(new []{step}));
            
            Debug.Log("Move " + State.MoveCounter + ": Player " + (State.Player0Turn ? "0" : "X") 
                      + "'s turn, tried to draw at " + pos);
            
            squares[pos.x*3 + pos.y].UpdateState();
            
            var result = Rules.CheckGameOver(State, pos).Result;
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
