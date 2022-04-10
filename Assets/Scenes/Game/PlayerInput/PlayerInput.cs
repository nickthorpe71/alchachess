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
            if (game.localPlayerCanInput)
            {
                HandleClick();
                HandleHover();
            }
            else if (savedHover.x > -1)
            {
                DeactivateSavedHover();
            }
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
                game.SetHighlightedMoves(savedClick, deactivate: true);
                savedClick = nullV2;
                clickedTile.Click(deactivate: true);
            }
            // if we are clicking on a new tile we don't currently have a tile selected
            else if (savedClick == nullV2)
            {
                if (clickedTile.HasPlayersPiece(game.currentTurn))
                {
                    game.SetHighlightedMoves(newClick);
                    savedClick = newClick;
                    clickedTile.Click();
                }
            }
            // if we are clicking on a new tile and we have a tile selected
            else
            {
                // reset saved tile
                Tile savedTile = game.board.GetTile(savedClick);
                savedTile.Click(deactivate: true);
                savedTile.Hover(deactivate: true);
                game.SetHighlightedMoves(savedClick, deactivate: true);

                if (clickedTile.HasPlayersPiece(game.currentTurn))
                {
                    game.SetHighlightedMoves(newClick);
                    clickedTile.Click();
                    savedClick = newClick;
                }
                else if (clickedTile.HasActiveElement())
                {
                    if (savedTile.HasPlayersPiece(game.currentTurn))
                        game.SubmitMove(start: savedClick, end: newClick);

                    savedClick = nullV2;
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

            if (hoveredTile.IsHighlighted())
                game.SetAOEMarkers(newHover);
        }

        private void DeactivateSavedHover()
        {
            if (savedHover != nullV2)
            {
                Tile tile = game.board.GetTile(savedHover);
                tile.Hover(deactivate: true);
                game.SetAOEMarkers(savedHover, deactivate: true);
                savedHover = nullV2;
            }
        }
    }
}

