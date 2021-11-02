using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Source.MiniStategy.Runtime {
    /// <summary>
    /// Defines the behaviour of the game, as well as querying methods.
    /// </summary>
    public class GameRulesDefinition
    {
        #region ActionRules

        public static void ApplyMove(GameStateDefinition state, GameActionsDefinition.Move move) {
            var fromTile = GetTileData(state, move.From);
            var toTile = GetTileData(state, move.To);

            fromTile.RemovePiece(move.Target);
            toTile.AddPiece(move.Target);
        }

        #endregion

        #region Querying

        public static Piece GetPieceAt(GameStateDefinition state, Vector2Int pos, Piece piece) {
            return GetTileData(state, pos).Pieces[piece.ID];
        }
        
        public static GameTileData GetTileData(GameStateDefinition state, Vector2Int pos) {
            return (GameTileData) state.Map.GetFace(pos).Data;
        }
        
        #endregion
    }
}
