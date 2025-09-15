using UnityEngine;
using UnityEngine.UI;

public class sharpenCode : PowerUpEffect
{
    public Sprite spikyBall;
    public Sprite spikySlimyBall;
    bool sticky;
    public override void Apply(gameCore game)
    {
        game.ball.GetComponent<Image>().sprite = spikyBall;
        sticky = game.isSticky;
        if (sticky) 
        {
            game.ball.GetComponent<Image>().sprite = spikySlimyBall;
        }
        game.damageToBricks = 2;
        game.ball.GetComponent<Image>().SetNativeSize();

    }
}
