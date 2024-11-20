using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image barImage;

    void Awake()
    {
        if (barImage.type != Image.Type.Filled)
        {
            Debug.LogError($"{name}'s image type is not of type \"Filled \" and so cannot be used as a progress bar image." +
            "Disabling this progress bar.");
            this.enabled = false;
        }
        else
        {
            Reset();
        }
    }

    public void SetProgress(float amount)
    {
        print("Setting size");
        print(amount);
        barImage.fillAmount = amount;
    }

    public void Reset()
    {
        barImage.fillAmount = 0;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
