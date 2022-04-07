using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct StringFair
{
    public string Key { get; private set; }
    public string Value { get; private set; }

    public StringFair(string key, string value)
    {
        this.Key = key;
        this.Value = value;
    }
}

public class WebManager : MonoBehaviour
{
    #region 상수

    private const string URL = "https://script.google.com/macros/s/AKfycbwnbZFBuqJZPEUruCK41JYBiRlgZGCLylK5DD2J2n1XL7C71b8gVZCgP9V0KekgSypx9w/exec";
    static WebManager instance;
    public static WebManager Instance => instance;

    [System.Serializable] public class WebData
    {
        public string order;
        public string result;
        public string msg;
        public int value;
    }

    public delegate void Callback(WebData data);   // WebData 자료형을 매개변수로 받는 델리게이트.

    #endregion

    private bool isNetworking;
    Callback callback;

    private void Awake()
    {
        instance = this;
    }

    public bool Post(Callback callback, string order, params StringFair[] postDatas)
    {
        Debug.Log($"Post : {order}, isNetworking : {isNetworking}");
        
        if (isNetworking)
            return false;

        this.callback = callback;

        WWWForm form = new WWWForm();
        form.AddField("order", order);

        foreach (StringFair data in postDatas)
            form.AddField(data.Key, data.Value);

        StartCoroutine(WebPost(form));
        return true;
    }
    private IEnumerator WebPost(WWWForm form)
    {
        isNetworking = true;
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            // 웹으로부터 응답이 왔다면.
            if (www.isDone)
            {
                // 웹에서 내려받은 문자열 데이터를 Json을 이용해 WebData객체로 변환.
                // 객체를 Callback으로 리턴.
                string json = www.downloadHandler.text;
                WebData webData = (WebData)JsonUtility.FromJson(json, typeof(WebData));
                callback?.Invoke(webData);
                Debug.Log($"Callback : {webData.msg}");
            }
            else
            {
                callback?.Invoke(null);
                Debug.Log($"Callback : 실패");
            }

            isNetworking = false;
        }
    }
}
