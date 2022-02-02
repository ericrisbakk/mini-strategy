using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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
        
        public Vector2Int clickedTarget;
        public bool clicked;
        
        #endregion

        #endregion
        
        private void Awake() {
            foreach (var s in Squares) {
                s.OnPointerClick -= OnPointerClick;
                s.OnPointerClick += OnPointerClick;
                s.OnPointerEnter -= OnPointerEnter;
                s.OnPointerEnter += OnPointerEnter;
                s.OnPointerExit -= OnPointerExit;
                s.OnPointerExit += OnPointerExit;
            }
        }

            // TODO: Clicking on somewhere not highlighted while having clicked should set that to the new target.
            private void OnPointerClick(Vector2Int target) {
            if (clicked) {
                if (target == clickedTarget) {
                    clicked = false;
                }
                else {
                    // clickedTarget = target;
                }
            }
            else {
                clicked = true;
                clickedTarget = target;
            }
        }

        private void OnPointerEnter(Vector2Int target) {
            if (!clicked) { 
                Highlight(target);
            }
        }
        
        private void OnPointerExit(Vector2Int target) {
            if (!clicked) { 
                ClearHighlight();
            }
        }

        private void Highlight(Vector2Int target) {
            var s = Squares[target.x, target.y];
            s.Highlight(HighlightColorsDict[HighlightType.Selected],
                true, true, true, true, true);
        }

        private void ClearHighlight() {
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
