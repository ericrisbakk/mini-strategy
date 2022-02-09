using Sirenix.OdinInspector;
using Source.Chess.Runtime.Actions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Behaviours {
    public class PromotionSelectBehaviour : MonoBehaviour {
        #region Variables
    
        public GameObject pieceButton;
        public ViewControllerBehaviour viewController;

        public Vector3 pieceImgScale;

        #endregion

        [Button(ButtonSizes.Medium)]
        public void PresentPromotions(Color color) {
            DestroyChildren();
            Assert.IsTrue(color != Color.Unassigned);
            var isWhite = color == Color.White;
            var start = isWhite ? 3 : 9;
            var stop = isWhite ? 7 : 13;
            for (int i = start; i < stop; i++) {
                var button = Instantiate(pieceButton, transform);
                var pieceImg = Instantiate(viewController.piecePrefabDict[(PieceType) i], button.transform);
                pieceImg.transform.localScale = pieceImgScale;

                var pieceBehaviour = pieceImg.GetComponent<PieceBehaviour>();
                var promotionButtonBehaviour = button.GetComponent<PromotionButtonBehaviour>();
                promotionButtonBehaviour.Piece = pieceBehaviour;
                promotionButtonBehaviour.Callback -= PromoteTo;
                promotionButtonBehaviour.Callback += PromoteTo;
            }
        }

        private void DestroyChildren() {
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public void PromoteTo(PieceType piece) {
            viewController.Promote(piece);
            DestroyChildren();
        }
    }
}
