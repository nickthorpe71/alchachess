using System;
using UnityEngine;

namespace Logic
{
    public class PlayerInput
    {
        private Vector2 prevClick;
        private Vector2 prevHover;
        private int tileLayerMask = LayerMask.GetMask("Tile");

        private Game game;

        public PlayerInput(Game game)
        {
            prevClick = NullV2();
            prevHover = NullV2();
            this.game = game;
        }

        public void HandleInput()
        {
            HandleClick();
            // HandleHover();
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

            // if we are clicking on the tile already selected
            if (newClick == prevClick)
            {
                prevClick = NullV2();
            }
            // if we are clicking on a new tile we don't currently have a tile selected
            else if (prevClick.x == -1)
            {
                prevClick = newClick;
                GameObject currentTile = game.board.GetTile(newClick);
                // List<Vector2> possibleMoves = currentTile.GetComponent<Tile>().piece.PossibleMoves(game.board, newClick);
                // Debug.Log(possibleMoves);
            }
            // if we are clicking on a new tile and we have a tile selected
            else
            {
                game.SubmitMove(start: prevClick, end: newClick);
                prevClick = NullV2();
            }
        }

        private void OnHover(GameObject obj)
        {
            Debug.Log($"Hovered: {obj.name}");
        }

        private Vector2 NullV2() => new Vector2(-1, -1);
    }
}

