using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowControl : MonoBehaviour
{
    Jelly parentJelly;

    private void Awake()
    {
        parentJelly = GetComponentInParent<Jelly>();    
    }

    private void Start()
    {
        MakePos();
    }

    void MakePos()
    {
        float ypos;
        switch (parentJelly.id)
        {
            case 0:
                ypos = -0.05f;
                break;
            case 6:
                ypos = -0.12f;
                break;
            case 3:
                ypos = -0.14f;
                break;
            case 10:
                ypos = -0.16f;
                break;
            case 11:
                ypos = -0.16f;
                break;
            default:
                ypos = -0.1f;
                break;
        }

        transform.localPosition = new Vector3(0, ypos, 0);
    }
}
