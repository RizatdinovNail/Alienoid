using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class changeScene : MonoBehaviour
{
    public InfoPopUp infoPopUp;
    public GameObject windowInfo;
    public TextMeshProUGUI textInfo;
    public Button button;
    public void GoToScene(GameObject clickedButton)
    {
        if(clickedButton.name == "PlayButton")
        {
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));
            SceneManager.LoadScene("LevelSelectorScene");
        }

        else if(clickedButton.name == "ShopButton")
        {
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));

            SceneManager.LoadScene("ShopScene");

        }

        else if(clickedButton.name == "MainMenu")
        {
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));

            SceneManager.LoadScene("MainMenuScene");

        }

        else if(clickedButton.name == "ExitButton")
        {
            infoPopUp.createInfoWindow(textInfo, windowInfo);
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));
            button.onClick.AddListener(() => SceneManager.LoadScene("ShopScene"));
        }

        else if(clickedButton.name == "SkinSelectorButton")
        {
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));

            SceneManager.LoadScene("SkinSelectorScene");

        }

        else if(clickedButton.name == "OptionsButton")
        {
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));

            SceneManager.LoadScene("OptionScene");

        }

        else if(clickedButton.name == "Level1")
        {
            gameManager.Instance.currentLevel = 1.1;
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));
            SceneManager.LoadScene("GameScene");

        }

        else if (clickedButton.name == "Level2")
        {
            gameManager.Instance.currentLevel = 2.1;
            SceneManager.LoadScene("GameScene");
        }

        else if (clickedButton.name == "Level3")
        {
            gameManager.Instance.currentLevel = 3.1;
            SceneManager.LoadScene("GameScene");
        }

        else if (clickedButton.name == "Vs")
        {
            gameManager.Instance.currentLevel = 4;
            SceneManager.LoadScene("GameScene");
        }

        else if(clickedButton.name == "ExitBtn")
        {
            foreach (PowerUps powerUp in gameManager.Instance.aquiredPowerUps.ToList())
            {
                gameManager.Instance.powerUps.Add(powerUp);
                gameManager.Instance.aquiredPowerUps.Remove(powerUp);
            }
            if (gameManager.Instance.phase == 2)
            {
                gameManager.Instance.currentLevel -= 0.1;
            }

            else if(gameManager.Instance.phase == 3)
            {
                gameManager.Instance.currentLevel -= 0.2;
            }

            gameManager.Instance.phase = 1;
            gameManager.Instance.isReviveUsed = false;
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));

            SceneManager.LoadScene("LevelSelectorScene");
        }

        else if(clickedButton.name == "CreditsButton")
        {
            gameManager.Instance.PlaySound(gameManager.Instance.clickSound, (gameManager.Instance.SFXvolume / 100f) * (gameManager.Instance.masterVolume / 100f));

            SceneManager.LoadScene("Credits");
        }

        else
        {
            Debug.Log("Clicked Button: " + clickedButton.name);
        }
    }
}
