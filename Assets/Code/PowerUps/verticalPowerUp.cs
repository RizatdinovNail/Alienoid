using UnityEngine;

public class verticalPowerUp : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.isVerticalMovEnabled = true;
    }
}
