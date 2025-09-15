using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CreditEntry
{
    public string title;
    public string role;
    public string name;
}

[System.Serializable]
public class CreditData
{
    public List<CreditEntry> credits;
}

public class CreditsCode : MonoBehaviour
{
    public RectTransform creditsContent;
    public GameObject textPrefab; // A prefab with a TMP_Text

    float scrollSpeed = 65f;

    private void Start()
    {
        string json = Resources.Load<TextAsset>("creditsText").text;
        CreditData data = JsonUtility.FromJson<CreditData>(json);

        // Build all the text first
        foreach (var entry in data.credits)
        {
            if (!string.IsNullOrEmpty(entry.title))
                AddText(entry.title, Color.yellow, 36, true);
            else if (!string.IsNullOrEmpty(entry.role))
            {
                AddText(entry.role, Color.red, 28, true);
                if (!string.IsNullOrEmpty(entry.name))
                    AddText(entry.name, Color.white, 24, false);
            }
        }

        // Move the content below the screen
        creditsContent.anchoredPosition = new Vector2(0, -Screen.height - 1200f);
    }

    void AddText(string content, Color color, int fontSize, bool bold)
    {
        GameObject go = Instantiate(textPrefab, creditsContent);
        var tmp = go.GetComponent<TMP_Text>();

        tmp.text = content;
        tmp.color = color;
        tmp.fontSize = fontSize;
        tmp.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    private void Update()
    {
        creditsContent.anchoredPosition += Vector2.up * (scrollSpeed * Time.deltaTime);
    }
}
