using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpAgree : MonoBehaviour
{
    [System.Serializable]
    public struct Agree
    {
        public Toggle all;
        public Toggle force;
        public Toggle select;
        public TMP_Text errorText;
    }

    [SerializeField] Agree agree;


    // 1.toggle���� üũ ������ �ƴ� bool ������ �ִ�. (isOn)
    // 2.toogle�� ������ �� �����ϴ� �̺�Ʈ �Լ��� �ִ�.

    private void OnEnable()
    {
        agree.all.isOn = false;
        OnChangedAgree(agree.all);
        agree.errorText.gameObject.SetActive(false);
    }

    public void OnChangedAgree(Toggle toggle)
    {
        if (toggle == agree.all)
        {
            agree.force.isOn = toggle.isOn;
            agree.select.isOn = toggle.isOn;
            return;
        }

        agree.all.isOn = agree.force.isOn && agree.select.isOn;
    }
    public void OnConfirm()
    {
        if (agree.force.isOn == false)
        {
            agree.errorText.gameObject.SetActive(true);
            return;
        }

        agree.errorText.gameObject.SetActive(false);
        Debug.Log("���� �Ϸ�");
    }
}
