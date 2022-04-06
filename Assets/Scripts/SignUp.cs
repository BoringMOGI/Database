using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SignUp : MonoBehaviour
{
    [SerializeField] SignUpAgree agree;
    [SerializeField] SignUpInfo info;

    // 회원가입 시도.
    System.Action<bool> OnEndSignup;

    public void OnSignup(System.Action<bool> OnEndSignup)
    {
        this.OnEndSignup = OnEndSignup;

        List<StringFair> list = new List<StringFair>();
        list.Add(new StringFair("id", info.fieldID.text));
        list.Add(new StringFair("pw", info.fieldPW.text));
        list.Add(new StringFair("name", info.fieldName.text));

        string birthday = string.Format("{0}-{1}-{2}", info.fieldYear.text, info.dropdownMonth.value, info.fieldDay.text);
        list.Add(new StringFair("birthday", birthday));

        list.Add(new StringFair("gender", info.dropdownGender.value.ToString()));
        list.Add(new StringFair("email", info.fieldEmail.text));

        list.Add(new StringFair("agree1", agree.Agree1 ? "T" : "F"));
        list.Add(new StringFair("agree2", agree.Agree2 ? "T" : "F"));

        WebManager.Instance.Post(OnResult, "signUp", list.ToArray());
    }

    private void OnResult(WebManager.WebData data)
    {
        OnEndSignup?.Invoke(data.result == "ok");
        Debug.Log($"결과 : {data.result}, 메시지 : {data.msg}");
    }

}

