using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    protected bool destroysOccupant;
    protected bool hasKnockback;
    protected List<Vector2> spellPattern;
    protected string color;
    private string _destroyAnimPath;
    public string spellAnimPath { get; private set; }
    private string _castAnimPath;
    private GameObject _graphic;
    private SphereCollider _sphereCollider;
    private Game game;

    public void Init(Game game, string color)
    {
        this.game = game;
        _sphereCollider = GetComponent<SphereCollider>();
        this.color = color;
        _destroyAnimPath = $"Element/DestroyAnimations/{color}DestroyAnim";
        spellAnimPath = $"Element/SpellAnims/{color}SpellAnim";
        _castAnimPath = $"Element/CastAnims/{color}CastAnim";
        _graphic = Helpers.FindComponentInChildWithTag<Transform>(gameObject, "Graphic").gameObject;
    }
    public ElementData GetData()
    {
        return new ElementData(
            destroysOccupant,
            hasKnockback,
            spellPattern,
            color
        );
    }
    public bool DestroysOccupant() => destroysOccupant;
    public bool HasKnockback() => hasKnockback;
    public List<Vector2> GetSpellPattern() => spellPattern;
    public string GetColor() => color;
    public bool IsActive() => _graphic.activeSelf;

    private IEnumerator Cast(Piece caster)
    {
        GameObject castAnim = game.Spawn(
            _castAnimPath,
            new Vector3(transform.position.x, 0.45f, transform.position.z),
            Quaternion.identity);
        Destroy(castAnim, 2);

        yield return new WaitForSeconds(1.2f);

        game.board.CastSpell(this, caster);
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
            GameObject destroyAnim = game.Spawn(_destroyAnimPath, transform.position, Quaternion.identity);
            Destroy(destroyAnim, 2);
        }
    }

    public void Activate()
    {
        if (_graphic.activeSelf) return;

        GameObject destroyAnim = game.Spawn(_destroyAnimPath, transform.position, Quaternion.identity);
        Destroy(destroyAnim, 2);
        _sphereCollider.enabled = true;
        _graphic.SetActive(true);
    }
}
