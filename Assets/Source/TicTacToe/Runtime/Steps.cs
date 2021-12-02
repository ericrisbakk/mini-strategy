using System;
using Source.StrategyFramework.Runtime.Representation;
using Source.TicTacToe.Runtime.Actions;
using Source.TicTacToe.Runtime.Objects;

namespace Source.TicTacToe.Runtime {

    public class DrawStep : IStep<GameState> {

        public Draw Action { get; }
        
        public DrawStep(Draw action) {
            Action = action;
        }
        
        public GameState Forward(GameState state) {
            var pos = Action.Position;

            state.GetBoard()[pos.x, pos.y].State = Action.Player.IsPlayer0 ? SquareState.Nought : SquareState.Cross;
            state.MoveCounter += 1;
            state.Player0Turn = !state.Player0Turn;
            return state;
        }

        public GameState ValidateForward(GameState state) {
            CommonValidation(state);
            if (state.Result != GameResult.Undecided)
                throw new Exception("[Validation][Step][Forward] The game is already over");
            if (!Rules.IsEmpty(state, Action.Position))
                throw new Exception("[Validation][Step][Forward] The square was not empty.");

            return state;
        }

        public GameState Backward(GameState state) {
            var pos = Action.Position;

            state.GetBoard()[pos.x, pos.y].State = SquareState.Empty;
            state.MoveCounter -= 1;
            state.Player0Turn = !state.Player0Turn;
            state.Result = GameResult.Undecided;
            return state;
        }

        public GameState ValidateBackward(GameState state) {
            CommonValidation(state);
            if (state.MoveCounter <= 0)
                throw new Exception("[Validation][Step][Backward] No moves have been made.");
            if (Rules.IsEmpty(state, Action.Position))
                throw new Exception("[Validation][Step][Backward] The square was empty.");

            return state;
        }

        private void CommonValidation(GameState state) {
            if (!Rules.IsInBounds(Action.Position))
                throw new Exception("[Validation][Step] Given coordinates were out of bounds.");
        }
    }
}
