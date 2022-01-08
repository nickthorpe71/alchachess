using UnityEngine;

namespace Actions
{
    public static class Player
    {
        public static void HandleInput(GameLogic logic)
        {
            HandleClick(logic);
            HandleHover(logic);
        }

        public static void HandleClick(GameLogic logic)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                    if (hit.transform.tag == "Tile")
                        logic.TileClick(hit.transform.gameObject);
            }
        }

        public static void HandleHover(GameLogic logic)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
                if (hit.transform.tag == "Tile")
                    logic.TileHover(hit.transform.gameObject);
        }
    }
}

