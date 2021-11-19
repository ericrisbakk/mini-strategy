using Source.TicTacToe.Runtime.Objects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.TicTacToe.Runtime {
    
    public class ActionsDefinition {

        #region Actions

        /// <summary>
        /// The player draws their symbol on an empty square.
        ///
        /// TODO: Implement as a functional approach (Railway oriented programming)
        /// TODO: Actions are also data objects. Convert into a class.
        /// TODO: Asserts are not the correct way to do this. Error massages as in ROP
        /// TODO: Actions should change the game result as well! Remember to call isGameOver()
        /// </summary>
        /// <param name="state"></param>
        /// <param name="player"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static GameState Draw(GameState state, Vector2Int pos) {
            Assert.IsTrue(RulesDefinition.IsInBounds(pos));
            Assert.IsTrue(RulesDefinition.IsEmpty(state, pos));
            state.GetBoard()[pos.x, pos.y].State = state.Player0Turn ? SquareState.Nought : SquareState.Cross;
            state.MoveCounter += 1;
            state.Player0Turn = !state.Player0Turn;
            return state;
        }
        
        
        #endregion
    }
}