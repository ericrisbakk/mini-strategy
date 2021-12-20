using System;
using System.Collections.Generic;
using Source.StrategyFramework.Runtime.Representation;

namespace Source.StrategyFramework.Runtime.History {
    
    /// <summary>
    /// Linear history - keeps track of Action-Step tuples inside a list. Backtracking means the event is lost to time.
    /// </summary>
    public class LinearHistory {
        public List<Tuple<IAction, List<IStep>>> Events;

        public LinearHistory() {
            Events = new List<Tuple<IAction, List<IStep>>>();
        }

        public void Add(IAction action, List<IStep> stepList) {
            Events.Add(new Tuple<IAction, List<IStep>>(action, stepList));
        }

        public void Backtrack() { 
            Events.RemoveAt(Events.Count-1);
        }
    }
}
