using UnityEngine;

public class thrillerScript : PowerUpEffect
{
    public Sprite usedPowerUp;
    public override void Apply(gameCore game)
    {
        if (!game.isReviveUsed)
        {
            game.secondLive = 1;
        }

        else
        {
            game.secondLive = 0;
            foreach (PowerUps powerup in gameManager.Instance.aquiredPowerUps) {
                if (powerup.name == "Thriller")
                {
                    powerup.smallSprite = usedPowerUp;
                }
            }
        }
    }
}
