using UnityEngine;

public class parryScript : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.isParryEnabled = true;
    }
}
