using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    static Loading instance;
    public static Loading Instance => instance;

    [SerializeField] GameObject panel;
    [SerializeField] TMPro.TMP_Text commentText;

    private void Awake()
    {
        instance = this;
    }

    public void Show(string comment)
    {
        panel.SetActive(true);
        commentText.text = comment;
    }
    public void Close()
    {
        panel.SetActive(false);
    }

}
