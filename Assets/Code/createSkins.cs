using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class createSkins : MonoBehaviour
{
    public GameObject prefab;
    public GameObject windowWarning;
    public GameObject windowInfo;
    public TextMeshProUGUI textWarning;
    public TextMeshProUGUI textInfo;
    private bool buttonIsClicked = false;
    float speed;
    public TextMeshProUGUI goldText;
    public GameObject skinWindow;
    public Button equipButton;
    int gold = gameManager.Instance.currentGold;

    Vector2 firstPos;

    public GameObject gamblingScreen;
    public GameObject hidingPanel;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    InfoPopUp infoPopUp;


    private void Start()
    {
        speed = Random.Range(4500f, 6000f);
        infoPopUp = GetComponent<InfoPopUp>();
        skinWindow.SetActive(false);
        updateGoldText();
    }

    public void changeGambling()
    {
        if (!buttonIsClicked)
        {
            if (gold >= 10)
            {
                gold -= 10;
                gameManager.Instance.currentGold = gold;
                updateGoldText();
                buttonIsClicked = !buttonIsClicked;
                startGambling();
            }
            else
            {
                infoPopUp.createInfoWindow(textInfo, windowInfo);
            }

        }
        else
        {
            infoPopUp.createInfoWindow(textWarning, windowWarning);

        }
    }

    void startGambling()
    {
        gameManager.Instance.PlaySound(gameManager.Instance.gambling, gameManager.Instance.musicVolume * gameManager.Instance.masterVolume);
        gamblingScreen.SetActive(true);
        hidingPanel.SetActive(true);

        foreach (var obj in spawnedObjects)
            Destroy(obj);

        spawnedObjects.Clear();

        GameObject newObj = Instantiate(prefab, hidingPanel.transform);
        RectTransform rt = newObj.GetComponent<RectTransform>();
        newObj.SetActive(true);
        

        spawnedObjects.Add(newObj);

        StartCoroutine(CycleSingleSkin(newObj, 3f, 0.05f));
    }

    private SkinWithRarity GetRandomWeightedSkin()
    {
        int totalWeight = gameManager.Instance.skinsWithRarity.Sum(s => s.weight);
        int rand = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var skin in gameManager.Instance.skinsWithRarity)
        {
            current += skin.weight;
            if (rand < current)
                return skin;
        }

        return gameManager.Instance.skinsWithRarity[0];
    }

    IEnumerator CycleSingleSkin(GameObject obj, float totalDuration, float interval)
    {
        float elapsed = 0f;
        int index = 0;
        Image img = obj.GetComponent<Image>();

        while (elapsed < totalDuration)
        {
            if (img != null && gameManager.Instance.skinsWithRarity.Count > 0)
            {
                SkinWithRarity selected = GetRandomWeightedSkin();
                img.sprite = selected.sprite;
                obj.name = selected.sprite.name;
            }

            index++;
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        // Final selection
        if (img != null)
        {
            Sprite selectedSprite = img.sprite;
            GameObject selectedSkin = obj;

            SkinWithRarity selectedSkinData = gameManager.Instance.skinsWithRarity.Find(s => s.sprite == img.sprite);
            SkinWithRarity aquiredSkinData = gameManager.Instance.skinsAquired.Find(s => s.sprite == img.sprite);
            SkinWithRarity copyToRemove = gameManager.Instance.skinsWithRarityCopy
        .Find(s => s.sprite == selectedSkinData.sprite);

            if (selectedSkinData != null && selectedSkinData != aquiredSkinData)
            {
                gameManager.Instance.skinsAquired.Add(selectedSkinData);
                gameManager.Instance.skinsWithRarityCopy.Remove(copyToRemove);
            }

            showSelectedSkin(selectedSkin);
            equipButton.onClick.AddListener(() => equipSkin(selectedSkinData));
        }

        buttonIsClicked = false;
    }

    public void updateGoldText()
    {
        if(gold > 999)
        {
            goldText.text = "999+";
        }
        else
        {
            goldText.text = gold.ToString();
        }
    }

    void showSelectedSkin(GameObject skin)
    {
        skinWindow.SetActive(true);
        skin.transform.SetParent(skinWindow.transform, false);
        RectTransform rt = skin.GetComponent<RectTransform>();
        Vector2 pos = rt.anchoredPosition;
        pos.x = 0f;
        rt.anchoredPosition = pos;
    }

    public void closePopUp()
    {
        skinWindow.SetActive(false);
        gamblingScreen.SetActive(false);
    }

    public void equipSkin(SkinWithRarity skinData)
    {
        gameManager.Instance.currentSkin[0] = skinData;
        skinWindow.SetActive(false);
        gamblingScreen.SetActive(false);
    }
}
