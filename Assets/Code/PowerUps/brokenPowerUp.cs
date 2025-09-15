using UnityEngine;

public class brokenPowerUp : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.damageToBricks = 10;
        game.damageToPaddle = 20;
    }
}
