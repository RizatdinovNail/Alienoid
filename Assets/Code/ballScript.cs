using UnityEngine;

public class ballScript : MonoBehaviour
{
    public gameCore game;
    public Rigidbody2D rb;
    Vector2 savedVelocity;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == game.paddle)
        {
            savedVelocity = rb.linearVelocity;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (rb.linearVelocity == Vector2.zero && Input.GetKey(KeyCode.Space)) 
        {
            rb.linearVelocity = savedVelocity;
            savedVelocity = Vector2.zero;
        }


    }
    private void FixedUpdate()
    {
        if (rb.linearVelocity == Vector2.zero)
        {
            Vector2 ballPosition = rb.position;
            if (Input.GetKey(KeyCode.A))
            {
                ballPosition.x -= game.speed * Time.fixedDeltaTime;

            }

            if (Input.GetKey(KeyCode.D))
            {
                ballPosition.x += game.speed * Time.fixedDeltaTime;

            }

            if (game.isVerticalMovEnabled)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    ballPosition.y += game.speed * Time.fixedDeltaTime;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    ballPosition.y -= game.speed * Time.fixedDeltaTime;
                }
            }
        }
    }
}
