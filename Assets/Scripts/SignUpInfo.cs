using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUpInfo : MonoBehaviour
{
    [SerializeField] SignUp signUp;
    [SerializeField] GameObject loading;

    [Header("Default")]
    public TMP_InputField fieldID;
    public TMP_InputField fieldPW;
    public TMP_InputField fieldPWRe;
    public TMP_InputField fieldName;

    [Header("Date")]
    public TMP_InputField fieldYear;
    public TMP_Dropdown dropdownMonth;
    public TMP_InputField fieldDay;

    [Header("Other")]
    public TMP_Dropdown dropdownGender;
    public TMP_InputField fieldEmail;

    [Header("Error")]
    [SerializeField] TMP_Text errorTextID;      // 아이디의 에러 텍스트.
    [SerializeField] TMP_Text errorTextPW;      // 패스워드 에러 텍스트.
    [SerializeField] TMP_Text errorTextName;    // 이름 에러 텍스트.
    [SerializeField] TMP_Text errorTextBirthday;// 생년월일 에러 텍스트.
    [SerializeField] TMP_Text errorTextGender;  // 성별 에러 텍스트.

    bool isValidID;      // 아이디가 유효한가?   (4자 이상, 알파벳+숫자 조합)
    bool isNotOverlapID; // 아이디가 중복되지 않는가? (실제로 백엔드 서버에 중복 아이디가 존재하는가?)

    bool isValidPW;      // 비밀번호가 유효한가? (8 ~ 16자, 알파벳+숫자+특수문자 조합. 대문자 포함.)
    bool isEqualPW;      // 비밀번호가 동일한가?

    bool isValidName;       // 이름이 유효한가?
    bool isValidBirthday;   // 생년월일이 유효한가?
    bool isValidGender;     // 성별유형이 유효한가?
    
    private void OnEnable()
    {
        fieldID.text = string.Empty;
        fieldPW.text = string.Empty;
        fieldName.text = string.Empty;
        fieldYear.text = string.Empty;
        dropdownMonth.value = 0;
        fieldDay.text = string.Empty;
        dropdownGender.value = 0;
        fieldEmail.text = string.Empty;

        isValidID = false;
        isNotOverlapID = false;
        isValidPW = false;
        isEqualPW = false;
        isValidName = false;
        isValidBirthday = false;
        isValidGender = false;

        errorTextID.gameObject.SetActive(false);
        errorTextPW.gameObject.SetActive(false);
        errorTextName.gameObject.SetActive(false);
        errorTextBirthday.gameObject.SetActive(false);
        errorTextGender.gameObject.SetActive(false);
    }
    private void Start()
    {
        fieldName.onValueChanged.AddListener((str) => { OnChangedName(); });
        fieldName.onEndEdit.AddListener((str) => { OnEndEditName(); });

        fieldYear.onEndEdit.AddListener((str) => { OnEndEditBirthday(); });
        dropdownMonth.onValueChanged.AddListener((index) => { OnEndEditBirthday(); });
        fieldDay.onEndEdit.AddListener((str) => { OnEndEditBirthday(); });

        dropdownGender.onValueChanged.AddListener((index) => { OnEndEditGender(); });
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
        if (!isValidID)                          // 유효하지 않다면.
        {
            Debug.Log("아이디가 유효하지 않다.");
            return;                             // 중복체크 X.
        }

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
        isValidID = true;

        if (string.IsNullOrEmpty(id))
        {
            text = "아이디가 입력되지 않았습니다.";
            isValidID = false;
        }
        else if (id.Length < 4)
        {
            text = "아이디는 최소 4글자 이상입니다.";
            isValidID = false;
        }
        else if (!IsEnglisOrNumber(id))
        {
            text = "아이디는 알파벳 + 문자의 조합이어야만 합니다.";
            isValidID = false;
        }

        if (!isValidID)
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
        string text = string.Empty;
        isValidPW = true;

        //(8 ~ 16자, 알파벳+숫자+특수문자 조합. 대문자 포함.)
        if (pw.Length < 8 || 16 < pw.Length)
        {
            text = "8 ~ 16자, 알파벳+숫자+특수문자 조합. 대문자 포함.";
            isValidPW = false;            
        }
        // 기타 다른 조건식 (생략)
        // ...

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

        if (string.Equals(fieldPW.text, fieldPWRe.text))
        {
            errorTextPW.text = "비밀번호가 동일합니다.";
            errorTextPW.color = Color.green;
            isEqualPW = true;
        }
        else
        {
            errorTextPW.text = "비밀번호가 동일하지 않습니다.";
            errorTextPW.color = Color.red;
            isEqualPW = false;
        }
    }


    #endregion

    #region 이름, 생년월일

    public void OnChangedName()
    {
        errorTextName.gameObject.SetActive(false);
    }
    public void OnEndEditName()
    {
        isValidName = !string.IsNullOrEmpty(fieldName.text);
        errorTextName.gameObject.SetActive(!isValidName);
        errorTextName.text = "이름을 입력해주세요.";
    }

    public void OnEndEditBirthday()
    {
        errorTextBirthday.gameObject.SetActive(true);
        isValidBirthday = false;

        if (string.IsNullOrEmpty(fieldYear.text))
        {
            errorTextBirthday.text = "년도를 입력해주세요.";
        }
        else if (dropdownMonth.value < 1)
        {
            errorTextBirthday.text = "월을 입력해주세요";
        }
        else if (string.IsNullOrEmpty(fieldDay.text))
        {
            errorTextBirthday.text = "일을 입력해주세요.";
        }
        else
        {
            // 생년월일이 유효하다.
            errorTextBirthday.gameObject.SetActive(false);
            isValidBirthday = true;
        }
                
    }
    public void OnEndEditGender()
    {
        isValidGender = dropdownGender.value > 0;
        errorTextGender.gameObject.SetActive(!isValidGender);
        errorTextGender.text = "성별을 선택해주세요.";
    }

    #endregion

    public void OnConfirm()
    {
        // 아이디가 유효하지 않을 경우.
        if (!isValidID || !isNotOverlapID)
        {
            Debug.Log("아이디가 유효하지 않음");
            return;
        }

        // 패스워드가 유효하지 않을 경우.
        if (!isValidPW || !isEqualPW)
        {
            Debug.Log("패스워드 문제");
            return;
        }

        // 이름이 비었을 경우.
        if (!isValidName)
        {
            Debug.Log("이름 비었다.");
            return;
        }

        // 생년월일이 제대로 입력되지 않았을 겨우.
        if(!isValidBirthday)
        {
            Debug.Log("생년월일 오류");
            return;
        }

        // 설별이 제대로 선택되지 않았을 경우.
        if(!isValidGender)
        {
            Debug.Log("성별 오류");
            return;
        }

        // 회원가입 시도.
        loading.gameObject.SetActive(true);
        signUp.OnSignup(OnEndSignup);
    }
    private void OnEndSignup(bool isSuccess)
    {
        loading.gameObject.SetActive(false);
    }

}
