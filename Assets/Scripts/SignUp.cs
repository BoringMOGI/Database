using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SignUp : MonoBehaviour
{
    [SerializeField] Toggle agreeAll;
    [SerializeField] Toggle agreeForce;
    [SerializeField] Toggle agreeSelect;
    [SerializeField] TMP_Text errorText;

    // 1.toggle���� üũ ������ �ƴ� bool ������ �ִ�. (isOn)
    // 2.toogle�� ������ �� �����ϴ� �̺�Ʈ �Լ��� �ִ�.

    [SerializeField] EventSystem es;

    private void OnEnable()
    {
        agreeAll.isOn = false;
        OnChangedAgree(agreeAll);
        errorText.gameObject.SetActive(false);
    }

    public void OnChangedAgree(Toggle toggle)
    {
        if (toggle == agreeAll)
        {
            agreeForce.isOn = toggle.isOn;
            agreeSelect.isOn = toggle.isOn;
            return;
        }

        agreeAll.isOn = agreeForce.isOn && agreeSelect.isOn;
    }
    public void OnConfirm()
    {
        if (agreeForce.isOn == false)
        {
            errorText.gameObject.SetActive(true);
            return;
        }

        errorText.gameObject.SetActive(false);
        Debug.Log("���� �Ϸ�");
    }
}
