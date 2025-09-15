using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopUp : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    float speed = 0.02f;


    public void createInfoWindow(TextMeshProUGUI textStart, GameObject window)
    {
            string text = textStart.text;
            window.SetActive(true);
            StartCoroutine(TypeText(text, textStart));

        if(buttons.Count > 0)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];
                button = button.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => window.SetActive(false));
            }
        }
            
    }

    private IEnumerator TypeText(string text, TextMeshProUGUI textType)
    {
        textType.text = "";
        foreach(char c in text)
        {
            textType.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    

}
