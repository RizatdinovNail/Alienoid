using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource sceneMusic;

    void Start()
    {
        float volume = gameManager.Instance.musicVolume * gameManager.Instance.masterVolume;
        gameManager.Instance.PlaySound(sceneMusic, volume);
    }
}
