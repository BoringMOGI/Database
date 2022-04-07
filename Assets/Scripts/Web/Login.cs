using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Events;

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
    [SerializeField] UnityEvent onLogin;


    delegate void Callback(WebData data);   // string�� �޴� �ݹ�.
    bool isLogin;
  
    private void Start()
    {
        errorText.gameObject.SetActive(false);
        idField.onValueChanged.AddListener((str) => { errorText.gameObject.SetActive(false); });
        pwField.onValueChanged.AddListener((str) => { errorText.gameObject.SetActive(false); });
    }
    private void OnEnable()
    {
        // ������ �ʵ带 �ʱ� ���·� �ǵ�����.
        isLogin = false;
        idField.text = string.Empty;
        pwField.text = string.Empty;
        errorText.gameObject.SetActive(false);
    }

    public void OnLogin()
    {
        // �α��� ���� ���� �������� �ʴ´�.
        if (isLogin)
            return;

        // ���̵� Ȥ�� ��й�ȣ�� ������� ��.
        if (string.IsNullOrEmpty(idField.text) || string.IsNullOrEmpty(pwField.text))
        {
            errorText.text = "���̵� Ȥ�� ��й�ȣ�� �Է��ϼ���";
            errorText.gameObject.SetActive(true);
            return;
        }

        isLogin = true;
        Loading.Instance.Show("�α��� ��...");
        WebManager.Instance.Post(WebCallback, "login",
            new StringFair("id", idField.text), new StringFair("pw", pwField.text));
    }
    private void WebCallback(WebManager.WebData data)
    {
        isLogin = false;
        Loading.Instance.Close();

        if(data.result == "ok")
        {
            onLogin?.Invoke();
        }
        else
        {
            errorText.text = data.msg;
            errorText.gameObject.SetActive(true);
        }
    }

}
