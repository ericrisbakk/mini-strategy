using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Behaviours {
    public enum HighlightType {
        Selected,
        Move,
        Capture,
        Castle,
        EnPassant,
        Promotion,
        Check,
        Checkmate
    }
    
    public class BoardBehaviour : SerializedMonoBehaviour {

        #region Variables

        public GameObject square;

        public Colors whiteColors;
        public Colors blackColors;
        public Dictionary<HighlightType, Colors> HighlightColorsDict = new Dictionary<HighlightType, Colors>();
        public SquareBehaviour[,] Squares;

        #region State

        [NonSerialized] public GameObject[,] Pieces = new GameObject[8,8];

        #endregion

        #endregion

        public void Highlight(Dictionary<HighlightType, List<Vector2Int>> highlights) {
            foreach (var pair in highlights) {
                var highlight = pair.Key;
                foreach (var target in pair.Value) {
                    Highlight(target, highlight);
                }
            }
        }
        
        public void Highlight(Vector2Int target, HighlightType highlight) {
            var s = Squares[target.x, target.y];
            s.Highlight(HighlightColorsDict[highlight],
                true, true, true, true, true);
        }

        public void ClearHighlight() {
            foreach (var s in Squares) {
                s.ClearHighlight();
            }
        }


#if UNITY_EDITOR
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
#endif
    }
}
