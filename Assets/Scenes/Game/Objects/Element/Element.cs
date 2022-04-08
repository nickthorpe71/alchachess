using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Logic;

public abstract class Element : MonoBehaviour
{
    public bool destroysOccupant { get; protected set; }
    public bool hasKnockback { get; protected set; }
    public List<Vector2> spellPattern { get; protected set; }
    public string color { get; private set; }
    private GameObject _destroyAnimPrefab;
    public GameObject spellAnim { get; private set; }
    private GameObject _castAnim;
    private GameObject _graphic;
    private Board _board;

    public void Init(Board board, string color)
    {
        _board = board;
        this.color = color;
        _destroyAnimPrefab = Resources.Load($"Element/DestroyAnimations/{color}DestroyAnim") as GameObject;
        spellAnim = Resources.Load($"Element/SpellAnims/{color}SpellAnim") as GameObject;
        _castAnim = Resources.Load($"Element/CastAnims/{color}CastAnim") as GameObject;
        _graphic = Helpers.FindComponentInChildWithTag<Transform>(gameObject, "Graphic").gameObject;
    }

    private IEnumerator Cast()
    {
        _board.SpawnAnim(_castAnim, new Vector3(transform.position.x, 0.45f, transform.position.z), 2);
        yield return new WaitForSeconds(1.2f);
        _board.CastSpell(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_graphic.activeSelf) return;

        if (other.gameObject.tag == "Piece")
        {
            Deactivate();
            StartCoroutine(Cast());
        }
    }

    public void Deactivate()
    {
        if (!_graphic.activeSelf) return;

        GameObject destroyAnim = Instantiate(_destroyAnimPrefab, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        _graphic.SetActive(false);
    }

    public void Activate()
    {
        if (_graphic.activeSelf) return;
        Debug.Log("in");
        GameObject destroyAnim = Instantiate(_destroyAnimPrefab, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        _graphic.SetActive(true);
    }
}
