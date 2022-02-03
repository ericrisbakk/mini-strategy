using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Source.Chess.Runtime.Actions;
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
        [NonSerialized] public Vector2Int clickedTarget;
        [NonSerialized] public bool clicked;

        #endregion

        #endregion

        private void Awake() {
            SetupGame();
            foreach (var s in board.Squares) {
                s.OnPointerClick -= OnPointerClick;
                s.OnPointerClick += OnPointerClick;
                s.OnPointerEnter -= OnPointerEnter;
                s.OnPointerEnter += OnPointerEnter;
                s.OnPointerExit -= OnPointerExit;
                s.OnPointerExit += OnPointerExit;
            }
        }

        public void SetupGame() {
            State = new GameState(startingConfiguration.white, startingConfiguration.black);
            History = new LinearHistory();
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

        #region Controller
        
        // TODO: Clicking on somewhere not highlighted while having clicked should set that to the new target.
        private void OnPointerClick(Vector2Int target) {
            if (clicked) {
                if (target == clickedTarget) {
                    clicked = false;
                }
                else {
                    // clickedTarget = target;
                }
            }
            else {
                clicked = true;
                clickedTarget = target;
            }
        }

        private void OnPointerEnter(Vector2Int target) {
            if (!clicked) { 
                board.Highlight(GetHighlightsFromTarget(target));
            }
        }
        
        private void OnPointerExit(Vector2Int target) {
            if (!clicked) { 
                board.ClearHighlight();
            }
        }

        public Dictionary<HighlightType, List<Vector2Int>> GetHighlightsFromTarget(Vector2Int target) {
            var actions = Rules.GetActions(State, History, target);
            var highlights = GetHighlightDict();
            highlights[HighlightType.Selected].Add(target);
            foreach (var action in actions) {
                switch (action) {
                    case Move move:
                        if (Rules.MoveCaptures(move))
                            highlights[HighlightType.Capture].Add(move.Target);
                        else
                            highlights[HighlightType.Move].Add(move.Target);
                        break;
                    case EnPassant enPassant:
                        highlights[HighlightType.EnPassant].Add(enPassant.Target);
                        break;
                    case Promote promote:
                        highlights[HighlightType.Promotion].Add(promote.Pawn);
                        break;
                    case Castle castle:
                        highlights[HighlightType.Castle].Add(castle.Rook);
                        break;
                }
            }

            return highlights;
        }

        private Dictionary<HighlightType, List<Vector2Int>> GetHighlightDict() {
            var dict = new Dictionary<HighlightType, List<Vector2Int>>();
            foreach (int value in Enum.GetValues(typeof(HighlightType))) {
                dict[(HighlightType) value] = new List<Vector2Int>();
            }

            return dict;
        }
        
        #endregion

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
