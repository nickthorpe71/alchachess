using UnityEngine;

public class ElementGraphic : MonoBehaviour
{
    public GameObject destroyAnim;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Piece")
        {
            Instantiate(destroyAnim, transform.position, Quaternion.identity);
            Destroy(this);
        }
    }
}
