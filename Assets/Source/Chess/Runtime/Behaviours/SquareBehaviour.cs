using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.Chess.Runtime.Behaviours {
    [Serializable]
    public struct Colors {
        public UnityEngine.Color face;
        public UnityEngine.Color edges;
    }
    
    public class SquareBehaviour : MonoBehaviour {

        
        #region Variables
        
        public UnityEngine.Color invisible;

        #region Hookups

        public EventTrigger eventTrigger;
        public Image face;
        public Image highlight;
        public Image upperEdge;
        public Image rightEdge;
        public Image lowerEdge;
        public Image leftEdge;
        
        #endregion

        public Colors defaultColors;
        public Vector2Int position;
        
        #region State
        
        [NonSerialized] public Colors HighlightColors;
        private Image[] _edges;

        #endregion
        
        #endregion
        
        public Action<Vector2Int> PointerClicked;
        public Action<Vector2Int> PointerEntered;
        public Action<Vector2Int> PointerExited;
        
        public void PointerClick() { PointerClicked?.Invoke(position); }
        
        public void PointerEnter() { PointerEntered?.Invoke(position); }
        
        public void PointerExit() { PointerExited?.Invoke(position); }
        
#if UNITY_EDITOR

        public void UpdateDefaultColors() {
            face.color = defaultColors.face;
            highlight.color = invisible;
            upperEdge.color = invisible;
            rightEdge.color = invisible;
            lowerEdge.color = invisible;
            leftEdge.color = invisible;
        }
#endif

        public void Highlight(Colors highlightColors, bool newFace, bool upper, bool right, bool lower, bool left) {
            HighlightColors = highlightColors;
            highlight.color = newFace ? HighlightColors.face : invisible;
            SetEdgeHighlight(upperEdge, upper);
            SetEdgeHighlight(rightEdge, right);
            SetEdgeHighlight(lowerEdge, lower);
            SetEdgeHighlight(leftEdge, left);
        }

        public void ClearHighlight() {
            highlight.color = invisible;
            foreach (var edge in _edges) {
                SetEdgeHighlight(edge, false);
            }
        }

        private void SetEdgeHighlight(Image edge, bool highlighted) {
            edge.color = highlighted ? HighlightColors.edges : defaultColors.edges;
        }


        #region Unity

        private void Awake() {
            _edges = new[] {
                upperEdge,
                rightEdge,
                lowerEdge,
                leftEdge
            };
        }

        #endregion
    }
}
