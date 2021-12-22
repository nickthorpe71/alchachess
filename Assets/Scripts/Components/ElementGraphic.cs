using UnityEngine;

public class ElementGraphic : MonoBehaviour
{
    [SerializeField] public GameObject destroyAnimPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Piece")
        {
            GameObject destroyAnim = Instantiate(destroyAnimPrefab, transform.position, Quaternion.identity);
            Destroy(destroyAnim, 2);
            gameObject.SetActive(false);
        }
    }
}
