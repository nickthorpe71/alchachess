using System;
using UnityEngine;

namespace Logic
{
    public class PlayerInput
    {
        private Vector2 savedClick;
        private Vector2 savedHover;
        private int tileLayerMask = LayerMask.GetMask("Tile");

        private Game game;

        public PlayerInput(Game game)
        {
            savedClick = NullV2();
            savedHover = NullV2();
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
                if (hit.transform.tag == "Tile")
                    onEvent(hit.transform.gameObject);
        }

        private void OnClick(GameObject obj)
        {
            Vector2 newClick = new Vector2(obj.transform.position.x, obj.transform.position.z);
            GameObject clickedTileObj = game.board.GetTile(newClick);
            Tile clickedTile = clickedTileObj.GetComponent<Tile>();

            // if we are clicking on the tile already selected
            if (newClick == savedClick)
            {
                savedClick = NullV2();
                clickedTile.Click();
                clickedTile.Hover();
            }
            // if we are clicking on a new tile we don't currently have a tile selected
            else if (savedClick == NullV2())
            {
                savedClick = newClick;
                GameObject currentTile = clickedTileObj;
                clickedTile.Click();
                // List<Vector2> possibleMoves = currentTile.GetComponent<Tile>().piece.PossibleMoves(game.board, newClick);
                // Debug.Log(possibleMoves);
            }
            // if we are clicking on a new tile and we have a tile selected
            else
            {
                // game.SubmitMove(start: savedClick, end: newClick);

                // reset savedious tile
                game.board.GetTile(savedClick)
                    .GetComponent<Tile>()
                    .ResetEffects(removeClick: true);

                savedClick = newClick;
                clickedTile.Click();
            }
        }

        private void OnHover(GameObject obj)
        {
            if (obj == null) return;

            Vector2 newHover = new Vector2(obj.transform.position.x, obj.transform.position.z);
            Tile hoveredTile = game.board.GetTile(newHover).GetComponent<Tile>();

            // if we are hovering on the same thing as before
            if (newHover != NullV2() && newHover == savedHover)
                return;

            // if nothing has been hovered yet
            if (savedHover != NullV2())
            {
                game.board.GetTile(savedHover).GetComponent<Tile>().ResetEffects();
            }

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

        private Vector2 NullV2() => new Vector2(-1, -1);
    }
}

