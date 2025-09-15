using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public gameCore gameCore;
    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    bool isPaused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastVelocity = rb.linearVelocity;

    }

    void Update()
    {
        if (gameCore.isGamePaused)
        {
            if (rb.linearVelocity != Vector2.zero && !isPaused)
            {
                lastVelocity = rb.linearVelocity;
                rb.linearVelocity = Vector2.zero;
                isPaused = true;

            }
        }
        else if(isPaused && !gameCore.isGamePaused)
        {
            if (rb.linearVelocity == Vector2.zero && lastVelocity != Vector2.zero)
            {
                rb.linearVelocity = lastVelocity;
                isPaused=false;
            }
        }

        if (gameCore.restoreTimeout && gameObject != null)
        {
            Destroy(gameObject);
        }

        if(rb.position.y < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == gameCore.paddle)
        {
            if (gameCore.isParryEnabled && Input.GetKey(KeyCode.E)) 
            { 
                rb.linearVelocity = -rb.linearVelocity;
            }
            else
            {
                gameManager.Instance.PlaySound(gameManager.Instance.losePoints, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
                Destroy(gameObject);
                gameCore.time = gameCore.time - gameCore.damageToPaddle;
                gameCore.updateTimer();
            }
                
        }
    }
}
