using System.Collections.Generic;
using Data;

namespace Calc
{
    public static class TileC
    {
        public static Tile Clone(Tile tile)
        {
            return new Tile(
                tile.X,
                tile.Y,
                tile.Piece == null ? null : PieceC.Clone(tile.Piece),
                tile.Element,
                tile.Contents,
                tile.IsClicked,
                tile.IsHovered,
                tile.IsHighlighted,
                tile.IsAOE,
                tile.RemainingTimeOnEnvironment
            );
        }

        public static Tile RemovePiece(Tile tile)
        {
            return new Tile(
                tile.X,
                tile.Y,
                null,
                tile.Element,
                TileContents.Empty,
                tile.IsClicked,
                tile.IsHovered,
                tile.IsHighlighted,
                tile.IsAOE,
                tile.RemainingTimeOnEnvironment
            );
        }

        public static Tile UpdatePiece(Tile tile, Piece newPiece)
        {
            return new Tile(
                tile.X,
                tile.Y,
                newPiece,
                tile.Element,
                TileContents.Piece,
                tile.IsClicked,
                tile.IsHovered,
                tile.IsHighlighted,
                tile.IsAOE,
                tile.RemainingTimeOnEnvironment
            );
        }

        public static Tile UpdateStates(Tile tile, bool newState, List<TileState> statesToChange)
        {
            return new Tile(
                tile.X,
                tile.Y,
                tile.Piece == null ? null : PieceC.Clone(tile.Piece),
                tile.Element,
                tile.Contents,
                statesToChange.Contains(TileState.isClicked) ? newState : tile.IsClicked,
                statesToChange.Contains(TileState.isHovered) ? newState : tile.IsHovered,
                statesToChange.Contains(TileState.isHighlighted) ? newState : tile.IsHighlighted,
                statesToChange.Contains(TileState.isAOE) ? newState : tile.IsAOE,
                tile.RemainingTimeOnEnvironment
            );
        }

        public static Tile UpdateRemainingEnvTime(Tile tile, int newRemainingTimeOnEnv)
        {
            return new Tile(
               tile.X,
               tile.Y,
               tile.Piece == null ? null : PieceC.Clone(tile.Piece),
               tile.Element,
               tile.Contents,
               tile.IsClicked,
               tile.IsHovered,
               tile.IsHighlighted,
               tile.IsAOE,
               newRemainingTimeOnEnv
           );
        }

        public static Tile UpdateContents(Tile tile, TileContents newContents)
        {
            return new Tile(
                tile.X,
                tile.Y,
                tile.Piece == null ? null : PieceC.Clone(tile.Piece),
                tile.Element,
                newContents,
                tile.IsClicked,
                tile.IsHovered,
                tile.IsHighlighted,
                tile.IsAOE,
                tile.RemainingTimeOnEnvironment
            );
        }

        public static Tile UpdateElement(Tile tile, string element)
        {
            return new Tile(
                tile.X,
                tile.Y,
                tile.Piece == null ? null : PieceC.Clone(tile.Piece),
                element,
                tile.Contents,
                tile.IsClicked,
                tile.IsHovered,
                tile.IsHighlighted,
                tile.IsAOE,
                tile.RemainingTimeOnEnvironment
            );
        }
    }
}

