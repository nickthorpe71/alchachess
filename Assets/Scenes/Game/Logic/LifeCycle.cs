using UnityEngine;
using Objects;
using Logic;

public class LifeCycle : MonoBehaviour
{
    private Game game;
    private GenericPlayer p1;
    private GenericPlayer p2;

    private PlayerInput inputSystem;

    void Start()
    {
        p1 = new HumanPlayer(goldSide: true);
        p1 = new AIPlayer(goldSide: false);

        game = new Game(p1, p2);
        game.SetStatus(GameStatus.ACTIVE);

        inputSystem = new PlayerInput();

        Debug.Log(game.board.AsString());
    }

    void Update()
    {
        inputSystem.HandleInput(game);
    }
}
