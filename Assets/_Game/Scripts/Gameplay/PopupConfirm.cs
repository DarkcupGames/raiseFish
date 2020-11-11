using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupConfirm : MonoBehaviour
{
    public Action yes;
    public Action no;
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    public void Show(string title, string content, Action actionYes = null, Action actionNo = null)
    {
        this.title.text = title;
        this.content.text = content;
        yes = actionYes;
        no = actionNo;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void OnYes()
    {
        if (yes == null)
        {
            Hide();
            return;
        }

        yes.Invoke();
    }

    public void OnNo()
    {
        if (no == null)
        {
            Hide();
            return;
        }

        no.Invoke();
    }
}
