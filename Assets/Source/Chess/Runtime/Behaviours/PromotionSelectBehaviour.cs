using Sirenix.OdinInspector;
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
            }
        }

        private void DestroyChildren() {
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i));
            }
        }
    }
}
