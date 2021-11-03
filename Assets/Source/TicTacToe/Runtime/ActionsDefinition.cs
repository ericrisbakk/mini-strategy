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
        /// </summary>
        /// <param name="state"></param>
        /// <param name="player"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static GameState Draw(GameState state, Player player, Vector2Int pos) {
            Assert.IsTrue(IsInBounds(pos));
            Assert.IsTrue(IsEmpty(state, pos));
            
            state.GetBoard()[pos.x, pos.y].State = player.IsPlayer0 ? SquareState.Nought : SquareState.Cross;
            return state;
        }
        
        #endregion

        #region Validity

        private static bool IsInBounds(Vector2Int pos) {
            if (pos.x < 0 || pos.x > 2)
                return false;
            if (pos.y < 0 || pos.y > 2)
                return false;
            return true;
        }

        private static bool IsEmpty(GameState state, Vector2Int pos) {
            return state.GetBoard()[pos.x, pos.y].State is SquareState.Empty;
        }

        #endregion
        
    }
}