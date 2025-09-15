using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class brickCode : MonoBehaviour
{
    public int health;
    public gameCore gameCore;
    public GameObject ball;
    public GameObject secondBall;
    public string brick;
    public float fallSpeed = 240f;
    private Rigidbody2D rb;
    private Vector2 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (gameCore.isGamePaused)
        {
            if (rb.linearVelocity != Vector2.zero)
            {
                lastVelocity = rb.linearVelocity;
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if (rb.linearVelocity == Vector2.zero && lastVelocity != Vector2.zero)
            {
                rb.linearVelocity = lastVelocity;
                lastVelocity = Vector2.zero;
            }
        }

        if(rb.position.y < 0)
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.Q)) {
            health = 0;

                    Image img = gameObject.GetComponent<Image>();
                    img.sprite = gameCore.Coin;
                    img.SetNativeSize();
                    gameCore.allBricks.Remove(gameObject);
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -fallSpeed);
              

            }
        
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Image img = gameObject.GetComponent<Image>();
        if ((collision.gameObject == ball || collision.gameObject == secondBall) && img.sprite != gameCore.Coin)
        {
            health -= gameCore.damageToBricks;
            if (health <= 0)
            {
                img.sprite = gameCore.Coin;
                img.SetNativeSize();
                gameManager.Instance.PlaySound(gameManager.Instance.brickDestroyed, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
                gameCore.allBricks.Remove(gameObject);
                moveCoin(gameObject);
                
                if(brick == "explosive")
                {
                    gameManager.Instance.PlaySound(gameManager.Instance.expolosion, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
                    explode(gameObject);
                }
            }
        }
    }

    public void moveCoin(GameObject coin)
    {
        if (gameCore.isGamePaused)
        {
            coin.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -fallSpeed);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == gameCore.paddle)
        {
            Destroy(gameObject);
            gameCore.changeGold(gameCore.rewardCount);
        }
    }


    void explode(GameObject boomBrick)
    {
        Vector2 position = boomBrick.transform.position;

        float radius = 100f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius);

        foreach (Collider2D hit in hits)
        {
                brickCode brick = hit.GetComponent<brickCode>();
                if (brick != null) {
                    brick.health--;
                    if(brick.health <= 0)
                    {
                        Image img = brick.GetComponent<Image>();
                        img.sprite = gameCore.Coin;
                        img.SetNativeSize();
                        gameCore.allBricks.Remove(hit.gameObject);
                        brick.GetComponent<BoxCollider2D>().isTrigger = true;
                        brick.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -fallSpeed);
                    }
                
            }
        }
    }
}
