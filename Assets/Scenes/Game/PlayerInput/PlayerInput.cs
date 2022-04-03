using System;
using UnityEngine;

namespace Logic
{
    public class PlayerInput
    {
        private Vector2 savedClick;
        private Vector2 savedHover;
        private int tileLayerMask = LayerMask.GetMask("Tile");

        private Vector2 nullV2 = new Vector2(-1, -1);

        private Game game;

        public PlayerInput(Game game)
        {
            savedClick = nullV2;
            savedHover = nullV2;
            this.game = game;
        }

        public void HandleInput()
        {
            HandleClick();
            HandleHover();
        }

        private void HandleClick()
        {
            if (Input.GetMouseButtonDown(0))
                MouseAction(OnClick);
        }
        private void HandleHover()
        {
            MouseAction(OnHover);
        }
        private void MouseAction(Action<GameObject> onEvent)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
            {
                if (hit.transform.tag == "Tile")
                    onEvent(hit.transform.gameObject);
                else if (hit.transform.tag == "OffBoard")
                    DeactivateSavedHover();
            }
        }

        private void OnClick(GameObject obj)
        {
            Vector2 newClick = new Vector2(obj.transform.position.x, obj.transform.position.z);
            Tile clickedTile = game.board.GetTile(newClick);

            // if we are clicking on the tile already selected
            if (newClick == savedClick)
            {
                // reset highlighted moves
                game.ResetHighlights(savedClick);

                savedClick = nullV2;
                clickedTile.Click();
                clickedTile.Hover();
            }
            // if we are clicking on a new tile we don't currently have a tile selected
            else if (savedClick == nullV2)
            {
                savedClick = newClick;
                clickedTile.Click();
                if (clickedTile.HasPlayersPiece(game.currentTurn))
                    game.ShowMoves(newClick);
            }
            // if we are clicking on a new tile and we have a tile selected
            else
            {
                // reset saved tile
                game.board.GetTile(savedClick)
                    .GetComponent<Tile>()
                    .ResetEffects(removeClick: true);

                // reset highlighted moves
                game.ResetHighlights(savedClick);

                savedClick = newClick;
                clickedTile.Click();

                if (clickedTile.HasPlayersPiece(game.currentTurn))
                    game.ShowMoves(newClick);
                else if (clickedTile.HasActiveElement())
                {
                    Debug.Log("clicked element");
                    game.SubmitMove(start: savedClick, end: newClick);
                }
            }
        }

        private void OnHover(GameObject obj)
        {
            Vector2 newHover = new Vector2(obj.transform.position.x, obj.transform.position.z);
            Tile hoveredTile = game.board.GetTile(newHover);

            // if we are hovering on the same thing as before
            if (newHover != nullV2 && newHover == savedHover)
                return;

            // if hovering something new
            DeactivateSavedHover();

            hoveredTile.Hover();
            savedHover = newHover;

            // // if hovering a piece
            // if (currentHover.Contents == TileContents.Piece)
            // {
            //     ui.TurnOffCurrentGlow();
            //     ui.TogglePieceUIGlow(currentHover.Piece.Guid);

            //     if (localPlayerCanInput)
            //         ui.ToggleSpellUI(false);
            // }
            // else // if hovering an element
            // {
            //     ui.TurnOffCurrentGlow();

            //     // if piece is clicked and there are elements in our path
            //     if (currentClicked != null && currentHover.IsHighlighted)
            //     {
            //         Spell potentialSpell = SpellC.GetSpellByRecipe(BoardC.GetRecipeByPath(board, new Vector2(currentClicked.X, currentClicked.Y), new Vector2(currentHover.X, currentHover.Y)));
            //         if (potentialSpell != null)
            //         {
            //             // show stats of potential spell
            //             ui.UpdateSpellUI(potentialSpell, currentClicked.Piece);

            //             // show potential spell AOE
            //             board.tiles = BoardC.ChangeTilesState(
            //                 board.tiles,
            //                 new List<TileState> { TileState.isAOE },
            //                 true,
            //                 BoardC.CalculateAOEPatterns(potentialSpell.Pattern, currentHover, currentClicked.Piece.Player)
            //             );
            //         }
            //     }
            // }
        }

        private void DeactivateSavedHover()
        {
            if (savedHover != nullV2)
            {
                game.board.GetTile(savedHover).UnHover();
                savedHover = nullV2;
            }
        }
    }
}

