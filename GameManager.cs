using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int MAXVALUE = 999999999;
    public Text jelatinText;
    public Text goldText;

    public int jelatin = 1000;
    public int gold = 1000;
    int numLevel = 1;
    int clickLevel = 1;

    //float curDelayTime = 0f;
    //float maxDelayTime = 1f;

    public Camera cam;

    public int[] jellyGoldList;
    public int[] jellyJelatinList;
    public Sprite[] jellySpriteList;
    public string[] jellyNameList;
    public int[] numGoldList;
    public int[] clickGoldList;

    bool[] jellyUnlockList;

    public bool isSell;

    public bool isLive;

    int page = 0;
    public Image jellyInfoImage;
    public Text jellyNameText;
    public Text jellyGoldText;
    public Text pageText;

    public Image jellyInfoLockImage;
    public Text jellyJelatinText;

    public GameObject lockGroup;

    public GameObject jellyPrefab;

    public GameObject borderTopLeft;
    public GameObject borderBottomRight;

    public List<GameObject> jellyList;

    // Plant Panel
    public Text jellyAllowAmountText;
    public Text jellyApartGoldText;
    public Text jellyClickAmountText;
    public Text jellyClickGoldText;
    public GameObject numGroupBtn;
    public GameObject clickGroupBtn;

    // Sound System
    public GameObject soundManager;
    SoundManager sm;

    // Notice Manager
    public GameObject noticeManager;
    NoticeManager nm;

    // Clear
    bool isClear = false;
    public GameObject clearMedal;

    void Awake()
    {
        jelatinText.text = string.Format("{0:n0}", jelatin);
        goldText.text = string.Format("{0:n0}", gold);

        jellyUnlockList = new bool[jellyNameList.Length];
        jellyUnlockList[0] = true;
        for (int i = 1; i < jellyUnlockList.Length; i++)
            jellyUnlockList[i] = false;

        jellyList = new List<GameObject>();
        jellyList.Clear();

        sm = soundManager.GetComponent<SoundManager>();
        nm = noticeManager.GetComponent<NoticeManager>();
    }

    void Start()
    {
        jellyApartGoldText.text = numGoldList[numLevel].ToString();
        jellyClickGoldText.text = clickGoldList[clickLevel].ToString();

        ChangeInfo();
        if (PlayerPrefs.HasKey("Jelatin"))
            jelatin = PlayerPrefs.GetInt("Jelatin");

        if (PlayerPrefs.HasKey("Gold"))
            gold = PlayerPrefs.GetInt("Gold");

        StartCoroutine("Auto");

        if (PlayerPrefs.GetInt("Clear") == 0)
            isClear = false;
        else
            isClear = true;
        Clear();
    }

    void Clear()
    {
        if (isClear)
        {
            clearMedal.SetActive(true);
            sm.PlayClip("Clear");
            nm.MakeNotice("Clear");
        }
        else
        {
            nm.MakeNotice("Start");
        }
    }

    void OnEnable()
    {
        
    }

    void Update()
    {
        MakeDelay();
    }

    void LateUpdate()
    {
        jelatinText.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelatinText.text), jelatin, 0.5f));
        goldText.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(goldText.text), gold, 0.5f));
    }

    void MakeDelay()
    {
        //curDelayTime += Time.deltaTime;
    }

    public void GetJelatin(int id, int level) 
    {
        jelatin += ((id + 1) * level) * (int)Mathf.Pow(2, clickLevel - 1);
        jelatin = Mathf.Min(MAXVALUE, jelatin);
        sm.PlayClip("Touch");
    }

    // 월드 좌표계 마우스 위치
    public Vector3 GetWorldMousePosition()
    {
        Vector3 tmp = cam.ScreenToWorldPoint(Input.mousePosition);
        tmp.z = tmp.y;
        return tmp;
    }

    public void EnterBtn()
    {
        isSell = true;
    }

    public void ExitBtn()
    {
        isSell = false;
    }

    public void GetGold(int id, int level)
    {
        gold += jellyGoldList[id] * level;
        gold = Mathf.Min(MAXVALUE, gold);
    }

    public void PageDown()
    {
        if (page == 0)
        {
            sm.PlayClip("Fail");
            return;
        }

        page -= 1;
        ChangeInfo();
        sm.PlayClip("Button");
    }

    public void PageUp()
    {
        if (page == 11)
        {
            sm.PlayClip("Fail");
            return;
        }

        page += 1;
        ChangeInfo();
        sm.PlayClip("Button");
    }

    void ChangeInfo()
    {
        if (!jellyUnlockList[page])
        {
            jellyInfoLockImage.sprite = jellySpriteList[page];
            jellyInfoLockImage.color = new Color(0, 0, 0);
            jellyJelatinText.text = string.Format("{0:n0}", jellyJelatinList[page]);
            lockGroup.SetActive(true);
        }
        else
        {
            lockGroup.SetActive(false);
        }

        jellyInfoImage.sprite = jellySpriteList[page];
        jellyInfoImage.SetNativeSize();
        jellyNameText.text = jellyNameList[page];
        jellyGoldText.text = string.Format("{0:n0}", jellyGoldList[page]);
        pageText.text = string.Format("#{0:00}", page + 1);
    }

    public void Unlock()
    {
        if (jelatin < jellyJelatinList[page])
        {
            sm.PlayClip("Fail");
            nm.MakeNotice("NotJelatin");
            return;
        }
            

        jelatin -= jellyJelatinList[page];
        jellyUnlockList[page] = true;
        ChangeInfo();
        sm.PlayClip("Unlock");

        for(int i = 0; i < jellyUnlockList.Length; i++)
        {
            if (!jellyUnlockList[i])
            {
                return;
            }
        }
        isClear = true;
        Clear();
    }

    public void Buy()
    {
        if (gold < jellyGoldList[page])
        {
            sm.PlayClip("Fail");
            nm.MakeNotice("NotGold");
            return;
        }

        if(numLevel * 2 <= jellyList.Count)
        {
            sm.PlayClip("Fail");
            nm.MakeNotice("NotNum");
            return;
        }

        gold -= jellyGoldList[page];        

        MakeJelly();
        sm.PlayClip("Buy");
    }

    void MakeJelly()
    {
        Jelly tmp = jellyPrefab.GetComponent<Jelly>();
        tmp.manager = gameObject;
        tmp.sm = sm;

        tmp.id = page;
        tmp.level = 1;
        tmp.exp = 0;

        tmp.spriteRenderer = jellyPrefab.GetComponent<SpriteRenderer>();
        tmp.spriteRenderer.sprite = jellySpriteList[page];

        tmp.name = string.Format("{0}", string.Concat("Jelly", page.ToString()));

        GameObject ins = Instantiate(jellyPrefab, transform, transform);
        jellyList.Add(ins);
    }

    IEnumerator Auto()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            foreach(var jel in jellyList)
            {
                Jelly tmp = jel.GetComponent<Jelly>();
                //GetJelatin(tmp.id, tmp.level);
                jelatin += ((tmp.id + 1) * tmp.level) * (int)Mathf.Pow(2, clickLevel - 1);
                jelatin = Mathf.Min(MAXVALUE, jelatin);
            }
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("End");
        PlayerPrefs.SetInt("Jelatin", jelatin);
        PlayerPrefs.SetInt("Gold", gold);
        if (isClear)
            PlayerPrefs.SetInt("Clear", 1);
        else
            PlayerPrefs.SetInt("Clear", 0);
    }

    public void BuyApart()
    {
        if (gold < numGoldList[numLevel])
        {
            sm.PlayClip("Fail");
            nm.MakeNotice("NotGold");
            return;
        }
            

        gold -= numGoldList[numLevel];
        numLevel++;
        sm.PlayClip("Clear");

        jellyAllowAmountText.text = string.Format("{0} {1}", "젤리 수용량", (numLevel * 2).ToString());
        jellyApartGoldText.text = string.Format("{0}", numGoldList[numLevel]);

        if (numLevel == 5)
        {
            numGroupBtn.SetActive(false);
            return;
        }
    }

    public void BuyClick()
    {
        if (gold < clickGoldList[clickLevel])
        {
            sm.PlayClip("Fail");
            nm.MakeNotice("NotGold");
            return;
        }

        gold -= clickGoldList[clickLevel];
        clickLevel++;
        sm.PlayClip("Clear");

        jellyClickAmountText.text = string.Format("{0} {1}", "클릭 생산량 x", (Mathf.Pow(2, clickLevel-1)).ToString());
        jellyClickGoldText.text = string.Format("{0}", clickGoldList[clickLevel]);

        if (clickLevel == 5)
        {
            clickGroupBtn.SetActive(false);
            return;
        }
    }

    public void QuitGame()
    {
        sm.PlayClip("Pause Out");

        StartCoroutine("EndGame");
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);

        Application.Quit();
    }
}
