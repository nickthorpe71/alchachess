using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadAI()
    {
        // TODO: Add difficulty selection etc
        Context.instance.LoadScene("Game");
    }
}
