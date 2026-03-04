using System;
using UnityEngine;

public class AnimTranistions : MonoBehaviour
{

    Animator anim;
    public Boolean stuck;
    public CanGrab canGrab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        /*if(Input.GetKeyDown(KeyCode.Space) && (stuck == false))
        {
           stuck = true;
        }

        else if (Input.GetKeyDown(KeyCode.Space) && (stuck == true))
        {
            stuck = false;
        }*/



        if (Input.GetMouseButtonDown(0) && (stuck == false) && canGrab.fuel > 0f)
        {
            anim.SetTrigger("Clicked");
        }
        
        else
        {

            anim.ResetTrigger("Clicked");
        }

        if (stuck == true)
        {
            anim.speed = 0;
        }

        else if (stuck == false)
        {
            anim.speed = 1;
        }
    }
}
