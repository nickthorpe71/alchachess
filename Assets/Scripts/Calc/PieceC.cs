using System;
using System.Linq;
using System.Collections.Generic;
using Data;

namespace Calc
{
    public static class PieceC
    {
        public static string GetPathByLabel(PieceLabel label, PieceColor color)
            => $"Pieces/{Enum.GetName(typeof(PieceColor), color)}/{Enum.GetName(typeof(PieceLabel), label)}";

        public static string PieceAsString(Piece piece)
        {
            return $"|Label: {piece.Label} |Color: {piece.Color} |Health: {piece.Health} |MaxHealth: {piece.MaxHealth} |Power: {piece.Power} |Move: {piece.MoveDistance} " + $"|Player: {piece.Player}";
        }

        public static Piece ApplySpellToPiece(Piece attacker, Piece defender, Spell spell)
        {
            Piece pieceCopy = PieceC.Clone(defender);
            SpellEffect spellEffect = SpellEffects.list[spell.Color];
            bool isEnemy = attacker.Player != defender.Player;

            if (isEnemy)
            {
                if (spellEffect.DamagesEnemies)
                    pieceCopy = PieceC.UpdateHealth(pieceCopy, pieceCopy.Health - SpellC.CalcDamage(spell.Damage, attacker.Power));
                if (spellEffect.HealsEnemies)
                    pieceCopy = PieceC.UpdateHealth(pieceCopy, pieceCopy.Health + SpellC.CalcHeal(spell.Damage, attacker.Power));
            }
            else // it's an ally
            {
                if (spellEffect.DamagesAllies)
                    pieceCopy = PieceC.UpdateHealth(pieceCopy, pieceCopy.Health - SpellC.CalcDamage(spell.Damage, attacker.Power));
                if (spellEffect.HealsAllies)
                    pieceCopy = PieceC.UpdateHealth(pieceCopy, pieceCopy.Health + SpellC.CalcHeal(spell.Damage, attacker.Power));
            }

            if (pieceCopy.Health > pieceCopy.MaxHealth)
                pieceCopy = PieceC.UpdateHealth(pieceCopy, SpellC.CalcDamage(spell.Damage, attacker.Power));

            if (pieceCopy.Health < 0)
                pieceCopy = PieceC.UpdateHealth(pieceCopy, 0);

            return pieceCopy;
        }

        public static Piece Clone(Piece piece)
        {
            return new Piece(
                piece.Guid,
                piece.Label,
                piece.GodType,
                piece.Color,
                piece.Player,
                piece.CurrentRecipe,
                piece.Health,
                piece.MaxHealth,
                piece.Power,
                piece.MoveDistance,
                piece.MovePattern,
                piece.AttackDistance,
                piece.AttackPattern
            );
        }

        public static Piece UpdateHealth(Piece piece, float newHealth)
        {
            return new Piece(
                piece.Guid,
                piece.Label,
                piece.GodType,
                piece.Color,
                piece.Player,
                piece.CurrentRecipe,
                newHealth,
                piece.MaxHealth,
                piece.Power,
                piece.MoveDistance,
                piece.MovePattern,
                piece.AttackDistance,
                piece.AttackPattern
            );
        }

        public static Piece NewPieceFromTemplate(Piece template, PlayerData player)
        {
            return new Piece(
                Guid.NewGuid(),
                template.Label,
                template.GodType,
                player.PieceColor,
                player.PlayerToken,
                "",
                template.Health,
                template.MaxHealth,
                template.Power,
                template.MoveDistance,
                template.MovePattern,
                template.AttackDistance,
                template.AttackPattern
            );
        }

        public static List<Piece> GetByGodType(GodType godType, List<Piece> pieceList) => pieceList.Where(piece => piece.GodType == godType).ToList();
    }
}
