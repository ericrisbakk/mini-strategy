using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Source.Chess.Runtime.Behaviours {
    [Serializable]
    public struct Colors {
        public UnityEngine.Color face;
        public UnityEngine.Color edges;
    }
    
    public class SquareBehaviour : MonoBehaviour {
        public Colors DefaultColors { get; set; }
        public Colors HighlightColors { get; set; }
        
        public Image face;
        public Image highlight;
        public Image upperEdge;
        public Image rightEdge;
        public Image lowerEdge;
        public Image leftEdge;

        public void Highlight(Colors highlightColors, bool newFace, bool upper, bool right, bool lower, bool left) {
            HighlightColors = highlightColors;
            face.color = newFace ? HighlightColors.face : DefaultColors.face;
            SetEdgeHighlight(upperEdge, upper);
            SetEdgeHighlight(rightEdge, right);
            SetEdgeHighlight(lowerEdge, lower);
            SetEdgeHighlight(leftEdge, left);
        }

        public void ClearHighlight() {
            face.color = DefaultColors.face;
            SetEdgeHighlight(upperEdge, false);
            SetEdgeHighlight(rightEdge, false);
            SetEdgeHighlight(lowerEdge, false);
            SetEdgeHighlight(leftEdge, false);
        }

        private void SetEdgeHighlight(Image edge, bool highlight) {
            edge.color = highlight ? HighlightColors.edges : DefaultColors.edges;
        }
    }
}
