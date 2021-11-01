using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {
    void Apply(IAction action);
    void Undo();
    List<IAction> GetAvailableActions();
}
