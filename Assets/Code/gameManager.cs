using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum Rarity
{
    Common, //50
    Uncommon, //30
    Rare, // 10
    Epic, // 8
    Legendary //2
}

[System.Serializable]
public class SkinWithRarity
{
    public Sprite sprite;
    public Sprite ballSprite;
    public Sprite bottomSprite;
    public Sprite topSprite;
    public Rarity rarity;
    public int weight;
}

[System.Serializable]
public class BlockType
{
    public string typeName;
    public Sprite sprite;
    public int defaultHealth;
}

[System.Serializable]
public class PowerUps
{
    public Sprite bigSprite;
    public Sprite smallSprite;
    public int weight;
    public string description;
    public string name;
    public GameObject effectScript;
}


public class gameManager : MonoBehaviour
{
    public static gameManager Instance;
    public List<SkinWithRarity> skinsWithRarity;
    public List<SkinWithRarity> skinsWithRarityCopy = new List<SkinWithRarity>();
    public List<SkinWithRarity> skinsAquired = new List<SkinWithRarity>();
    public int currentGold = 0;
    public double currentLevel = 0;
    public SkinWithRarity[] currentSkin = new SkinWithRarity[0];
    public List<PowerUps> powerUps = new List<PowerUps>();
    public List<BlockType> blockTypes;
    public int masterVolume = 50;
    public int SFXvolume = 50;
    public int musicVolume = 50;
    public int phase = 1;
    public List<PowerUps> aquiredPowerUps = new List<PowerUps>();
    public bool isReviveUsed = false;
    public AudioSource clickSound;
    public AudioSource mainMenuMusic;
    public AudioSource shopMusic;
    public AudioSource creditsMusic;
    public AudioSource ballFalls;
    public AudioSource brickDestroyed;
    public AudioSource expolosion;
    public AudioSource gambling;
    public AudioSource gameOver;
    public AudioSource gaming;
    public AudioSource losePoints;
    public AudioSource powerUp;
    public AudioSource reboot;


    Dictionary<string, BlockType> blockTypeDict;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitBlockTypes();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void InitBlockTypes()
    {
        blockTypeDict = new Dictionary<string, BlockType>();
        foreach(var block in blockTypes)
        {
            blockTypeDict[block.typeName] = block;
        }
    }

    public BlockType GetBlockType(string typeName)
    {
        if(blockTypeDict.TryGetValue(typeName, out BlockType type)) return type;
        return null;
    }

    public void PlaySound(AudioSource audio, float volume)
    {
        audio.volume = volume;
        audio.Play();
    }

}
