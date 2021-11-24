using System;
using Source.TicTacToe.Runtime.Actions;
using Source.TicTacToe.Runtime.Objects;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Source.TicTacToe.Runtime {
    
    /// <summary>
    /// A single IStep object corresponds to a single event within the game, containing the necessary info
    /// to both modify the state to bring it forwards or backwards in time.
    ///
    /// A forward step can create other steps
    /// </summary>
    public interface IStep {
        public GameState Forward(GameState state);
        public GameState ValidateForward(GameState state);
        public GameState Backward(GameState state);
        public GameState ValidateBackward(GameState state);

    }

    public class DrawStep : IStep {

        public Draw action { get; }
        
        public DrawStep(Draw action) {
            action = action;
        }
        
        public GameState Forward(GameState state) {
            var pos = action.Position;

            state.GetBoard()[pos.x, pos.y].State = action.Player.IsPlayer0 ? SquareState.Nought : SquareState.Cross;
            state.MoveCounter += 1;
            state.Player0Turn = !state.Player0Turn;
            return state;
        }

        public GameState ValidateForward(GameState state) {
            CommonValidation(state);
            if (RulesDefinition.IsEmpty(state, action.Position))
                throw new Exception("[Validation][Step][Forward] The square was not empty.");

            return state;
        }

        public GameState Backward(GameState state) {
            var pos = action.Position;

            state.GetBoard()[pos.x, pos.y].State = SquareState.Empty;
            state.MoveCounter -= 1;
            state.Player0Turn = !state.Player0Turn;
            return state;
        }

        public GameState ValidateBackward(GameState state) {
            CommonValidation(state);
            if (!RulesDefinition.IsEmpty(state, action.Position))
                throw new Exception("[Validation][Step][Backward] The square was empty.");

            return state;
        }

        private void CommonValidation(GameState state) {
            if (RulesDefinition.IsInBounds(action.Position))
                throw new Exception("[Validation][Step] Given coordinates were out of bounds.");
        }
    }
}
