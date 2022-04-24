using UnityEngine;
public class UserState : MonoBehaviour
{
    private GenericPlayer localPlayer;
    private GenericPlayer opponentPlayer;

    public UserState()
    {
        localPlayer = new HumanPlayer(true, true);
    }

    public UserState(GenericPlayer loadedPlayer)
    {
        localPlayer = loadedPlayer;
    }

    public void SetLocalPlayer(GenericPlayer localPlayer)
    {
        this.localPlayer = localPlayer;
    }

    public void SetOpponentPlayer(GenericPlayer opponentPlayer)
    {
        this.opponentPlayer = opponentPlayer;
    }
}
