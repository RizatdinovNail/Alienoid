using UnityEngine;

public class doubleReward : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        game.rewardCount++;
    }
}

