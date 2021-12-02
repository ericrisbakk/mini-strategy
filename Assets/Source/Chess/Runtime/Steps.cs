using System;
using System.Collections.Generic;
using Source.Chess.Runtime.Actions;
using Source.StrategyFramework.Runtime.Representation;

namespace Source.Chess.Runtime {
    public class Steps {

        public class MoveStep : IStep<GameState> {
            public Move move { get; }
            
            public MoveStep(Move move) {
                this.move = move;
            }

            public GameState Forward(GameState state) {
                // TODO: Use PieceType information to select what function to use.
                throw new System.NotImplementedException();
            }

            public GameState ValidateForward(GameState state) {
                throw new System.NotImplementedException();
            }

            public GameState Backward(GameState state) {
                throw new System.NotImplementedException();
            }

            public GameState ValidateBackward(GameState state) {
                throw new System.NotImplementedException();
            }
        }
    }
}