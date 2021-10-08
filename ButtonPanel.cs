using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour
{
    bool isClick;
    public Sprite showSprite;
    public Sprite hideSprite;
    public GameObject panel;
    public GameObject anotherBtn;

    public GameObject optionPanel;
    ButtonOption btnOp;

    Animator anim;

    ButtonPanel bp;

    public GameObject manager;
    GameManager gameManager;

    // Sound System
    public GameObject soundManager;
    SoundManager sm;

    // Notice Manager
    public GameObject noticeManager;
    NoticeManager nm;

    void Awake()
    {
        anim = panel.GetComponent<Animator>();
        gameManager = manager.GetComponent<GameManager>();
        bp = anotherBtn.GetComponent<ButtonPanel>();
        btnOp = optionPanel.GetComponent<ButtonOption>();
        sm = soundManager.GetComponent<SoundManager>();
        isClick = false;

        nm = noticeManager.GetComponent<NoticeManager>();
    }

    void Update()
    {
        HidePanel();
    }

    public void OnButtonClick()
    {
        sm.PlayClip("Button");
        Image sr = GetComponent<Image>();
        if (!isClick)
        {
            if (!gameManager.isLive)
            {
                bp.anim.SetTrigger("doHide");
                Image aSr = bp.GetComponent<Image>();
                aSr.sprite = bp.hideSprite;
                bp.isClick = false;
            }
                
            anim.SetTrigger("doShow");
            sr.sprite = showSprite;
            isClick = true;
            btnOp.isClick = true;
            gameManager.isLive = false;
        }
        else if(isClick)
        {
            anim.SetTrigger("doHide");
            sr.sprite = hideSprite;
            isClick = false;
            btnOp.isClick = false;
            gameManager.isLive = true;
        }
    }

    void HidePanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isClick)
        {
            sm.PlayClip("Button");
            anim.SetTrigger("doHide");
            Image sr = GetComponent<Image>();
            sr.sprite = hideSprite;
            isClick = false;
            Invoke("ChangeFalse", 0.5f);
            gameManager.isLive = true;
        }
    }

    void ChangeFalse()
    {
        btnOp.isClick = false;
    }

    public void SellButtonClicked()
    {
        sm.PlayClip("Button");
        nm.MakeNotice("Sell");
    }
}
