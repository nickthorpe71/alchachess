using UnityEngine;

public class ElementGraphic : MonoBehaviour
{
    [HideInInspector] public GameObject destroyAnimPrefab;
    [HideInInspector] public Graphics graphics;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Piece")
            Deactivate();
    }

    public void Deactivate()
    {
        GameObject destroyAnim = Instantiate(destroyAnimPrefab, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        GameObject destroyAnim = Instantiate(destroyAnimPrefab, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        gameObject.SetActive(true);
    }
}
