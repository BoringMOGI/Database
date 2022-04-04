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

    [SerializeField] TMP_Text errorTextID;      // ���̵��� ���� �ؽ�Ʈ.
    [SerializeField] TMP_Text errorTextPW;

    bool isValidID;      // ���̵� ��ȿ�Ѱ�?   (4�� �̻�, ���ĺ�+���� ����)
    bool isNotOverlapID; // ���̵� �ߺ��Ǵ°�? (������ �鿣�� ������ �ߺ� ���̵� �����ϴ°�?)

    bool isValidPW;      // ��й�ȣ�� ��ȿ�Ѱ�? (8 ~ 16��, ���ĺ�+����+Ư������ ����. �빮�� ����.)
    bool isEqualPW;      // ��й�ȣ�� �����Ѱ�?

    private void OnEnable()
    {
        errorTextID.gameObject.SetActive(false);
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
        if(!isValidID)                          // ��ȿ���� �ʴٸ�.
            return;                             // �ߺ�üũ X.

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
        bool isValid = true;

        if (string.IsNullOrEmpty(id))
        {
            text = "���̵� �Էµ��� �ʾҽ��ϴ�.";
            isValid = false;
        }
        else if (id.Length < 4)
        {
            text = "���̵�� �ּ� 4���� �̻��Դϴ�.";
            isValid = false;
        }
        else if (!IsEnglisOrNumber(id))
        {
            text = "���̵�� ���ĺ� + ������ �����̾�߸� �մϴ�.";
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
        //(8 ~ 16��, ���ĺ�+����+Ư������ ����. �빮�� ����.)
        string text = string.Empty;
        if (pw.Length < 8 || 16 < pw.Length)
        {
            text = "8 ~ 16��, ���ĺ�+����+Ư������ ����. �빮�� ����.";
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
            errorTextPW.text = "��й�ȣ�� �����մϴ�.";
            errorTextPW.color = Color.green;
        }
        else
        {
            errorTextPW.text = "��й�ȣ�� �������� �ʽ��ϴ�.";
            errorTextPW.color = Color.red;
        }
    }


    #endregion


}
