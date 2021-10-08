using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    public int id;
    public int level;
    public float exp = 0;

    bool isIdle = true;
    bool isMoveStart = false;

    float timer = 0;
    float randrange;
    float xspeed;
    float yspeed;

    Animator anim;

    public SpriteRenderer spriteRenderer;

    GameObject borderTopLeft;
    GameObject borderBottomRight;

    public GameObject manager;
    GameManager gameManager;

    public RuntimeAnimatorController[] LevelAc;
    float curPickTime = 0;
    float maxPickTime = 0.5f;

    //Sound System
    public SoundManager sm;

    private void Awake()
    {
        randrange = Random.Range(2f, 5f);
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Init2();
    }

    void Init2()
    {
        gameManager = manager.GetComponent<GameManager>();
        borderTopLeft = gameManager.borderTopLeft;
        borderBottomRight = gameManager.borderBottomRight;
    }

    private void Update()
    {
        if (isIdle)
        {
            Idle();
        }
        else
        {
            Move();
        }
        SetExp();
        ChangeTimer();
    }

    void Idle()
    {
        anim.SetBool("isWalk", false);
        if (randrange > timer)
            return;

        Debug.Log("idle");

        Init();
        isMoveStart = true;
    }

    void Move()
    {
        if (isMoveStart)
        {
            MakeDirection();
            Debug.Log("move");
        }

        Vector3 curPos = transform.position;

        float xpos = curPos.x + xspeed * Time.deltaTime;
        float ypos = curPos.y + yspeed * Time.deltaTime;

        if (xspeed < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (xpos > borderBottomRight.transform.position.x || xpos < borderTopLeft.transform.position.x)
            xspeed = -xspeed;

        if (ypos > borderTopLeft.transform.position.y || ypos < borderBottomRight.transform.position.y)
            yspeed = -yspeed;

        transform.position = new Vector3(xpos, ypos, ypos);

        anim.SetBool("isWalk", true);

        if (randrange < timer)
        {
            Init();
        }
    }

    void ChangeTimer()
    {
        timer += Time.deltaTime;
    }

    void Init()
    {
        randrange = Random.Range(2f, 5f);
        isIdle = !isIdle;
        timer = 0;
    }

    void MakeDirection()
    {
        xspeed = Random.Range(-0.7f, 0.7f);
        yspeed = Random.Range(-0.7f, 0.7f);
        isMoveStart = false;
    }

    void OnMouseDown()
    {
        if (!gameManager.isLive)
            return;

        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        gameManager.GetJelatin(id, level);

        exp += 1;

        randrange = Random.Range(2f, 5f);
        isIdle = true;
        timer = 0;
    }

    void ChangeAc()
    {
        Debug.Log("ch");
        anim.runtimeAnimatorController = LevelAc[level - 1];
    }

    void SetExp()
    {
        if (level == 3)
            return;

        exp += Time.deltaTime;

        float requireExp = level * 10;
        if (exp >= requireExp)
        {
            level += 1;
            ChangeAc();
            sm.PlayClip("Grow");
        }
    }

    void Pick()
    {
        transform.position = gameManager.GetWorldMousePosition();
    }

    void OnMouseDrag()
    {
        if (!gameManager.isLive)
            return;

        curPickTime += Time.deltaTime;

        if (maxPickTime > curPickTime)
            return;

        Pick();

    }

    // Sell
    void OnMouseUp()
    {
        curPickTime = 0;

        float xpos = transform.position.x;
        float ypos = transform.position.y;
        if (xpos > borderBottomRight.transform.position.x || xpos < borderTopLeft.transform.position.x)
            transform.position = new Vector3(0, 0, 0);

        if (ypos > borderTopLeft.transform.position.y || ypos < borderBottomRight.transform.position.y)
            transform.position = new Vector3(0, 0, 0);

        if (gameManager.isSell)
        {
            gameManager.GetGold(id, level);
            Debug.Log("sell: " + gameObject);
            gameManager.jellyList.Remove(gameObject);

            Destroy(gameObject);
            sm.PlayClip("Sell");
            return;
        }
    }
}
