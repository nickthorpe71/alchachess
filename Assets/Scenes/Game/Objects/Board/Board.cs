using System;
using System.Linq;
using UnityEngine;

namespace Objects
{
    public class Board
    {
        private Tile[] _tiles;
        private readonly int _width;
        private readonly int _height;

        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            Reset();
        }

        public Tile GetTile(Vector2 v2)
        {
            try
            {
                return _tiles[(int)(_width * v2.x + v2.y)];
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new ArgumentException($"Tile position out of bounds x:{v2.x} y:{v2.y} Exception:{exception}");
            }
        }

        public string AsString() => string.Join("", _tiles.Select(tile => $"{tile.AsString()}\n"));

        public void Reset()
        {
            _tiles = new Tile[_width * _height];

            FillGoldSide();
            FillMidBoard();
            FillBlackSide();
        }

        private void FillGoldSide()
        {
            _tiles[0] = new Tile(0, 0, new Demon(isGold: true), new Blue());
            _tiles[1] = new Tile(1, 0, new Witch(isGold: true), new Black());
            _tiles[2] = new Tile(2, 0, new Wraith(isGold: true), new Red());
            _tiles[3] = new Tile(3, 0, new GodOfLife(isGold: true), new White());
            _tiles[4] = new Tile(4, 0, new Witch(isGold: true), new Yellow());
            _tiles[5] = new Tile(5, 0, new Demon(isGold: true), new Green());

            _tiles[6] = new Tile(0, 1, new Gargoyle(isGold: true), new Yellow());
            _tiles[7] = new Tile(1, 1, new Gargoyle(isGold: true), new White());
            _tiles[8] = new Tile(2, 1, new Gargoyle(isGold: true), new Green());
            _tiles[9] = new Tile(3, 1, new Gargoyle(isGold: true), new Red());
            _tiles[10] = new Tile(4, 1, new Gargoyle(isGold: true), new Blue());
            _tiles[11] = new Tile(5, 1, new Gargoyle(isGold: true), new Black());
        }

        private void FillMidBoard()
        {
            _tiles[12] = new Tile(0, 2, null, new Green());
            _tiles[13] = new Tile(1, 2, null, new Blue());
            _tiles[14] = new Tile(2, 2, null, new Yellow());
            _tiles[15] = new Tile(3, 2, null, new Black());
            _tiles[16] = new Tile(4, 2, null, new White());
            _tiles[17] = new Tile(5, 2, null, new Red());

            _tiles[18] = new Tile(0, 3, null, new Red());
            _tiles[19] = new Tile(3, 3, null, new White());
            _tiles[20] = new Tile(3, 3, null, new Black());
            _tiles[21] = new Tile(3, 3, null, new Yellow());
            _tiles[22] = new Tile(3, 3, null, new Blue());
            _tiles[23] = new Tile(3, 3, null, new Green());
        }

        private void FillBlackSide()
        {
            _tiles[24] = new Tile(0, 4, new Gargoyle(isGold: false), new Black());
            _tiles[25] = new Tile(1, 4, new Gargoyle(isGold: false), new Blue());
            _tiles[26] = new Tile(2, 4, new Gargoyle(isGold: false), new Red());
            _tiles[27] = new Tile(3, 4, new Gargoyle(isGold: false), new Green());
            _tiles[28] = new Tile(4, 4, new Gargoyle(isGold: false), new White());
            _tiles[29] = new Tile(5, 4, new Gargoyle(isGold: false), new Yellow());

            _tiles[30] = new Tile(0, 5, new Demon(isGold: false), new Green());
            _tiles[31] = new Tile(1, 5, new Witch(isGold: false), new Yellow());
            _tiles[32] = new Tile(2, 5, new Wraith(isGold: false), new White());
            _tiles[33] = new Tile(3, 5, new GodOfLife(isGold: false), new Red());
            _tiles[34] = new Tile(4, 5, new Witch(isGold: false), new Black());
            _tiles[35] = new Tile(5, 5, new Demon(isGold: false), new Blue());
        }

        public int width { get { return _width; } }
        public int height { get { return _height; } }
    }
}
