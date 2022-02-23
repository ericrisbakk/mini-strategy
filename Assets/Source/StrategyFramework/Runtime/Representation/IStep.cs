using System;

namespace Source.StrategyFramework.Runtime.Representation {
    // TODO: Will be combined with IStep.
    // TODO: The current plan is to make this a data object, and handle forwards/backwards/validation behaviour through static methods.
    public interface IStep {}

    /// <summary>
    /// A single IStep object corresponds to a single event within the game, containing the necessary info
    /// to both modify the state to bring it forwards or backwards in time.
    ///
    /// A forward step can create other steps
    /// </summary>
    public interface IStep<TState, THistory> : IStep
    where TState : IState
    where THistory : IHistory {
        public TState Forward(TState state, THistory history);
        public TState Backward(TState state, THistory history);
        public TState ValidateForward(TState state, THistory history);
        public TState ValidateBackward(TState state, THistory history);
    }
}