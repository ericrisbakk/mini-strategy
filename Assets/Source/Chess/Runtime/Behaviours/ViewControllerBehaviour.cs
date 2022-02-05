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
        [NonSerialized] public char targetRank;
        [NonSerialized] public char targetFile;
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
        private void OnPointerClick(char rank, char file) {
            if (clicked) {
                if (EqualRankAndFile(rank, file)) {
                    clicked = false;
                }
                else {
                    var highlights = GetHighlightsFromTarget(targetRank, targetFile);
                    if (ClickedOnHighlight(rank, file, highlights, out var action)) {
                        Debug.Log($"Action selected: {action}, at {file}{rank}.");
                        clicked = false;
                    }
                    else {
                        Debug.Log($"No action at at {file}{rank}.");
                    }
                    
                }
            }
            else {
                clicked = true;
                SetRankAndFile(rank, file);
            }
        }

        private bool ClickedOnHighlight(char rank, char file,
            Dictionary<HighlightType, List<Tuple<IAction, Vector2Int>>> highlights, out IAction action) {
            var target = ToInternalIndex(Rules.ToVector2Int(rank, file));
            foreach (var pair in highlights) {
                foreach (var tuple in pair.Value) {
                    if (target == tuple.Item2) {
                        action = tuple.Item1;
                        return true;
                    }
                }
            }

            action = null;
            return false;
        }

        private void OnPointerEnter(char rank, char file) {
            if (!clicked) { 
                board.Highlight(GetHighlightsFromTarget(rank, file));
            }
        }
        
        private void OnPointerExit(char rank, char file) {
            if (!clicked) { 
                board.ClearHighlight();
            }
        }

        private bool EqualRankAndFile(char rank, char file) => rank == targetRank && file == targetFile;

        private void SetRankAndFile(char rank, char file) {
            targetRank = rank;
            targetFile = file;
            Debug.Log($"Target is: {targetFile}{targetRank}.");
        }

        public Dictionary<HighlightType, List<Tuple<IAction, Vector2Int>>> GetHighlightsFromTarget(char rank, char file) {
            var target = Rules.ToVector2Int(rank, file);
            var actions = Rules.GetActions(State, History, target);
            var highlights = GetHighlightDict();
            AddHighlight(highlights, HighlightType.Selected, null, target);
            foreach (var action in actions) {
                switch (action) {
                    case Move move:
                        if (Rules.MoveCaptures(move))
                            AddHighlight(highlights, HighlightType.Capture, action, move.Target);
                        else
                            AddHighlight(highlights, HighlightType.Move, action, move.Target);
                        break;
                    case EnPassant enPassant:
                        AddHighlight(highlights, HighlightType.EnPassant, action, enPassant.Target);
                        break;
                    case Promote promote:
                        AddHighlight(highlights, HighlightType.Promotion, action, promote.Pawn);
                        break;
                    case Castle castle:
                        AddHighlight(highlights, HighlightType.Castle, action, castle.Rook);
                        break;
                }
            }

            return highlights;
        }

        private void AddHighlight(Dictionary<HighlightType, List<Tuple<IAction, Vector2Int>>> dict, 
            HighlightType highlight, IAction action, Vector2Int vec) {
            dict[highlight].Add(new Tuple<IAction, Vector2Int>(action, ToInternalIndex(vec)));
        }
        
        private Vector2Int ToInternalIndex(Vector2Int vec) => new Vector2Int(vec.x - 2, vec.y - 2);

        private Dictionary<HighlightType, List<Tuple<IAction, Vector2Int>>> GetHighlightDict() {
            var dict = new Dictionary<HighlightType, List<Tuple<IAction, Vector2Int>>>();
            foreach (int value in Enum.GetValues(typeof(HighlightType))) {
                dict[(HighlightType) value] = new List<Tuple<IAction, Vector2Int>>();
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
