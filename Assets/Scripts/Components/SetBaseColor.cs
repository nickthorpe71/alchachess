using UnityEngine;
using System.Collections.Generic;

public class SetBaseColor : MonoBehaviour
{
    public List<MeshRenderer> baseMats;

    public void SetColor(Material mat)
    {
        baseMats.ForEach(mesh => { mesh.material = mat; });
    }
}
