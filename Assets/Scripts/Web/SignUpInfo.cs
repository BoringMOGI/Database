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
    [SerializeField] TMP_Text errorTextID;      // ���̵��� ���� �ؽ�Ʈ.
    [SerializeField] TMP_Text errorTextPW;      // �н����� ���� �ؽ�Ʈ.
    [SerializeField] TMP_Text errorTextName;    // �̸� ���� �ؽ�Ʈ.
    [SerializeField] TMP_Text errorTextBirthday;// ������� ���� �ؽ�Ʈ.
    [SerializeField] TMP_Text errorTextGender;  // ���� ���� �ؽ�Ʈ.

    bool isValidID;      // ���̵� ��ȿ�Ѱ�?   (4�� �̻�, ���ĺ�+���� ����)
    bool isNotOverlapID; // ���̵� �ߺ����� �ʴ°�? (������ �鿣�� ������ �ߺ� ���̵� �����ϴ°�?)

    bool isValidPW;      // ��й�ȣ�� ��ȿ�Ѱ�? (8 ~ 16��, ���ĺ�+����+Ư������ ����. �빮�� ����.)
    bool isEqualPW;      // ��й�ȣ�� �����Ѱ�?

    bool isValidName;       // �̸��� ��ȿ�Ѱ�?
    bool isValidBirthday;   // ��������� ��ȿ�Ѱ�?
    bool isValidGender;     // ���������� ��ȿ�Ѱ�?
    
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

    #region ���̵�

    public void OnChangedID(TMP_InputField textID)
    {
        // ���̵� �����߱� ������ ��ȿ���� üũ ������ �ʱ�ȭ ��Ų��.
        isValidID = false;
        isNotOverlapID = false;

        errorTextID.gameObject.SetActive(false);
    }
    public void OnEndEditID(TMP_InputField textID)
    {          
        // ���̵�� �ּ� 4���� �̻�, ���ĺ� + ���ڸ� ����.
        string id = textID.text;
        IsCheckValiedID(id);
    }
    public void OnDoubleCheckID(TMP_InputField textID)
    {
        IsCheckValiedID(textID.text);           // ��ȿ�� üũ.
        if (!isValidID)                          // ��ȿ���� �ʴٸ�.
        {
            Debug.Log("���̵� ��ȿ���� �ʴ�.");
            return;                             // �ߺ�üũ X.
        }

        // �Է��� ���̵��� �ߺ� üũ�� ��Ų��.
        string id = textID.text;
        WebManager.Instance.Post(CallbackDoubleCheckID, "doubleCheckID", new StringFair("id", id));
    }
    private void CallbackDoubleCheckID(WebManager.WebData data)
    {
        // ������ ������ ���� ��� ���� ���� ���� ����.
        isNotOverlapID = string.Equals(data.result, "ok");

        errorTextID.gameObject.SetActive(true);
        errorTextID.text = isNotOverlapID ? "��� ������ ���̵��Դϴ�." : "�ߺ��� ���̵��Դϴ�.";
        errorTextID.color = isNotOverlapID ? Color.green : Color.red;
    }

    private void IsCheckValiedID(string id)
    {
        string text = string.Empty;
        isValidID = true;

        if (string.IsNullOrEmpty(id))
        {
            text = "���̵� �Էµ��� �ʾҽ��ϴ�.";
            isValidID = false;
        }
        else if (id.Length < 4)
        {
            text = "���̵�� �ּ� 4���� �̻��Դϴ�.";
            isValidID = false;
        }
        else if (!IsEnglisOrNumber(id))
        {
            text = "���̵�� ���ĺ� + ������ �����̾�߸� �մϴ�.";
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
        // id�� ����� ���ڸ� ���ԵǾ� �ִ��� Ȯ���ϴ� �Լ�.
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

    #region ��й�ȣ

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

        //(8 ~ 16��, ���ĺ�+����+Ư������ ����. �빮�� ����.)
        if (pw.Length < 8 || 16 < pw.Length)
        {
            text = "8 ~ 16��, ���ĺ�+����+Ư������ ����. �빮�� ����.";
            isValidPW = false;            
        }
        // ��Ÿ �ٸ� ���ǽ� (����)
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
            errorTextPW.text = "��й�ȣ�� �����մϴ�.";
            errorTextPW.color = Color.green;
            isEqualPW = true;
        }
        else
        {
            errorTextPW.text = "��й�ȣ�� �������� �ʽ��ϴ�.";
            errorTextPW.color = Color.red;
            isEqualPW = false;
        }
    }


    #endregion

    #region �̸�, �������

    public void OnChangedName()
    {
        errorTextName.gameObject.SetActive(false);
    }
    public void OnEndEditName()
    {
        isValidName = !string.IsNullOrEmpty(fieldName.text);
        errorTextName.gameObject.SetActive(!isValidName);
        errorTextName.text = "�̸��� �Է����ּ���.";
    }

    public void OnEndEditBirthday()
    {
        errorTextBirthday.gameObject.SetActive(true);
        isValidBirthday = false;

        if (string.IsNullOrEmpty(fieldYear.text))
        {
            errorTextBirthday.text = "�⵵�� �Է����ּ���.";
        }
        else if (dropdownMonth.value < 1)
        {
            errorTextBirthday.text = "���� �Է����ּ���";
        }
        else if (string.IsNullOrEmpty(fieldDay.text))
        {
            errorTextBirthday.text = "���� �Է����ּ���.";
        }
        else
        {
            // ��������� ��ȿ�ϴ�.
            errorTextBirthday.gameObject.SetActive(false);
            isValidBirthday = true;
        }
                
    }
    public void OnEndEditGender()
    {
        isValidGender = dropdownGender.value > 0;
        errorTextGender.gameObject.SetActive(!isValidGender);
        errorTextGender.text = "������ �������ּ���.";
    }

    #endregion

    public void OnConfirm()
    {
        // ���̵� ��ȿ���� ���� ���.
        if (!isValidID || !isNotOverlapID)
        {
            Debug.Log("���̵� ��ȿ���� ����");
            return;
        }

        // �н����尡 ��ȿ���� ���� ���.
        if (!isValidPW || !isEqualPW)
        {
            Debug.Log("�н����� ����");
            return;
        }

        // �̸��� ����� ���.
        if (!isValidName)
        {
            Debug.Log("�̸� �����.");
            return;
        }

        // ��������� ����� �Էµ��� �ʾ��� �ܿ�.
        if(!isValidBirthday)
        {
            Debug.Log("������� ����");
            return;
        }

        // ������ ����� ���õ��� �ʾ��� ���.
        if(!isValidGender)
        {
            Debug.Log("���� ����");
            return;
        }

        // ȸ������ �õ�.
        loading.gameObject.SetActive(true);
        signUp.OnSignup(OnEndSignup);
    }
    private void OnEndSignup(bool isSuccess)
    {
        loading.gameObject.SetActive(false);
    }

}
