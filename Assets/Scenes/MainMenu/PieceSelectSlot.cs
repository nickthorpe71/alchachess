using UnityEngine;
using System.Collections.Generic;

public class PieceSelectSlot : MonoBehaviour
{
    [SerializeField] GameObject kaidoModel;
    [SerializeField] GameObject luffyModel;
    [SerializeField] GameObject shanksModel;
    [SerializeField] GameObject whiteBeardModel;

    private List<GameObject> allModels;

    private void Awake()
    {
        allModels = new List<GameObject>() {
            kaidoModel,
            luffyModel,
            shanksModel,
            whiteBeardModel
        };
    }

    public void EnableModel(string pieceName)
    {
        DeactivateAllModels();

        switch (pieceName)
        {
            case "Kaido":
                kaidoModel.SetActive(true);
                break;
            case "Luffy":
                luffyModel.SetActive(true);
                break;
            case "Shanks":
                shanksModel.SetActive(true);
                break;
            case "WhiteBeard":
                whiteBeardModel.SetActive(true);
                break;
        }
    }

    public void NextRight()
    {
        RotateModels(1);
    }

    public void NextLeft()
    {
        RotateModels(-1);
    }


    private void RotateModels(int direction)
    {
        int activeIndex = allModels.FindIndex(a => a.activeSelf);
        allModels[activeIndex].SetActive(false);

        int newIndex = activeIndex + direction;

        if (newIndex > allModels.Count)
            newIndex = 0;
        else if (newIndex < 0)
            newIndex = allModels.Count - 1;

        allModels[newIndex].SetActive(true);
    }

    private void DeactivateAllModels()
    {
        allModels.ForEach(model => model.SetActive(false));
    }
}
