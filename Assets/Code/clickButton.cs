using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickButton : MonoBehaviour
{
    public createSkins skinController;

    public void changeState()
    {
        if (skinController != null)
        {
            skinController.changeGambling();
        }
    }
}
