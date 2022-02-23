using System;
using UnityEngine;
using static Source.Chess.Runtime.ChessConstants;

namespace Source.Chess.Runtime.Behaviours {
    public class PromotionButtonBehaviour : MonoBehaviour {
        [NonSerialized] public PieceBehaviour Piece;

        public Action<PieceType> Callback;

        public void OnClick() {
            Callback?.Invoke(Piece.piece);
        }
    }
}
