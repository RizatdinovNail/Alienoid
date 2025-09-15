using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class configMenu : MonoBehaviour
{
    int currentMasterVolume;
    int currentSFXVolume;
    int currentMusicVolume;

    int savedMasterVolume;
    int savedSFXVolume;
    int savedMusicVolume;

    public TextMeshProUGUI volumeMasterText;
    public TextMeshProUGUI volumeSFXText;
    public TextMeshProUGUI volumeMusicText;

    public Image masterSpeaker;
    public Image SFXSpeaker;
    public Image musicSpeaker;

    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider musicSlider;

    public Sprite masterSpeakerMuted;
    public Sprite SFXSpeakerMuted;
    public Sprite musicSpeakerMuted;

    public Sprite masterSpeakerUmuted;
    public Sprite SFXSpeakerUmuted;
    public Sprite musicSpeakerUmuted;

    public GameObject audioParent;
    public GameObject controlersParent;
    public GameObject GoAudio;
    public GameObject GoControls;
    public Sprite audioSelected;
    public Sprite controlsSelected;
    public Sprite audioDiseleceted;
    public Sprite controlsDiseleceted;

    public AudioSource clickSound;
    public AudioSource soundOn;

    private void Start()
    {
        currentMasterVolume = gameManager.Instance.masterVolume;
        currentSFXVolume = gameManager.Instance.SFXvolume;
        currentMusicVolume = gameManager.Instance.musicVolume;

        savedMasterVolume = currentMasterVolume;
        savedSFXVolume = currentSFXVolume;
        savedMusicVolume = currentMusicVolume;

        masterSlider.value = currentMasterVolume;
        SFXSlider.value = currentSFXVolume;
        musicSlider.value = currentMusicVolume;

        UpdateVolumeTexts();

        masterSpeaker.sprite = (currentMasterVolume == 0) ? masterSpeakerMuted : masterSpeakerUmuted;
        SFXSpeaker.sprite = (currentSFXVolume == 0) ? SFXSpeakerMuted : SFXSpeakerUmuted;
        musicSpeaker.sprite = (currentMusicVolume == 0) ? musicSpeakerMuted : musicSpeakerUmuted;

        masterSlider.onValueChanged.AddListener((value) => OnSliderChange(value, masterSlider));
        SFXSlider.onValueChanged.AddListener((value) => OnSliderChange(value, SFXSlider));
        musicSlider.onValueChanged.AddListener((value) => OnSliderChange(value, musicSlider));
        GoAudio.GetComponent<Button>().onClick.AddListener(() => {
            changeScene(true);
            gameManager.Instance.PlaySound(clickSound, (currentSFXVolume / 100f) * (currentMasterVolume / 100f));
        });

        GoControls.GetComponent<Button>().onClick.AddListener(() => {
            changeScene(false);
            gameManager.Instance.PlaySound(clickSound, (currentSFXVolume / 100f) * (currentMasterVolume / 100f));
        });
    }

    private void UpdateVolumeTexts()
    {
        volumeMasterText.text = $"{currentMasterVolume}%";
        volumeSFXText.text = $"{currentSFXVolume}%";
        volumeMusicText.text = $"{currentMusicVolume}%";
    }

    public void muteUnmute(Image currentSpeaker)
    {
        if (currentSpeaker == masterSpeaker)
        {
            if (masterSpeaker.sprite == masterSpeakerMuted)
            {
                if (savedMasterVolume <= 0) savedMasterVolume = 20;

                masterSpeaker.sprite = masterSpeakerUmuted;
                masterSlider.value = savedMasterVolume;
                currentMasterVolume = savedMasterVolume;
                gameManager.Instance.PlaySound(soundOn, (currentSFXVolume / 100f) * (currentMasterVolume / 100f));
            }
            else
            {
                savedMasterVolume = currentMasterVolume;
                masterSpeaker.sprite = masterSpeakerMuted;
                masterSlider.value = 0;
                currentMasterVolume = 0;
            }

            gameManager.Instance.masterVolume = currentMasterVolume;
        }

        else if (currentSpeaker == SFXSpeaker)
        {
            if (SFXSpeaker.sprite == SFXSpeakerMuted)
            {
                if (savedSFXVolume <= 0) savedSFXVolume = 20;

                SFXSpeaker.sprite = SFXSpeakerUmuted;
                SFXSlider.value = savedSFXVolume;
                currentSFXVolume = savedSFXVolume;
                gameManager.Instance.PlaySound(soundOn, (currentSFXVolume / 100f) * (currentMasterVolume / 100f));

            }
            else
            {
                savedSFXVolume = currentSFXVolume;
                SFXSpeaker.sprite = SFXSpeakerMuted;
                SFXSlider.value = 0;
                currentSFXVolume = 0;
            }

            gameManager.Instance.SFXvolume = currentSFXVolume;
        }

        else if (currentSpeaker == musicSpeaker)
        {
            if (musicSpeaker.sprite == musicSpeakerMuted)
            {
                if (savedMusicVolume <= 0) savedMusicVolume = 20;

                musicSpeaker.sprite = musicSpeakerUmuted;
                musicSlider.value = savedMusicVolume;
                currentMusicVolume = savedMusicVolume;
                gameManager.Instance.PlaySound(soundOn, (currentSFXVolume / 100f) * (currentMasterVolume / 100f));

            }
            else
            {
                savedMusicVolume = currentMusicVolume;
                musicSpeaker.sprite = musicSpeakerMuted;
                musicSlider.value = 0;
                currentMusicVolume = 0;
            }

            gameManager.Instance.musicVolume = currentMusicVolume;
        }

        UpdateVolumeTexts();
    }

    public void OnSliderChange(float value, Slider slider)
    {
        int intValue = (int)value;

        if (slider == masterSlider)
        {
            currentMasterVolume = intValue;
            gameManager.Instance.masterVolume = intValue;
            masterSpeaker.sprite = (intValue == 0) ? masterSpeakerMuted : masterSpeakerUmuted;
        }
        else if (slider == SFXSlider)
        {
            currentSFXVolume = intValue;
            gameManager.Instance.SFXvolume = intValue;
            SFXSpeaker.sprite = (intValue == 0) ? SFXSpeakerMuted : SFXSpeakerUmuted;
        }
        else if (slider == musicSlider)
        {
            currentMusicVolume = intValue;
            gameManager.Instance.musicVolume = intValue;
            musicSpeaker.sprite = (intValue == 0) ? musicSpeakerMuted : musicSpeakerUmuted;
        }

        UpdateVolumeTexts();
    }

    public void changeScene(bool audioScene)
    {
        if (audioScene) 
        { 
            audioParent.SetActive(true);
            GoAudio.GetComponent<Image>().sprite = audioSelected;
            controlersParent.SetActive(false);
            GoControls.GetComponent<Image>().sprite = controlsDiseleceted;
        }

        else
        {
            audioParent.SetActive(false);
            GoAudio.GetComponent<Image>().sprite = audioDiseleceted;
            controlersParent.SetActive(true);
            GoControls.GetComponent<Image>().sprite = controlsSelected;
        }
    }
}
