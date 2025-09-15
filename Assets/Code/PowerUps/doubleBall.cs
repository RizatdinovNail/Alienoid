using UnityEngine;

public class doubleBall : PowerUpEffect
{
    public override void Apply(gameCore game)
    {
        
        GameObject newBall = Instantiate(game.ball);
        newBall.transform.SetParent(game.UIcontainer.transform);
        game.brickCodePrefab.secondBall = newBall;

        Vector2 oldBallPos = game.ball.transform.position;
        oldBallPos.x -= 20;
        game.ball.transform.position = oldBallPos;
        newBall.transform.rotation = game.ball.transform.rotation;
        newBall.transform.position = game.ball.transform.position;
        Vector2 newPos = newBall.transform.position;
        newPos.x += 40;
        newBall.transform.position = newPos;

        Rigidbody2D originalRb = game.ball.GetComponent<Rigidbody2D>();
        Rigidbody2D newRB = newBall.GetComponent<Rigidbody2D>();
        Vector2 oldVel = originalRb.linearVelocity;
        Vector2 newVel = newRB.linearVelocity;
        newVel.y = oldVel.y;
        newVel.x = -oldVel.x;
        newRB.linearVelocity = newVel;

    }
}
