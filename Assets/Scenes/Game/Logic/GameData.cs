using System.Collections.Generic;

public class GameData
{
    public GenericPlayer[] players { get; set; }
    public GenericPlayer currentTurn { get; set; }
    public GameStatus status { get; private set; }
    private List<MoveData> _movesPlayed;

    public GameData(GenericPlayer p1, GenericPlayer p2)
    {
        players = new GenericPlayer[2];
        players[0] = p1;
        players[1] = p2;
        currentTurn = p1;
        _movesPlayed = new List<MoveData>();
    }

    public void SetStatus(GameStatus newStatus)
    {
        status = newStatus;
    }

    public void AddPlayedMove(MoveData newMove)
    {
        _movesPlayed.Add(newMove);
    }
}
