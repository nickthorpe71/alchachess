using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class damage_calculator
{
    [Test]
    public void sets_damage_to_half_with_50_percent_mitigation()
    {
        // arrange

        // act
        int finalDamage = DamageCalculator.CalculateDamage(10, 0.5f);

        // assert
        Assert.AreEqual(5, finalDamage);
    }

}
