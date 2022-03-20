using UnityEngine;
using Objects;

public class LifeCycle : MonoBehaviour
{
    private Game game;
    private GenericPlayer p1;
    private GenericPlayer p2;

    void Start()
    {
        p1 = new HumanPlayer(goldSide: true);
        p1 = new AIPlayer(goldSide: false);
        game = new Game(p1, p2);

        game.SetStatus(GameStatus.ACTIVE);

        Debug.Log(game.board.AsString());
    }

    void Update()
    {
        PlayerInput();
    }
}
