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


    delegate void Callback(WebData data);   // string을 받는 콜백.
    bool isLogin;
  
    private void Start()
    {
        errorText.gameObject.SetActive(false);
        idField.onValueChanged.AddListener((str) => { errorText.gameObject.SetActive(false); });
        pwField.onValueChanged.AddListener((str) => { errorText.gameObject.SetActive(false); });
    }
    private void OnEnable()
    {
        // 각각의 필드를 초기 상태로 되돌린다.
        isLogin = false;
        idField.text = string.Empty;
        pwField.text = string.Empty;
        errorText.gameObject.SetActive(false);
    }

    public void OnLogin()
    {
        // 로그인 중일 때는 실행하지 않는다.
        if (isLogin)
            return;

        // 아이디 혹은 비밀번호가 비어있을 때.
        if (string.IsNullOrEmpty(idField.text) || string.IsNullOrEmpty(pwField.text))
        {
            errorText.text = "아이디 혹은 비밀번호를 입력하세요";
            errorText.gameObject.SetActive(true);
            return;
        }

        isLogin = true;
        Loading.Instance.Show("로그인 중...");
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
