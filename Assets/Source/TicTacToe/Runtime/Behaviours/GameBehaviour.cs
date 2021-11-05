using System;
using Source.TicTacToe.Runtime.Objects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.TicTacToe.Runtime.Behaviours {
    public class GameBehaviour : MonoBehaviour {
        [NonSerialized]
        public GameState State;

        private void Awake() {
            State = new GameState();
        }

        public void MakeMove(Player player, Vector2Int pos) {
            ActionsDefinition.Draw(State, pos);
            var result = RulesDefinition.IsGameOver(State, pos);
            if (result == GameResult.Undecided)
                return;

            GameOver(result);
        }

        private void GameOver(GameResult result) {
            throw new NotImplementedException();
        }
    }
}
