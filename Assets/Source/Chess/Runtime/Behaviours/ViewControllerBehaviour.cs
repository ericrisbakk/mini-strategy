using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Steps;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using UnityEngine.Assertions;
using static Source.Chess.Runtime.ChessConstants;
using Checks = Source.Chess.Runtime.ChessChecks;

namespace Source.Chess.Runtime.Behaviours {
    public class ViewControllerBehaviour : SerializedMonoBehaviour {

        #region Variables

        public BoardBehaviour board;
        public PromotionSelectBehaviour promotionSelect;
        public BoardConfiguration startingConfiguration;
        public GameObject piecesParent;
        public Dictionary<PieceType, GameObject> piecePrefabDict;

        #region State
        
        [NonSerialized] public GameState State;
        [NonSerialized] public LinearHistory History;
        [NonSerialized] public char targetRank;
        [NonSerialized] public char targetFile;
        [NonSerialized] public bool clicked;
        [NonSerialized] public int viewActionCount;

        #endregion
        
        #region Lambdas
        private Vector2Int ToInternalIndex(Vector2Int vec) => new Vector2Int(vec.x - 2, vec.y - 2);
        private Vector2Int ToInternalIndex(char rank, char file) => new Vector2Int(rank - '1', file - 'a');
        private bool EqualRankAndFile(char rank, char file) => rank == targetRank && file == targetFile;
        
        #endregion

        #endregion
        
        private void SetRankAndFile(char rank, char file) {
            targetRank = rank;
            targetFile = file;
            Debug.Log($"Target is: {targetFile}{targetRank}.");
        }

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

        #region Controller

        /// <summary>
        /// Using the `startingConfiguration`, place black and white pieces on the board.
        /// </summary>
        public void SetupGame() {
            State = new GameState(startingConfiguration.white, startingConfiguration.black);
            History = new LinearHistory();
            for (char rank = '1'; rank < '9'; rank++) {
                for (char file = 'a'; file < 'i'; file++) {
                    var piece = State.Square(Rules.ToVector2Int(rank, file));
                    if ((int) piece >= 2)
                        AddPiece(piece, ToInternalIndex(rank, file));
                }
            }
        }

        /// <summary>
        /// Adds piece GO at target location. Asserts that there is no piece at that location from before.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="target"></param>
        public void AddPiece(PieceType piece, Vector2Int target) {
            var square = board.Squares[target.x,  target.y];
            var go = Instantiate(piecePrefabDict[piece], square.transform);
            go.transform.localPosition = Vector3.zero;
            Assert.IsNull(board.Pieces[target.x, target.y]);
            board.Pieces[target.x, target.y] = go;
        }
        
        /// <summary>
        /// Moves a piece GO from location A to B. Asserts that the target location is empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void MovePiece(Vector2Int source, Vector2Int target) {
            var go = board.Pieces[source.x, source.y];
            var newParent = board.Squares[target.x, target.y];

            board.Pieces[source.x, source.y] = null;
            Assert.IsNull(board.Pieces[target.x, target.y]);
            board.Pieces[target.x, target.y] = go;
            go.transform.SetParent(newParent.transform, false);
        }

        /// <summary>
        /// Destroy piece at target location.
        /// </summary>
        /// <param name="target"></param>
        public void DestroyPiece(Vector2Int target) {
            Destroy(board.Pieces[target.x, target.y]);
            board.Pieces[target.x, target.y] = null;
        }

        
        /// <summary>
        /// Behaviour when the player clicks on a square on the board.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="file"></param>
        private void OnPointerClick(char rank, char file) {
            if (clicked) {
                if (EqualRankAndFile(rank, file)) {
                    clicked = false;
                }
                else {
                    var highlights = GetHighlightsFromTarget(targetRank, targetFile);
                    if (ClickedOnHighlight(rank, file, highlights, out var action)) {
                        Debug.Log($"Action selected: {action}, at {file}{rank}.");
                        UpdateState(action);
                        board.Highlight(GetHighlightsFromTarget(rank, file));
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

        /// <summary>
        /// Called by the `PromotionSelectBehaviour` after the player has selected something to promote.
        /// </summary>
        /// <param name="promotion"></param>
        public void Promote(PieceType promotion) {
            UpdateState(new Promote(State.CurrentPlayer, State.PromotionTarget, promotion));
            Debug.Log($"Promoted pawn to {State.Square(State.PromotionTarget)}.");
        }

        /// <summary>
        /// Called to update the state, then use the resulting steps to update the view.
        /// </summary>
        /// <param name="action"></param>
        private void UpdateState(IAction action) {
            Rules.Apply(State, History, action, true);
            var steps = History.LastAction.Item2;
            UpdateView(steps);
            board.ClearHighlight();
            clicked = false;
        }
        
        #endregion

        #region View
        
        /// <summary>
        /// Updated the view based on step information - additionally checks whether to allow the player to promote.
        /// </summary>
        /// <param name="steps"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateView(List<IStep> steps) {
            foreach (var step in steps) {
                switch (step) {
                    case MoveStep moveStep: HandleMoveStep(moveStep);
                        break;
                    case ChangePlayerStep changePlayerStep: HandleChangePlayerStep(changePlayerStep);
                        break;
                    case PromotionStep promotionStep: HandlePromotionStep(promotionStep);
                        break;
                    case EnPassantStep enPassantStep: HandleEnPassantStep(enPassantStep);
                        break;
                    default:
                        throw new Exception("Couldn't match step when trying to update view.");
                }
            }

            if (State.PromotionNeeded) {
                promotionSelect.PresentPromotions(State.CurrentPlayer.Color);
            }
        }
        
        private void HandleChangePlayerStep(ChangePlayerStep changePlayerStep) {
            Debug.Log($"Current player is: {State.CurrentPlayer.Color}");
        }

        private void HandleEnPassantStep(EnPassantStep enPassantStep) {
            var source = ToInternalIndex(enPassantStep.EnPassant.Source);
            var target = ToInternalIndex(enPassantStep.EnPassant.Target);
            var capture = ToInternalIndex(enPassantStep.EnPassant.Capture);
            
            DestroyPiece(capture);
            MovePiece(source, target);
            
        }

        private void HandlePromotionStep(PromotionStep promotionStep) {
            var promotion = promotionStep.Promote.Promotion;
            var target = ToInternalIndex(promotionStep.Promote.Pawn);
            
            DestroyPiece(target);
            AddPiece(promotion, target);
        }

        private void HandleMoveStep(MoveStep step) {
            var source = ToInternalIndex(step.Move.Source);
            var target = ToInternalIndex(step.Move.Target);
            
            if (Checks.MoveCaptures(step.Move))
                DestroyPiece(target);

            MovePiece(source, target);
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
        


        /// <summary>
        /// Gets all the actions for a given piece, then sorts them based on what kind of highlight they are.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public Dictionary<HighlightType, List<Tuple<IAction, Vector2Int>>> GetHighlightsFromTarget(char rank, char file) {
            var target = Rules.ToVector2Int(rank, file);
            var actions = Rules.GetActions(State, History, target);
            var highlights = GetHighlightDict();
            AddHighlight(highlights, HighlightType.Selected, null, target);
            foreach (var action in actions) {
                switch (action) {
                    case Move move:
                        if (Checks.MoveCaptures(move))
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
