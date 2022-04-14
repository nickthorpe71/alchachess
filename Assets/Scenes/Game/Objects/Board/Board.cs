using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public BoardData boardData { get; private set; }
    Game game;

    private void Awake()
    {
        game = GetComponent<Game>();
        boardData = new BoardData();
        BoardCalculation.ResetBoard(game, boardData);
    }

    public void CastSpell(Element element, Piece caster)
    {
        StartCoroutine(CastRoutine(element, caster));
    }
    IEnumerator CastRoutine(Element element, Piece caster)
    {
        Vector2 elementPos = new Vector2(element.transform.position.x, element.transform.position.z);
        List<Vector2> validatedSpellPattern = BoardCalculation.ValidateSpellPattern(boardData, element.spellPattern, elementPos);
        foreach (Vector2 pos in validatedSpellPattern)
        {
            Tile tile = BoardCalculation.GetTile(boardData, pos);

            // plan spell animation
            game.Spawn(element.spellAnimPath, new Vector3(pos.x, 0.45f, pos.y), Quaternion.identity);

            yield return new WaitForSeconds(0.1f);

            // change environment
            tile.ApplySpellToEnvironment(element.color);

            if (tile.HasPiece() && !tile.IsImmuneToElement(element))
            {
                // apply spell/environmental effects
                if (element.hasKnockback)
                {
                    // determine direction
                    Vector2 direction = tile.pos - elementPos;
                    // look at tile one more in that direction
                    Vector2 toCheck = tile.pos + direction;

                    // if position past target tile is in bounds and can be traversed
                    if (BoardCalculation.IsInBounds(boardData, toCheck))
                    {
                        Tile nextTile = BoardCalculation.GetTile(boardData, toCheck);
                        if (nextTile.CanTraverse())
                            tile.TransferPiece(nextTile, warp: false);
                    }
                    else // if that is the edge of the board move piece and kill
                    {
                        tile.GetPiece().KnockOffBoard(direction);
                        boardData.pieces.Remove(tile.GetPiece());
                        tile.SetPiece(null);
                    }
                }
                else if (element.destroysOccupant)
                {
                    boardData.pieces.Remove(tile.GetPiece());
                    tile.KillPiece();
                }
            }
        }

        if (!caster.isBeingKnockedBack)
            game.NextTurn();
        else
            caster.isBeingKnockedBack = false;
    }

    public void SetHighlightedMoves(Vector2 pos, bool deactivate = false)
    {
        Tile tile = BoardCalculation.GetTile(boardData, pos);
        if (!tile.HasPiece()) return;

        List<Vector2> possibleMoves = tile
            .GetPiece()
            .PossibleMoves(boardData, pos);

        foreach (Vector2 move in possibleMoves)
            BoardCalculation.GetTile(boardData, move).Highlight(deactivate);
    }

    public void SetAOEMarkers(Vector2 pos, bool deactivate = false)
    {
        Tile hoveredTile = BoardCalculation.GetTile(boardData, pos);

        if (hoveredTile.HasActiveElement())
        {
            Element element = hoveredTile.element.GetComponent<Element>();

            foreach (Vector2 aoe in BoardCalculation.ValidateSpellPattern(boardData, element.spellPattern, pos))
                BoardCalculation.GetTile(boardData, aoe).AOE(deactivate);
        }
    }
}
