using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Chess.Runtime.Behaviours {
    [Serializable]
    public struct Colors {
        public UnityEngine.Color faceDefault;
        public UnityEngine.Color faceHighlight;
        public UnityEngine.Color edgeDefault;
        public UnityEngine.Color edgeHighlight;
    }
    
    public class SquareBehaviour : MonoBehaviour { 
        public Colors Colors { get; set; }

        public Image face;
        public Image upperEdge;
        public Image rightEdge;
        public Image lowerEdge;
        public Image leftEdge;

        public void Highlight(bool newFace, bool upper, bool right, bool lower, bool left) {
            face.color = newFace ? Colors.faceHighlight : Colors.faceDefault;
            SetEdgeHighlight(upperEdge, upper);
            SetEdgeHighlight(rightEdge, right);
            SetEdgeHighlight(lowerEdge, lower);
            SetEdgeHighlight(leftEdge, left);
        }

        public void ClearHighlight() {
            face.color = Colors.faceDefault;
            SetEdgeHighlight(upperEdge, false);
            SetEdgeHighlight(rightEdge, false);
            SetEdgeHighlight(lowerEdge, false);
            SetEdgeHighlight(leftEdge, false);
        }

        private void SetEdgeHighlight(Image edge, bool highlight) {
            edge.color = highlight ? Colors.edgeHighlight : Colors.edgeDefault;
        }
    }
}
