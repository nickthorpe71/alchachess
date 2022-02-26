using System.Collections.Generic;

namespace Data
{
    public class PlayerData
    {
        private readonly PlayerToken _playerToken;
        private readonly PieceColor _pieceColor;
        private readonly bool _hasWon;
        private readonly bool _isAI;
        private readonly List<Piece> _pieces;

        public PlayerData(PlayerToken playerToken, PieceColor pieceColor, bool hasWon, bool isAI, List<Piece> pieces)
        {
            _playerToken = playerToken;
            _pieceColor = pieceColor;
            _hasWon = hasWon;
            _isAI = isAI;
            _pieces = pieces;
        }

        public PlayerToken PlayerToken { get { return _playerToken; } }
        public PieceColor PieceColor { get { return _pieceColor; } }
        public bool HasWon { get { return _hasWon; } }
        public bool IsAI { get { return _isAI; } }
        public List<Piece> Pieces { get { return _pieces; } }

    }

    public enum PlayerToken
    {
        P1,
        P2,
        NA
    }

}