using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        // attempt to load player data
        // send player pieces to piece selector
    }

    public void LoadAI()
    {
        // TODO: Add difficulty selection etc
        GlobalSceneLoader.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
