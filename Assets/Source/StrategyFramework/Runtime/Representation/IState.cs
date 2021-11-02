using System.Collections.Generic;

namespace Source.StrategyFramework.Runtime.Representation {
    public interface IState {
        void Apply(IAction action);
        void Undo();
        List<IAction> GetAvailableActions();
    }
}