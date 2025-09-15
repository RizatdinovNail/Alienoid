using UnityEngine;

public class hardenScript : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.damageToPaddle = 1;
    }
}
