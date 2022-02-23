using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;
using Checks = Source.Chess.Runtime.ChessChecks;

namespace Source.Chess.Runtime.Steps {
    public class ChangePlayerStep : IStep<GameState, LinearHistory> {

        public GameState Forward(GameState state, LinearHistory history) {
            return FlipCurrentPlayer(state);
        }
        
        public GameState Backward(GameState state, LinearHistory history) {
            return FlipCurrentPlayer(state);
        }

        private static GameState FlipCurrentPlayer(GameState state) {
            state.CurrentPlayer = Checks.GetOtherPlayer(state, state.CurrentPlayer);
            return state;
        }

        public GameState ValidateForward(GameState state, LinearHistory history) {
            return CommonValidation(state);
        }

        public GameState ValidateBackward(GameState state, LinearHistory history) {
            return CommonValidation(state);
        }

        private static GameState CommonValidation(GameState state) {
            Assert.AreNotEqual(state.Black, state.White);
            return state;
        }
    }
}