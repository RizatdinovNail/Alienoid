using UnityEngine;

public class stickyScript : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.isSticky = true;
    }
}
