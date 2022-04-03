using UnityEngine;
using Logic;

public class LifeCycle : MonoBehaviour
{
    private Game game;
    private GenericPlayer p1;
    private GenericPlayer p2;

    // Systems
    private PlayerInput inputSystem;
    private Graphics graphics;

    void Awake()
    {
        p1 = new HumanPlayer(goldSide: true);
        p1 = new AIPlayer(goldSide: false);

        game = new Game(p1, p2, GetComponent<Board>());
        game.SetStatus(GameStatus.ACTIVE);

        inputSystem = new PlayerInput(game);
    }

    void Start()
    {
        game.board.Init(this);
    }

    void Update()
    {
        inputSystem.HandleInput();
    }

    public GameObject Spawn(string objPath, Vector3 pos, Quaternion rot)
    {
        return Instantiate(Resources.Load(objPath) as GameObject, pos, rot);
    }
}
