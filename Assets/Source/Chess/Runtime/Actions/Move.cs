using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    /// <summary>
    /// Movement of a chess piece, which may cause a capture (No capture is given the "Empty" value).
    /// En-passant is included within the move rule.
    /// </summary>
    public class Move : IAction {
        public Player Player;
        public Vector2Int Source { get; }
        public Vector2Int Target { get; }
        public PieceType Piece;
        public PieceType Capture;

        public Move(Player player, PieceType piece, Vector2Int source, PieceType capture, Vector2Int target) {
            Player = player;
            Piece = piece;
            Source = source;
            Capture = capture;
            Target = target;
        }
        
        /// <summary>
        /// `source` and `target` converted to `Vector2Int` using `Rules.ToVector2Int()`
        /// </summary>
        public Move(Player player, PieceType piece, string source, PieceType capture, string target) {
            Player = player;
            Piece = piece;
            Source = Rules.ToVector2Int(source[1], source[0]);
            Capture = capture;
            Target = Rules.ToVector2Int(target[1], target[0]);
        }
        

        public bool Equals(Move other) {
            return Equals(
                Player, other.Player) 
                   && Piece == other.Piece 
                   && Capture == other.Capture 
                   && Source.Equals(other.Source) 
                   && Target.Equals(other.Target);
        }
    }
}