using UnityEngine;

namespace Logic
{
    public static class PlayerInput
    {
        static int tileLayerMask = LayerMask.GetMask("Tile");

        public static void HandleInput()
        {
            HandleClick();
            HandleHover();
        }

        private static void HandleClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                    if (hit.transform.tag == "Tile")
                        OnClick(hit.transform.gameObject);
            }
        }

        private static void HandleHover()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
                if (hit.transform.tag == "Tile")
                    OnHover(hit.transform.gameObject);
        }

        private static void OnClick(GameObject obj)
        {
            Debug.Log($"Clicked: {obj.name}");
        }

        private static void OnHover(GameObject obj)
        {
            Debug.Log($"Hovered: {obj.name}");
        }
    }
}

