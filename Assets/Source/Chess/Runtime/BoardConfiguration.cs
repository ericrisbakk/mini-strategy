using UnityEngine;

namespace Source.Chess.Runtime {
    /// <summary>
    /// The starting configuration for a game of chess. Uses algebraic notation for white and black pieces.
    /// </summary>
    [CreateAssetMenu(menuName = "Chess/Board Configuration")]
    public class BoardConfiguration : ScriptableObject {
        public string white;
        public string black;
    }
}