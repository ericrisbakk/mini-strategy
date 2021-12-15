using System;

namespace Source.StrategyFramework.Runtime.Representation {

    /// <summary>
    /// A single IStep object corresponds to a single event within the game, containing the necessary info
    /// to both modify the state to bring it forwards or backwards in time.
    ///
    /// A forward step can create other steps
    /// </summary>
    public interface IStep<T> {
        public T Forward(T state);
        public T Backward(T state);
        public T ValidateForward(T state);
        public T ValidateBackward(T state);
    }
}