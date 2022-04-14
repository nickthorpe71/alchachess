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
    public string spellAnim { get; private set; }
    private GameObject _castAnim;
    private GameObject _graphic;
    private SphereCollider _sphereCollider;
    private BoardData _board;

    public void Init(BoardData board, string color)
    {
        _board = board;
        _sphereCollider = GetComponent<SphereCollider>();
        this.color = color;
        _destroyAnimPrefab = Resources.Load($"Element/DestroyAnimations/{color}DestroyAnim") as GameObject;
        spellAnim = $"Element/SpellAnims/{color}SpellAnim";
        _castAnim = Resources.Load($"Element/CastAnims/{color}CastAnim") as GameObject;
        _graphic = Helpers.FindComponentInChildWithTag<Transform>(gameObject, "Graphic").gameObject;
    }
    public bool IsActive() => _graphic.activeSelf;

    private IEnumerator Cast(Piece caster)
    {
        // _board.SpawnAnim(_castAnim, new Vector3(transform.position.x, 0.45f, transform.position.z), 2);
        yield return new WaitForSeconds(1.2f);
        // _board.CastSpell(this, caster);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_graphic.activeSelf) return;

        if (other.gameObject.tag == "Piece")
        {
            Piece caster = other.GetComponent<Piece>();
            Deactivate();
            StartCoroutine(Cast(caster));
        }
    }

    public void Deactivate(bool playAnim = true)
    {
        if (!_graphic.activeSelf) return;
        _sphereCollider.enabled = false;
        _graphic.SetActive(false);

        if (playAnim)
        {
            GameObject destroyAnim = Instantiate(_destroyAnimPrefab, transform.position, Quaternion.identity);
            Destroy(destroyAnim, 2);
        }
    }

    public void Activate()
    {
        if (_graphic.activeSelf) return;

        GameObject destroyAnim = Instantiate(_destroyAnimPrefab, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        _sphereCollider.enabled = true;
        _graphic.SetActive(true);
    }
}
