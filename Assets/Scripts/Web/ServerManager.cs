using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class ServerManager : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycbzX7b3X3mwSdBvAJZKr-aGVUq6x0bZS74q8VO54nJ6yofKcwAIrKmNeMtU0DNUAiGI5/exec";
    private IEnumerator Start()
    {
        // ���� : ���� �����͸� ���� �� �����ִ� ����.
        WWWForm form = new WWWForm();
        form.AddField("type", "id");

        UnityWebRequest www = UnityWebRequest.Post(URL, form);      // �����͸� �Ǿ ���� ����.
        yield return www.SendWebRequest();                          // ���� ��û.
        string data = www.downloadHandler.text;

        Debug.Log(data);
    }





    // ��Ʈ�� ���� �����ؼ� �޾ƿ��� ����.
    [SerializeField] string sheetId;
    const string URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=tsv&gid=2126502618";
    private string GetURL()
    {
        // ��Ʈ ID�� �̿��� URL �ּ� ����.
        return string.Format(URL_FORMAT, sheetId);
    }
    private IEnumerator DownloadTSV()
    {
        string url = GetURL();                                  // �ּ�.
        UnityWebRequest www = UnityWebRequest.Get(url);         // WebRequest�� ���� url�б�.
        yield return www.SendWebRequest();                      // �ּҸ� ���� ���� ��û.
        string data = www.downloadHandler.text;                 // ��û ���� text������ ����.
        Debug.Log(data);                                        // ���.
    }
}

