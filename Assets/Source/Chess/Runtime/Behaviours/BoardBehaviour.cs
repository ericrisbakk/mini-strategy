using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Source.Chess.Runtime.Behaviours {
    public class BoardBehaviour : MonoBehaviour {

        #region Variables

        public GameObject square;

        public Colors whiteColors;
        public Colors blackColors;
        public SquareBehaviour[,] Squares;
        
        #endregion
        
        
        
        [Button(ButtonSizes.Medium)]
        public void GenerateBoard() {
            for (int i = transform.childCount; i > 0; i--) {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            Squares = new SquareBehaviour[8, 8];
            
            for (int i = 0; i < 64; i++) {
                var s = Instantiate(square, transform);
                var sBehaviour = s.GetComponent<SquareBehaviour>();
                int x = i / 8;
                int y = i - (8 * x);
                Squares[x, y] = sBehaviour;
                sBehaviour.position = new Vector2Int(x, y);

                var oddRow = x % 2 == 0;
                var even = i % 2 == 0;
                sBehaviour.defaultColors = (oddRow && !even) || (!oddRow && even) ? blackColors : whiteColors;
                sBehaviour.UpdateDefaultColors();
            }
        }
    }
}
