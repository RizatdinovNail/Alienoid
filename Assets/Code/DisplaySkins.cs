using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySkins : MonoBehaviour
{

    public GameObject skinSlotPrefab;
    public Transform parentContainer;
    public Image selectedSkin;
    public GameObject equippedButton;
    public GameObject notEquippedBtn;
    public GameObject lockPrefabw;

    void Start()
    { 
        selectedSkin.sprite = gameManager.Instance.currentSkin[0].sprite;

        Vector2 pos = skinSlotPrefab.transform.position;
        float slotWidth = skinSlotPrefab.GetComponent<RectTransform>().rect.width;
        float slotHeight = skinSlotPrefab.GetComponent<RectTransform>().rect.height;
        float startX = pos.x;
        float posX = pos.x;
        float posY = pos.y;
        foreach (var skin in gameManager.Instance.skinsAquired)
        {
            displaySkinsHelper(skin, startX, pos, posX, posY, true);
            if (posX > slotWidth * 5 + 10f)
            {
                posX = startX;
                posY -= slotHeight + 50f;
            }
            else
            {
                posX += slotWidth + 10f;
            }

        }

        foreach(var skin in gameManager.Instance.skinsWithRarityCopy)
        {
            
            displaySkinsHelper(skin, startX, pos, posX, posY, false);
            if (posX > slotWidth * 5 + 10f)
            {
                posX = startX;
                posY -= slotHeight + 50f;
            }
            else
            {
                posX += slotWidth + 10f;
            }
        }


    }

    void displaySkinsHelper(SkinWithRarity skin, float startX, Vector2 pos, float posX, float posY, bool isOwned)
    {
        GameObject slot = Instantiate(skinSlotPrefab, parentContainer);
        RectTransform rectTransform = slot.GetComponent<RectTransform>();
        Image img = slot.GetComponent<Image>();
        img.name = skin.sprite.name;
        slot.transform.position = new Vector2(posX, posY);
        slot.SetActive(true);

        if (img != null)
        {
            img.sprite = skin.sprite;

            if (!isOwned)
            {
                img.color = new Color(img.color.r * 0.01f, img.color.g * 0.01f, img.color.b * 0.01f, img.color.a);
            }
        }

        if (isOwned)
        {
            Button button = slot.AddComponent<Button>();
            button.onClick.AddListener(() =>
            {
                chooseSkin(img);
            });
        }

        if(!isOwned)
        {

        }

    }

    public void chooseSkin(Image skin)
    {
        selectedSkin.sprite = skin.sprite;
        if (gameManager.Instance.currentSkin[0].sprite == selectedSkin.sprite)
        {
            notEquippedBtn.SetActive(false);
            equippedButton.SetActive(true);
        }

        else
        {
            notEquippedBtn.SetActive(true);
            equippedButton.SetActive(false);
        }

        Button button = notEquippedBtn.GetComponent<Button>();
        if (button == null)
            button = notEquippedBtn.AddComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            notEquippedBtn.SetActive(false);
            equippedButton.SetActive(true);
            setSkin(skin);
        });
    }

    void setSkin(Image skin)
    {
        Sprite selectedSprite = skin.sprite;
        SkinWithRarity selectedSkinData = gameManager.Instance.skinsAquired.Find(s => s.sprite == skin.sprite);
        gameManager.Instance.currentSkin[0] = selectedSkinData;
    }
}
