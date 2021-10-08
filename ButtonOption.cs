using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOption : MonoBehaviour
{
    public bool isClick;
    public GameObject panel;
    public GameObject manager;
    GameManager gameManager;

    // Sound System
    public GameObject soundManager;
    SoundManager sm;

    void Awake()
    {
        gameManager = manager.GetComponent<GameManager>();
        sm = soundManager.GetComponent<SoundManager>();
    }
    void Update()
    {
        ControlOptionPanel();
    }

    void ControlOptionPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameManager.isLive && !isClick)
        {
            sm.PlayClip("Pause In");
            panel.SetActive(true);
            //isClick = true;
            gameManager.isLive = false;
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !gameManager.isLive && !isClick)
        {
            sm.PlayClip("Pause Out");
            panel.SetActive(false);
            //isClick = false;
            gameManager.isLive = true;
            Time.timeScale = 1;
        }
    }

    public void ResumeButton()
    {
        sm.PlayClip("Pause Out");
        panel.SetActive(false);
        //isClick = false;
        gameManager.isLive = true;
        Time.timeScale = 1;
    }
}
