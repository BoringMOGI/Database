using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;

public class Login : MonoBehaviour
{
    [System.Serializable]
    class WebData
    {
        public string order;
        public string result;
        public string msg;
        public int value;
    }


    const string URL = "https://script.google.com/macros/s/AKfycbwnbZFBuqJZPEUruCK41JYBiRlgZGCLylK5DD2J2n1XL7C71b8gVZCgP9V0KekgSypx9w/exec";
    
    [SerializeField] InputField idField;
    [SerializeField] InputField pwField;
    [SerializeField] Text errorText;
    [SerializeField] WebData data;

    delegate void Callback(WebData data);   // string을 받는 콜백.
    bool isNetworking;                      // 현재 통신 중인가?

    class Item
    {

    }

    private void Start()
    {
        errorText.gameObject.SetActive(false);
        idField.onValueChanged.AddListener((str) => { errorText.gameObject.SetActive(false); });
        pwField.onValueChanged.AddListener((str) => { errorText.gameObject.SetActive(false); });

        Item item1 = new Item();
        Item item2 = new Item();
        Item item3 = new Item();

        List<Item> list = new List<Item>();
        list.Add(item1);
        list.Add(item2);
        list.Add(item3);

        list.Remove(item2);


        /*
        string json = JsonUtility.ToJson(data, true);
        string path = string.Format("{0}/{1}.txt", Application.dataPath, "savefile");
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(json);
        }
        */

        /*
        string path = string.Format("{0}/{1}.txt", Application.dataPath, "savefile");
        using (StreamReader sr = new StreamReader(path))
        {
            string json = sr.ReadToEnd();
            data = (WebData)JsonUtility.FromJson(json, typeof(WebData));
        }
        */
    }

    void OnChangeValueID(string str)
    {

    }

    public void OnLogin()
    {
        if (isNetworking)
            return;

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", idField.text);
        form.AddField("pw", pwField.text);

        StartCoroutine(WebPost(form, (data) => {
            errorText.gameObject.SetActive(true);
            errorText.text = data.msg;
        }));
    }
    public void OnSignUp()
    {
        if (isNetworking)
            return;

        Debug.Log("회원가입 시도");

        WWWForm form = new WWWForm();
        form.AddField("order", "signUp");
        form.AddField("id", idField.text);
        form.AddField("pw", pwField.text);

        StartCoroutine(WebPost(form, (data) => {

            if (data.result == "ok")
            {
                Debug.Log("회원 가입에 성공했습니다.");
            }
            else if(data.result == "fail")
            {
                Debug.Log($"회원 가입 실패 : {data.msg}");
                errorText.gameObject.SetActive(true);
                errorText.text = data.msg;
            }
        }));
    }

    private IEnumerator WebPost(WWWForm form, Callback callback)
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
            }
            else
            {
                callback?.Invoke(null);
            }

            isNetworking = false;
        }
    }

}
