using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Chess.Runtime.Behaviours {
    public class BoardBehaviour : MonoBehaviour {
        public GameObject square;

        public Colors whiteColors;
        public Colors blackColors;

        [Button]
        public void GenerateBoard() {
            for (int i = transform.childCount; i > 0; i--) {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            for (int i = 0; i < 64; i++) {
                var s = Instantiate(square, transform);
                var sBehaviour = s.GetComponent<SquareBehaviour>();
                var oddRow = (i / 8) % 2 == 0;
                var even = i % 2 == 0;
                sBehaviour.Colors = (oddRow && !even) || (!oddRow && even) ? blackColors : whiteColors;
                sBehaviour.ClearHighlight();
            }
        }
    }
}