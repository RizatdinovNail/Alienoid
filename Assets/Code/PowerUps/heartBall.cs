using UnityEngine;
using UnityEngine.UI;

public class heartBall : PowerUpEffect
{
    public Sprite defaultHeart;
    public Sprite poisonHeart;
    public Sprite spikyHeart;
    public Sprite spikyPosionHeart;
    bool poison = false;
    bool spiky = false;
    public override void Apply(gameCore game)
    {
        game.ball.GetComponent<Image>().sprite = defaultHeart;
        foreach (PowerUps powerUp in gameManager.Instance.aquiredPowerUps) { 
            if(powerUp.name == "Sharpen")
            {
                spiky = true;
            }

            if(powerUp.name == "Sticky Situation")
            {
                poison = true;
            }
        }
        if (poison && spiky)
        {
            game.ball.GetComponent<Image>().sprite = spikyPosionHeart;
        }
        else if (poison && !spiky)
        {
            game.ball.GetComponent<Image>().sprite = poisonHeart;
        }

        else if (!poison && spiky) 
        {
            game.ball.GetComponent<Image>().sprite = spikyHeart;
        }

        game.ball.GetComponent<Image>().SetNativeSize();
    }
}
