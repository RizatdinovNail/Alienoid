using UnityEngine;

public class SpeedPaddle : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.speed = 2000f;
    }
}
