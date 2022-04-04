using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUpInfo : MonoBehaviour
{
    [SerializeField] TMP_InputField fieldID;
    [SerializeField] TMP_InputField fieldPW;
    [SerializeField] TMP_InputField fieldPWRe;

    [SerializeField] TMP_Text errorTextID;      // 아이디의 에러 텍스트.
    [SerializeField] TMP_Text errorTextPW;

    bool isValidID;      // 아이디가 유효한가?   (4자 이상, 알파벳+숫자 조합)
    bool isNotOverlapID; // 아이디가 중복되는가? (실제로 백엔드 서버에 중복 아이디가 존재하는가?)

    bool isValidPW;      // 비밀번호가 유효한가? (8 ~ 16자, 알파벳+숫자+특수문자 조합. 대문자 포함.)
    bool isEqualPW;      // 비밀번호가 동일한가?

    private void OnEnable()
    {
        errorTextID.gameObject.SetActive(false);
    }

    #region 아이디

    public void OnChangedID(TMP_InputField textID)
    {
        // 아이디를 변경했기 때문에 유효성과 체크 유무를 초기화 시킨다.
        isValidID = false;
        isNotOverlapID = false;

        errorTextID.gameObject.SetActive(false);
    }
    public void OnEndEditID(TMP_InputField textID)
    {          
        // 아이디는 최소 4글자 이상, 알파벳 + 숫자만 가능.
        string id = textID.text;
        IsCheckValiedID(id);
    }
    public void OnDoubleCheckID(TMP_InputField textID)
    {
        IsCheckValiedID(textID.text);           // 유효성 체크.
        if(!isValidID)                          // 유효하지 않다면.
            return;                             // 중복체크 X.

        // 입력한 아이디의 중복 체크를 시킨다.
        string id = textID.text;
        WebManager.Instance.Post(CallbackDoubleCheckID, "doubleCheckID", new StringFair("id", id));
    }
    private void CallbackDoubleCheckID(WebManager.WebData data)
    {
        // 실제로 웹에서 받은 결과 값을 토대로 변수 세팅.
        isNotOverlapID = string.Equals(data.result, "ok");

        errorTextID.gameObject.SetActive(true);
        errorTextID.text = isNotOverlapID ? "사용 가능한 아이디입니다." : "중복된 아이디입니다.";
        errorTextID.color = isNotOverlapID ? Color.green : Color.red;
    }

    private void IsCheckValiedID(string id)
    {
        string text = string.Empty;
        bool isValid = true;

        if (string.IsNullOrEmpty(id))
        {
            text = "아이디가 입력되지 않았습니다.";
            isValid = false;
        }
        else if (id.Length < 4)
        {
            text = "아이디는 최소 4글자 이상입니다.";
            isValid = false;
        }
        else if (!IsEnglisOrNumber(id))
        {
            text = "아이디는 알파벳 + 문자의 조합이어야만 합니다.";
            isValid = false;
        }

        if (!isValid)
        {
            errorTextID.gameObject.SetActive(true);
            errorTextID.text = text;
        }
    }
    private bool IsEnglisOrNumber(string id)
    {
        // id에 영어와 숫자만 포함되어 있는지 확인하는 함수.
        foreach (char c in id)
        {
            if (!(IsEnglish(c) || IsNumeric(c)))
                return false;
        }

        return true;
    }

    bool IsKorean(char ch)
    {
        return (0xAC00 <= ch && ch <= 0xD7A3) || (0x3131 <= ch && ch <= 0x318E);
    }
    bool IsNumeric(char ch)
    {
        return (0x30 <= ch && ch <= 0x39);
    }
    bool IsEnglish(char ch)
    {
        return (0x61 <= ch && ch <= 0x7A) || (0x41 <= ch && ch <= 0x5A);
    }

    #endregion

    #region 비밀번호

    public void OnChangedPW()
    {
        isValidPW = false;
        isEqualPW = false;
        errorTextPW.gameObject.SetActive(false);
    }

    private void IsCheckValidPW(string pw)
    {
        //(8 ~ 16자, 알파벳+숫자+특수문자 조합. 대문자 포함.)
        string text = string.Empty;
        if (pw.Length < 8 || 16 < pw.Length)
        {
            text = "8 ~ 16자, 알파벳+숫자+특수문자 조합. 대문자 포함.";
            isValidPW = false;
        }

        errorTextPW.gameObject.SetActive(!isValidPW);
        errorTextPW.color = Color.red;
        errorTextPW.text = text;
    }
    public void OnEndEditPW()
    {
        string pw = fieldPW.text;
        IsCheckValidPW(pw);
    }
    public void OnEndEditPWRe()
    {
        errorTextPW.gameObject.SetActive(true);
        IsCheckValidPW(fieldPWRe.text);
        
        if(!isValidPW)
        {
        }
        else if(string.Equals(fieldPW.text, fieldPWRe.text))
        {
            errorTextPW.text = "비밀번호가 동일합니다.";
            errorTextPW.color = Color.green;
        }
        else
        {
            errorTextPW.text = "비밀번호가 동일하지 않습니다.";
            errorTextPW.color = Color.red;
        }
    }


    #endregion


}
