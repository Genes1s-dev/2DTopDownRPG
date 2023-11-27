using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControlsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject showControlsUI;
    const string showText = "Show player controls";
    const string hideText = "Hide player controls";
    bool visible;

    public void ToggleControlsUI()
    {
        if (visible)
        {
            showControlsUI.SetActive(false);
            buttonText.text = hideText;
        } else {
            showControlsUI.SetActive(true);
            buttonText.text = showText;
        }
        visible = !visible;
    }
}
