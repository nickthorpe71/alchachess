using System;
using UnityEngine;

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
        Tile clickedTile = BoardCalculation.GetTile(game.board.boardData, newClick);

        // if we are clicking on the tile already selected
        if (newClick == savedClick)
        {
            game.board.SetHighlightedMoves(savedClick, deactivate: true);
            savedClick = nullV2;
            clickedTile.Click(deactivate: true);
        }
        // if we are clicking on a new tile we don't currently have a tile selected
        else if (savedClick == nullV2)
        {
            if (clickedTile.HasPlayersPiece(game.gameData.currentTurn))
            {
                game.board.SetHighlightedMoves(newClick);
                savedClick = newClick;
                clickedTile.Click();
            }
        }
        // if we are clicking on a new tile and we have a tile selected
        else
        {
            // reset saved tile
            Tile savedTile = BoardCalculation.GetTile(game.board.boardData, savedClick);
            savedTile.Click(deactivate: true);
            savedTile.Hover(deactivate: true);
            game.board.SetHighlightedMoves(savedClick, deactivate: true);

            if (clickedTile.HasPlayersPiece(game.gameData.currentTurn))
            {
                game.board.SetHighlightedMoves(newClick);
                clickedTile.Click();
                savedClick = newClick;
            }
            else
            {
                if (savedTile.HasPlayersPiece(game.gameData.currentTurn))
                    game.SubmitMove(start: savedClick, end: newClick);

                savedClick = nullV2;
            }
        }
    }

    private void OnHover(GameObject obj)
    {
        Vector2 newHover = new Vector2(obj.transform.position.x, obj.transform.position.z);
        Tile hoveredTile = BoardCalculation.GetTile(game.board.boardData, newHover);

        // if we are hovering on the same thing as before
        if (newHover != nullV2 && newHover == savedHover)
            return;

        // if hovering something new
        DeactivateSavedHover();

        hoveredTile.Hover();
        savedHover = newHover;

        if (hoveredTile.IsHighlighted())
            game.board.SetAOEMarkers(newHover);
    }

    private void DeactivateSavedHover()
    {
        if (savedHover != nullV2)
        {
            Tile tile = BoardCalculation.GetTile(game.board.boardData, savedHover);
            tile.Hover(deactivate: true);
            game.board.SetAOEMarkers(savedHover, deactivate: true);
            savedHover = nullV2;
        }
    }
}

