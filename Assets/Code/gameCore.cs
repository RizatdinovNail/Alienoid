using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[System.Serializable]
public class BrickData
{
    public float x;             
    public float y;             
    public string type; 
}

[System.Serializable]
public class LevelData
{
    public int level;
    public int levelPhase;
    public List<BrickData> bricks;
}
public class gameCore : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timer;
    float elapsedTime = 0;
    int STARTING_TIME = 400;
    public int time;
    public GameObject pauseBtn;
    public GameObject pauseMenuScreen;
    public GameObject resumeBtn;
    public GameObject quitBtn;
    public Sprite pauseBtnActive;
    public Sprite pauseBtnInActive;
    public float speed = 1000.0f;
    public float ballSpeed = 600f;
    public bool isGamePaused = false;
    bool isGameStarted = false;
    public bool restoreTimeout = false;
    bool isGameEnded = false;
    public List<Sprite> countdown = new List<Sprite>();
    public GameObject countdownImg;
    int i = 0;
    public brickCode brickCodePrefab;


    public Rigidbody2D paddleRB;
    public GameObject paddle;
    public GameObject paddle2;
    public Rigidbody2D paddle2RB;
    public Rigidbody2D ballRB;
    public GameObject ball;
    public GameObject ball2;
    public Rigidbody2D ball2RB;
    Vector2 ballDirection;
    Vector2 startBallPosition;
    Vector2 currentBallVelocity;
    public Transform bricksContainer;
    public GameObject panel;

    double currentLvl = gameManager.Instance.currentLevel;
    int currentPhase = gameManager.Instance.phase;

    public GameObject brickPrefab;
    public Sprite bulletSprite;
    public Sprite Coin;

    List<GameObject> gunBricks = new List<GameObject>();
    public List<GameObject> allBricks = new List<GameObject>();


    public GameObject powerUpPrefab;
    public GameObject powerUpContainer;
    public GameObject smallPowerUpPrefab;
    public GameObject smallPowerUpContainer;
    public GameObject infoIcon;
    public GameObject infoWindow;
    public TextMeshProUGUI descriptionText;
    public Button closeButton;
    InfoPopUp infoPopUp;
    bool isPowerSection = false;

    public GameObject gameOverContainer;
    public GameObject title;
    public Sprite gameWonTitle;
    public Sprite gameOverTitle;
    public GameObject retryButton;
    public GameObject reviveBtn;
    public int damageToPaddle = 2;
    public int damageToBricks = 1;
    public int rewardCount = 1;
    public int secondLive = 0;
    public bool isReviveUsed = gameManager.Instance.isReviveUsed;
    public bool isVerticalMovEnabled = false;
    public bool isParryEnabled = false;
    public bool isSticky = false;
    public GameObject UIcontainer;
    Vector2 screenBounds;

    void Start()
    {
        isReviveUsed = gameManager.Instance.isReviveUsed;
        time = STARTING_TIME;
        goldText.text = gameManager.Instance.currentGold.ToString();
        Image img = pauseBtn.GetComponent<Image>();
        img.sprite = pauseBtnInActive;
        timer.text = time.ToString();
        paddle.GetComponent<Image>().sprite = gameManager.Instance.currentSkin[0].bottomSprite;
        ball.GetComponent<Image>().sprite = gameManager.Instance.currentSkin[0].ballSprite;
        if (gameManager.Instance.aquiredPowerUps.Count > 0)
        {
            applyPowerUps();
            showSmallPowerUps();
        }

        if (!isPowerSection)
        {
            LoadLevel(currentLvl);
            startBall();
        }
        
        infoPopUp = GetComponent<InfoPopUp>();
        restoreTimeout = true;
        gameOverContainer.SetActive(false);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu();
        }

        if ((!isGameStarted || restoreTimeout) && !isGamePaused && !isPowerSection && !isGameEnded)
        {
            startCountdown();
        }

        if(!isGamePaused && isGameStarted && !restoreTimeout && !isPowerSection && !isGameEnded)
        {
            
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= 1 )
            {
                time--;
                updateTimer();
                elapsedTime -= 1;
                foreach(GameObject gunBrick in gunBricks)
                {
                    if(gunBrick != null)
                    {
                        shootBrick(gunBrick);
                    }
                }
            }

        }



        checkBallPos(ballRB);

        if(time <= 0 && !isGameEnded)
        {
            time = 0;
            isGameEnded = true;
            gameEnded(false);
        }

        if(allBricks.Count <= 0 && !isPowerSection)
        {
            currentPhase = gameManager.Instance.phase;
            if (currentPhase >= 3 && !isGameEnded)
            {

                isGameEnded = true;
                gameEnded(true);
            }

            else if(currentPhase < 3 && !isGameEnded)
            {
                isPowerSection = true;
                powerUpSection();
            }
        }

       
    }

    void LoadLevel(double level)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Levels/Level {level}");
        if (jsonFile != null)
        {
            LevelData Clevel = JsonUtility.FromJson<LevelData>(jsonFile.text);
            float panelWidth = panel.GetComponent<RectTransform>().rect.width;
            float panelHeight = panel.GetComponent<RectTransform>().rect.height;
            float offsetX = -panelWidth / 2f;
            float offsetY = -panelHeight / 2f;
            foreach (BrickData brick in Clevel.bricks)
            {
                BlockType type = gameManager.Instance.GetBlockType(brick.type);
                if (type != null)
                {
                    Vector2 position = new Vector2(brick.x * panelWidth + offsetX , (brick.y + 0.25f) * panelHeight + offsetY);
                    GameObject brickGO = Instantiate(brickPrefab, position, Quaternion.identity);
                    brickGO.transform.SetParent(bricksContainer, false);
                    brickGO.AddComponent<Image>();
                    brickGO.SetActive(true);
                    brickGO.GetComponent<Image>().sprite = type.sprite;
                    brickGO.GetComponent<Image>().SetNativeSize();

                    brickCode brickCode = brickGO.AddComponent<brickCode>();
                    brickCodePrefab = brickCode; 
                    brickCode.ball = ball;
                    brickCode.brick = type.typeName;
                    brickCode.health = type.defaultHealth;
                    brickCode.gameCore = this;
                    if(type.typeName == "gun")
                    {
                        gunBricks.Add(brickGO);
                    }
                    allBricks.Add(brickGO);

                }
                else
                {
                    Debug.Log("Error");
                }
            }
        }
        else
        {
            Debug.Log($"Level {level} not found!");
        }
    }

    private void FixedUpdate()
    {
        if (isGameStarted && !isGamePaused)
        {
            movePaddle();

            if (Mathf.Abs(ballDirection.y) < 0.1f)
            {
                float sign = ballDirection.y >= 0 ? 1f : -1f;
                ballDirection.y = 0.2f * sign;
                ballDirection = ballDirection.normalized;
            }

            if (ballRB != null)
            {
                ballRB.linearVelocity = ballRB.linearVelocity.normalized * ballSpeed;
            }

        }

    }

    public void checkBallPos(Rigidbody2D ballRigidBody)
    {
        if (ballRigidBody.position.y < 0)
        {
            ballRigidBody.position = startBallPosition;
            ballRigidBody.linearVelocity = Vector2.zero;
            gameManager.Instance.PlaySound(gameManager.Instance.ballFalls, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
            restoreTimeout = true;
        }
    }

    void startBall()
    {
        startBallPosition = ballRB.position;
        if (isSticky && ball.GetComponent<ballScript>() == null)
        {
            ballScript ballScript = ball.AddComponent<ballScript>();
            ballScript.game = this;
            ballScript.rb = ballRB;
        }
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(0.5f, 1f);

        ballDirection = new Vector2(x, y).normalized;
    }

    void movePaddle()
    {
        

        Vector2 position = paddleRB.position;

        // Move left
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed * Time.fixedDeltaTime;
        }

        // Move right
        if (Input.GetKey(KeyCode.D))
        {
            position.x += speed * Time.fixedDeltaTime;
        }

        float halfPaddleWidth = paddle.GetComponent<RectTransform>().rect.width / 2f * paddle.transform.lossyScale.x;
        float halfPaddleHeight = paddle.GetComponent<RectTransform>().rect.height / 2f * paddle.transform.lossyScale.y;

        RectTransform parentUIRect = UIcontainer.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        parentUIRect.GetWorldCorners(corners);

        float leftBound = corners[0].x + halfPaddleWidth;
        float rightBound = corners[2].x - halfPaddleWidth;
        float topBound = corners[2].y - halfPaddleHeight;
        float bottomBound = corners[0].y + halfPaddleHeight;


        if (position.x < leftBound) position.x = leftBound;
        if (position.x > rightBound) position.x = rightBound;

        if (isVerticalMovEnabled)
        {
            paddleRB.constraints = RigidbodyConstraints2D.None;
            paddleRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (Input.GetKey(KeyCode.W))
            {
                position.y += speed * Time.fixedDeltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                position.y -= speed * Time.fixedDeltaTime;
            }

            if(position.y < bottomBound) position.y = bottomBound;
            if(position.y >  topBound) position.y = topBound;
        }

        foreach (GameObject gunBrick in gunBricks)
        {
            if (gunBrick != null)
            {
                rotateGun(gunBrick, position);
            }
        }

        paddleRB.MovePosition(position);
    }

    void moveBall()
    {
        startBall();
        ballRB.linearVelocity = ballDirection * ballSpeed;
    }

    public void changeGold(int value)
    {
        int gold = Convert.ToInt32(goldText.text);
        gold += value;
        gameManager.Instance.currentGold = gold;
        goldText.text = gameManager.Instance.currentGold.ToString();
    }

    public void updateTimer()
    {
        timer.text = time.ToString();
    }

    void startCountdown()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1)
        {
            if (i >= 4)
            {
                isGameStarted = true;
                if (restoreTimeout)
                {
                    restoreTimeout = false;
                }
                moveBall();
                countdownImg.SetActive(false);
                elapsedTime -= 1;
                i = 0;
            }

            else
            {
                updateCountdown(i);
                elapsedTime -= 1;
                i++;
            }
        }
    }

    void updateCountdown(int i)
    {
        if(countdownImg.activeSelf == false)
        {
            countdownImg.SetActive(true);
        }
        countdownImg.GetComponent<Image>().sprite = countdown[i];
        countdownImg.GetComponent<Image>().SetNativeSize();
    }

    public void pauseMenu()
    {
        gameManager.Instance.PlaySound(gameManager.Instance.clickSound, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
        if (isGamePaused)
            {
                isGamePaused = false;
                pauseMenuScreen.SetActive(false);
                Image img = pauseBtn.GetComponent<Image>();
                img.sprite = pauseBtnInActive;
                ballRB.linearVelocity = currentBallVelocity;
            }

            else if(!isGamePaused && isGameStarted && !isGameEnded)
            {
                isGamePaused = true;
                Image img = pauseBtn.GetComponent<Image>();
                img.sprite = pauseBtnActive;
                pauseMenuScreen.SetActive(true);
                currentBallVelocity = ballRB.linearVelocity;
                ballRB.linearVelocity = Vector2.zero;
        }
    }


    private void shootBrick(GameObject gunBrick)
    {
        Vector2 gunPos = gunBrick.transform.position;

        GameObject bullet = new GameObject("Bullet");

        Image img = bullet.AddComponent<Image>();
        img.sprite = bulletSprite;
        img.SetNativeSize();

        bullet.transform.SetParent(bricksContainer, false);
        bullet.transform.position = gunPos;
        bullet.transform.rotation = gunBrick.transform.rotation;

        BoxCollider2D col = bullet.AddComponent<BoxCollider2D>();
        col.isTrigger = true;


        Vector2 direction = -gunBrick.transform.up;
        float bulletSpeed = 600f;
        Rigidbody2D bulletRB = bullet.AddComponent<Rigidbody2D>();
        bulletRB.gravityScale = 0;
        if(!isGamePaused)
        {
            bulletRB.linearVelocity = direction * bulletSpeed;
        }

        bulletScript script = bullet.AddComponent<bulletScript>();
        script.gameCore = this;
    }

    private void rotateGun(GameObject gun, Vector2 paddlePosition)
    {
        Vector2 gunPosition = gun.transform.position;
        Vector2 direction = paddlePosition - gunPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90 ;
        gun.transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    void gameEnded(bool isGameWon)
    {
        gameOverContainer.SetActive(true);
        retryButton.SetActive(false);
        reviveBtn.SetActive(false);
        paddle.SetActive(false);
        ball.SetActive(false);
        Image img = title.GetComponent<Image>();
        if (isGameWon) {
            img.sprite = gameWonTitle;
        }

        else
        {
            gameManager.Instance.PlaySound(gameManager.Instance.gameOver, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
            img.sprite = gameOverTitle;
            retryButton.SetActive(true);
            if (secondLive == 1 && !isReviveUsed)
            {
                reviveBtn.SetActive(true);
                reviveBtn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    gameManager.Instance.isReviveUsed = true;
                    gameManager.Instance.PlaySound(gameManager.Instance.reboot, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
                    SceneManager.LoadScene("GameScene");
                });
            }

            retryButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                int phase = gameManager.Instance.phase;
                if (phase == 2)
                {
                    gameManager.Instance.currentLevel -= 0.1;

                }

                else if (phase == 3) 
                {
                    gameManager.Instance.currentLevel -= 0.2;
                }

                gameManager.Instance.phase = 1;
                foreach (PowerUps powerUp in gameManager.Instance.aquiredPowerUps.ToList())
                {
                    gameManager.Instance.powerUps.Add(powerUp);
                    gameManager.Instance.aquiredPowerUps.Remove(powerUp);
                }
                gameManager.Instance.isReviveUsed = false;
                SceneManager.LoadScene("GameScene");

            });
        }

        img.SetNativeSize();
    }

    void powerUpSection()
    {
        powerUpContainer.SetActive(true);
        paddle.SetActive(false);
        ball.SetActive(false);
        float x = powerUpPrefab.transform.position.x;
        powerUpPrefab.SetActive(false);    
        for(int i = 0; i < 3; i++)
        {
            GameObject newObj = Instantiate(powerUpPrefab, powerUpContainer.transform);
            newObj.SetActive(true);
            PowerUps selectedPowerUp = getRandomPowerUp();
            Image img = newObj.GetComponent<Image>();
            img.sprite = selectedPowerUp.bigSprite;
            Vector2 position = new Vector2();
            position.x = x;
            position.y = powerUpPrefab.transform.position.y;
            newObj.transform.position = position;
            newObj.GetComponent<Button>().onClick.AddListener(() => choosePowerUp(selectedPowerUp));
            x += 221f;
            Transform infoButtonTransform = newObj.transform.Find("InfoIcon");
            if (infoButtonTransform != null)
            {
                Button infoButton = infoButtonTransform.GetComponent<Button>();
                if (infoButton != null)
                {
                    string descriptionCopy = selectedPowerUp.description;
                    string nameCopy = selectedPowerUp.name;
                    infoButton.onClick.AddListener(() => showDescription(descriptionCopy, nameCopy));
                }
            }
        }
    }

    PowerUps getRandomPowerUp() 
    {
        int totalWeight = gameManager.Instance.powerUps.Sum(s => s.weight);
        int rand = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var powerUp in gameManager.Instance.powerUps)
        {
            current += powerUp.weight;
            if (rand < current)
                return powerUp;
        }

        return gameManager.Instance.powerUps[0];
    }

    void showDescription(string description, string name)
    {
        gameManager.Instance.PlaySound(gameManager.Instance.clickSound, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
        descriptionText.text = description;
        infoPopUp.createInfoWindow(descriptionText, infoWindow);
    }

    void choosePowerUp(PowerUps powerUp)
    {
        gameManager.Instance.PlaySound(gameManager.Instance.powerUp, gameManager.Instance.SFXvolume * gameManager.Instance.masterVolume);
        gameManager.Instance.powerUps.Remove(powerUp);
        gameManager.Instance.aquiredPowerUps.Add(powerUp);
        gameManager.Instance.currentLevel += 0.1;
        gameManager.Instance.phase++;
        SceneManager.LoadScene("GameScene");
    }

    void showSmallPowerUps()
    {
        foreach(PowerUps powerUp in gameManager.Instance.aquiredPowerUps)
        {
            if (smallPowerUpPrefab.activeSelf)
            {
                smallPowerUpPrefab.SetActive(false);

            }
            else if (!smallPowerUpPrefab.activeSelf)
            {
                Vector2 newPos = new Vector2(smallPowerUpPrefab.transform.position.x, smallPowerUpPrefab.transform.position.y);
                newPos.x -= 50f;
                smallPowerUpPrefab.transform.position = newPos;
            }

            GameObject smallPowerUp = Instantiate(smallPowerUpPrefab, smallPowerUpContainer.transform);
            smallPowerUp.transform.position = smallPowerUpPrefab.transform.position;
            smallPowerUp.GetComponent<Image>().sprite = powerUp.smallSprite;
            smallPowerUp.SetActive(true);
        }


    }

    void applyPowerUps()
    {
        foreach(PowerUps powerUp in gameManager.Instance.aquiredPowerUps)
        {
            if(powerUp.name != "Double Trouble")
            {
                GameObject effect = Instantiate(powerUp.effectScript);
                PowerUpEffect effectScript = effect.GetComponent<PowerUpEffect>();
                if (effectScript != null)
                {
                    effectScript.Apply(this);
                }
            }
        }
    }
}

