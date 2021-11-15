using System;
using System.Collections;
using System.Collections.Generic;
using Source.TicTacToe.Runtime.Objects;
using UnityEngine;

public class SquareBehaviour : MonoBehaviour {
    
    public GameObject nought;
    public GameObject cross;
    
    [NonSerialized]
    public Square Target;

    public delegate void ClickedSquare(Square square);
    
     [NonSerialized]
    public ClickedSquare OnClickedSquare;

    public void UpdateState() {
        nought.SetActive(false);
        cross.SetActive(false);
        
        switch (Target.State) {
            case SquareState.Cross:
                cross.SetActive(true);
                break;
            case SquareState.Nought:
                nought.SetActive(true);
                break;
        }
    }

    public void Click() {
        OnClickedSquare?.Invoke(Target);
    }
}
