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
        // 윈폼 : 웹에 데이터를 보낼 때 도와주는 형식.
        WWWForm form = new WWWForm();
        form.AddField("type", "id");

        UnityWebRequest www = UnityWebRequest.Post(URL, form);      // 데이터를 실어서 웹에 전송.
        yield return www.SendWebRequest();                          // 응답 요청.
        string data = www.downloadHandler.text;

        Debug.Log(data);
    }





    // 시트에 직접 접근해서 받아오는 형식.
    [SerializeField] string sheetId;
    const string URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=tsv&gid=2126502618";
    private string GetURL()
    {
        // 시트 ID를 이용해 URL 주소 편집.
        return string.Format(URL_FORMAT, sheetId);
    }
    private IEnumerator DownloadTSV()
    {
        string url = GetURL();                                  // 주소.
        UnityWebRequest www = UnityWebRequest.Get(url);         // WebRequest를 통해 url읽기.
        yield return www.SendWebRequest();                      // 주소를 통해 웹에 요청.
        string data = www.downloadHandler.text;                 // 요청 받은 text데이터 대입.
        Debug.Log(data);                                        // 출력.
    }
}

