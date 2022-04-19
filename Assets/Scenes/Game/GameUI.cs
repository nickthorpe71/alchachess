using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public GameObject winLoseObj;
    private TextMeshProUGUI winLoseText;

    private void Awake()
    {
        winLoseText = winLoseObj.GetComponent<TextMeshProUGUI>();
    }

    public void DisplayWin()
    {
        winLoseText.text = "YOU WIN!";
        winLoseObj.SetActive(true);
    }

    public void DisplayLose()
    {
        winLoseText.text = "YOU LOSE";
        winLoseObj.SetActive(true);
    }
}
