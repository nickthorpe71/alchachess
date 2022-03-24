using UnityEngine;

public class Move
{
    private readonly GenericPlayer _player;
    public GenericPlayer player { get { return _player; } }
    private readonly GameObject _start;
    public GameObject start { get { return _start; } }
    private readonly GameObject _end;
    public GameObject end { get { return _end; } }

    public Move(GenericPlayer player, GameObject start, GameObject end)
    {
        _player = player;
        _start = start;
        _end = end;
    }
}