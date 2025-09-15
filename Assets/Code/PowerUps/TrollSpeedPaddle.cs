using UnityEngine;

public class TrollSpeedPaddle : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.speed = 4000f;
    }
}
