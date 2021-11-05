using System;
using System.Collections;
using System.Collections.Generic;
using Source.TicTacToe.Runtime.Objects;
using UnityEngine;

public class SquareBehaviour : MonoBehaviour {
    
    public GameObject nought;
    public GameObject cross;
    
    [NonSerialized] public SquareState State;
    // Start is called before the first frame update

    private void Awake() {
        State = SquareState.Empty;
    }

    public void SetState(SquareState newState) {
        nought.SetActive(false);
        cross.SetActive(false);
        State = newState;
        
        if (newState == SquareState.Cross)
            cross.SetActive(true);
        if (newState == SquareState.Nought)
            nought.SetActive(true);
    }
}
