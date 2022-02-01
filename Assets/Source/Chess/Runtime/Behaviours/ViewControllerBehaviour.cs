using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime.Behaviours {
    public class ViewControllerBehaviour : SerializedMonoBehaviour {

        #region Variables

        public BoardBehaviour board;
        public BoardConfiguration startingConfiguration;
        public GameObject piecesParent;
        public Dictionary<PieceType, GameObject> piecePrefabDict;

        #region State
        
        [NonSerialized] public GameState State;
        [NonSerialized] public LinearHistory History;

        #endregion

        #endregion

        private void Awake() {
            SetupGame();
        }

        public void SetupGame() {
            State = new GameState(startingConfiguration.white, startingConfiguration.black);
            for (char rank = '1'; rank < '9'; rank++) {
                for (char file = 'a'; file < 'i'; file++) {
                    var piece = State.Square(Rules.ToVector2Int(rank, file));
                    if ((int) piece >= 2)
                        AddPiece(piece, new string(new []{file, rank}));
                }
            }
        }

        public void AddPiece(PieceType piece, string algebraicNotation) {
            var x = algebraicNotation[1] - '1';
            var y = algebraicNotation[0] - 'a';
            var square = board.Squares[x,  y];
            var go = Instantiate(piecePrefabDict[piece], square.transform);
            go.transform.localPosition = Vector3.zero;
            board.Pieces[x, y] = go;
        }

#if UNITY_EDITOR
        private void Reset() {
            piecePrefabDict = new Dictionary<PieceType, GameObject>();
            for (int i = 0; i < 12; i++) {
                var piece = (PieceType) 2 + i;
                piecePrefabDict[piece] = null;
            }
        }
#endif
    }
}
