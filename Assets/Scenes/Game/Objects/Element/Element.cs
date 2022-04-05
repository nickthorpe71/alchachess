using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    public bool destroysOccupant { get; protected set; }
    public bool hasKnockback { get; protected set; }
    public GameObject environmentPrefab;
    public List<Vector2> spellPattern { get; protected set; }

    private GameObject _destroyAnimPrefab;

    protected void SetDestroyAnim(string elementName)
    {
        _destroyAnimPrefab = Resources.Load($"Element/DestroyAnimations/{elementName}DestroyAnim") as GameObject;
    }

    public abstract void Cast();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Piece")
            Deactivate();
    }

    public void Deactivate()
    {
        GameObject destroyAnim = Instantiate(_destroyAnimPrefab, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        GameObject destroyAnim = Instantiate(_destroyAnimPrefab, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        gameObject.SetActive(true);
    }

    public List<Vector2> GetSpellPattern(Board board, Vector2 pos)
    {
        return spellPattern
            .Select(target => new Vector2(target.x + pos.x, target.y + pos.y))
            .Where(target => board.IsInBounds(target)).ToList();
    }
}
