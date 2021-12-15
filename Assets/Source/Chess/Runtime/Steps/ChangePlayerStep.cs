using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Steps {
    public class ChangePlayerStep : IStep<GameState> {

        public GameState Forward(GameState state) {
            return FlipCurrentPlayer(state);
        }
        
        public GameState Backward(GameState state) {
            return FlipCurrentPlayer(state);
        }

        private static GameState FlipCurrentPlayer(GameState state) {
            state.CurrentPlayer = Rules.GetOtherPlayer(state, state.CurrentPlayer);
            return state;
        }

        public GameState ValidateForward(GameState state) {
            return CommonValidation(state);
        }

        public GameState ValidateBackward(GameState state) {
            return CommonValidation(state);
        }

        private static GameState CommonValidation(GameState state) {
            Assert.AreNotEqual(state.Black, state.White);
            return state;
        }
    }
}