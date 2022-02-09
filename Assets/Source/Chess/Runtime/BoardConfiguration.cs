using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Chess.Runtime {
    /// <summary>
    /// The starting configuration for a game of chess. Uses algebraic notation for white and black pieces.
    /// </summary>
    [CreateAssetMenu(menuName = "Chess/Board Configuration")]
    public class BoardConfiguration : ScriptableObject {
        [InfoBox("Use algebraic notation to denote pieces, separate using ',' with no spaces, e.g.:\n" +
                 "h2,Ra1,Nb1,Bc1,Qd1,Ke1")]
        public string white;
        public string black;
    }
}