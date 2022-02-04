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
        public char rank;
        public char file;
        
        #region State
        
        [NonSerialized] public Colors HighlightColors;
        // Convenient short-hand.
        private Image[] _edges;

        #endregion
        
        #endregion
        
        public Action<char, char> OnPointerClick;
        public Action<char, char> OnPointerEnter;
        public Action<char, char> OnPointerExit;
        
        public void PointerClick() { OnPointerClick?.Invoke(rank, file); }
        
        public void PointerEnter() { OnPointerEnter?.Invoke(rank, file); }
        
        public void PointerExit() { OnPointerExit?.Invoke(rank, file); }
        
#if UNITY_EDITOR

        /// <summary>
        /// Set face color to the default color, then set all highlight images to invisible.
        /// </summary>
        public void UpdateDefaultColors() {
            face.color = defaultColors.face;
            highlight.color = invisible;
            upperEdge.color = invisible;
            rightEdge.color = invisible;
            lowerEdge.color = invisible;
            leftEdge.color = invisible;
        }
#endif

        /// <summary>
        /// Update highlight colors, then set highlights based on given `bool` values.
        /// </summary>
        public void Highlight(Colors highlightColors, bool newFace, bool upper, bool right, bool lower, bool left) {
            HighlightColors = highlightColors;
            highlight.color = newFace ? HighlightColors.face : invisible;
            SetEdgeHighlight(upperEdge, upper);
            SetEdgeHighlight(rightEdge, right);
            SetEdgeHighlight(lowerEdge, lower);
            SetEdgeHighlight(leftEdge, left);
        }

        /// <summary>
        /// Set all highlights to invisible.
        /// </summary>
        public void ClearHighlight() {
            highlight.color = invisible;
            foreach (var edge in _edges) {
                SetEdgeHighlight(edge, false);
            }
        }

        /// <summary>
        /// Set a given image to either the highlight color, or invisible, based on bool value.
        /// </summary>
        private void SetEdgeHighlight(Image edge, bool highlighted) {
            edge.color = highlighted ? HighlightColors.edges : invisible;
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
