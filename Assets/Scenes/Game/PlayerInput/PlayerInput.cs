using System;
using UnityEngine;
using Objects;

namespace Logic
{
    public class PlayerInput
    {
        private Vector2 prevClick;
        private Vector2 prevHover;
        private int tileLayerMask = LayerMask.GetMask("Tile");

        public PlayerInput()
        {
            prevClick = NullV2();
            prevHover = NullV2();
        }

        public void HandleInput(Game game)
        {
            HandleClick(game);
            // HandleHover();
        }

        private void HandleClick(Game game)
        {
            if (Input.GetMouseButtonDown(0))
                MouseAction(OnClick, game);
        }
        private void HandleHover(Game game)
        {
            MouseAction(OnHover, game);
        }
        private void MouseAction(Action<GameObject, Game> onEvent, Game game)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                if (hit.transform.tag == "Tile")
                    onEvent(hit.transform.gameObject, game);
        }

        private void OnClick(GameObject obj, Game game)
        {
            Vector2 newClick = new Vector2(obj.transform.position.x, obj.transform.position.z);

            // if we are clicking on the tile already selected
            if (newClick == prevClick)
            {
                Debug.Log("same same");
                Debug.Log(newClick);
                prevClick = NullV2();
            }
            // if we are clicking on a new tile we don't currently have a tile selected
            else if (prevClick.x == -1)
            {
                Debug.Log("new fresh");
                Debug.Log(newClick);
                prevClick = newClick;
            }
            // if we are clicking on a new tile and we have a tile selected
            else
            {
                game.SubmitMove(start: prevClick, end: newClick);
                Debug.Log($"start:{prevClick} end:{newClick}");
                prevClick = NullV2();
            }
        }

        private void OnHover(GameObject obj, Game game)
        {
            Debug.Log($"Hovered: {obj.name}");
        }

        private Vector2 NullV2() => new Vector2(-1, -1);
    }
}

