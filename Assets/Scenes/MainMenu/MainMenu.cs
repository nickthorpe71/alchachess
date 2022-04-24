using UnityEngine;
using UnityCore.Audio;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AudioController.instance.Play(UnityCore.Audio.AudioType.MUSIC_MAIN_MENU, true, 1);
        // attempt to load player data
        // send player pieces to piece selector
    }

    public void LoadAI()
    {
        // TODO: Add difficulty selection etc
        AudioController.instance.Play(UnityCore.Audio.AudioType.UI_CLICK, _volume: 0.35f);
        GlobalSceneLoader.LoadScene("Game");
    }

    public void QuitGame()
    {
        AudioController.instance.Play(UnityCore.Audio.AudioType.UI_CLICK, _volume: 0.35f);
        Application.Quit();
    }
}
