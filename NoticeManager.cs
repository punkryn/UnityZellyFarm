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
                obj = "모든 젤리를 해금하는 것이 목표";
                break;
            case "Clear":
                obj = "모든 젤리를 해금";
                break;
            case "Sell":
                obj = "젤리를 드래그해서 판매";
                break;
            case "NotJelatin":
                obj = "젤라틴이 부족합니다.";
                break;
            case "NotGold":
                obj = "골드가 부족합니다.";
                break;
            case "NotNum":
                obj = "젤리 수용량이 부족합니다.";
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
