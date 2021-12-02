using System.Collections.Generic;

namespace Source.Chess.Runtime {
    
    public enum PieceType {
        Empty,
        OutOfBounds,
        WPawn,
        WKnight,
        WBishop,
        WRook,
        WQueen,
        WKing,
        BPawn,
        BKnight,
        BBishop,
        BRook,
        BQueen,
        BKing
    }
    
    // TODO: Rules should probably inherit from something defining base classes, especially a "GetAllAvailableActions" method.
    public class Rules {
        
    }
}