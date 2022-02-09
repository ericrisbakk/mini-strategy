using System;
using UnityEngine;

namespace Source.Chess.Runtime.Behaviours {
    public class PromotionButtonBehaviour : MonoBehaviour {
        [NonSerialized] public PieceBehaviour Piece;

        public Action<PieceType> Callback;

        public void OnClick() {
            Callback?.Invoke(Piece.piece);
        }
    }
}
