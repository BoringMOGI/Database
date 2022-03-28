using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

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
    [SerializeField] WebData data;

    delegate void Callback(WebData data);   // string�� �޴� �ݹ�.
    bool isNetworking;                      // ���� ��� ���ΰ�?

    private void Start()
    {
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

    public void OnLogin()
    {
        if (isNetworking)
            return;

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", idField.text);
        form.AddField("pw", pwField.text);

        StartCoroutine(WebPost(form, (data) => {
            Debug.Log($"��� : {data.result}");
            Debug.Log($"�޼��� : {data.msg}");
        }));
    }
    public void OnSignUp()
    {
        if (isNetworking)
            return;

        Debug.Log("ȸ������ �õ�");

        WWWForm form = new WWWForm();
        form.AddField("order", "signUp");
        form.AddField("id", idField.text);
        form.AddField("pw", pwField.text);

        StartCoroutine(WebPost(form, (data) => {

            if (data.result == "ok")
            {
                Debug.Log("ȸ�� ���Կ� �����߽��ϴ�.");
            }
            else if(data.result == "fail")
            {
                Debug.Log($"ȸ�� ���� ���� : {data.msg}");
            }
        }));
    }

    private IEnumerator WebPost(WWWForm form, Callback callback)
    {
        isNetworking = true;
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            // �����κ��� ������ �Դٸ�.
            if (www.isDone)                                  
            {
                // ������ �������� ���ڿ� �����͸� Json�� �̿��� WebData��ü�� ��ȯ.
                // ��ü�� Callback���� ����.
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
