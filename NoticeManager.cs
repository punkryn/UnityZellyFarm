using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
    public Text noticeText;
    public GameObject noticeBanner;
    bool isNegative = false;

    public void MakeNotice(string type)
    {
        string obj;
        switch (type)
        {
            case "Start":
                obj = "��� ������ �ر��ϴ� ���� ��ǥ";
                break;
            case "Clear":
                obj = "��� ������ �ر�";
                break;
            case "Sell":
                obj = "������ �巡���ؼ� �Ǹ�";
                break;
            case "NotJelatin":
                obj = "����ƾ�� �����մϴ�.";
                break;
            case "NotGold":
                obj = "��尡 �����մϴ�.";
                break;
            case "NotNum":
                obj = "���� ���뷮�� �����մϴ�.";
                break;
            default:
                obj = null;
                break;
        }

        if (type.Substring(0, 3) == "Not")
        {
            isNegative = true;
        }
        else
        {
            isNegative = false;
        }

        noticeText.text = obj;

        Change();
    }

    void Change()
    {
        Color c;
        if (isNegative)
        {
            c = new Color(0.925f, 0.4f, 0.4f);
        }
        else
        {
            c = new Color(0.396f, 0.925f, 0.651f);
        }

        noticeBanner.GetComponent<Image>().color = c;
        noticeBanner.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -3);

        StartCoroutine(Back());
    }

    IEnumerator Back()
    {
        yield return new WaitForSeconds(7f);

        noticeBanner.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
    }
}
