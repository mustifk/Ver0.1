﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDBallsEngine : MonoBehaviour
{
    //timebar
    public GameObject TBC;
    timebarScript timebar;

    public GameObject droplet;
    int difficulty = 3;
    int dropletCount,dCtemp;
    bool isGameover;
    
    // Start is called before the first frame update
    void Start()
    {
        //timebar
        GameObject temp = Instantiate(TBC);
        timebar = temp.GetComponent<TBCscript>().timebar();


        difficulty = GameObject.FindGameObjectWithTag("Player").GetComponent<mainScript>().Difficulty();
        isGameover = false;
        switch (difficulty)
        {
            case 2:
                timebar.SetMax(10.5f);
                dropletCount = 8;
                break;
            case 3:
                timebar.SetMax(11);
                dropletCount = 12;
                break;
            default:
                timebar.SetMax(7.5f);
                dropletCount = 4;
                break;
        }
        dCtemp = 2 * dropletCount;
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        timebar.Begin();
        GameObject temp;
        while (dropletCount != 0)
        {
            for (int i = 0; i < 2; i++)
            {
                temp = Instantiate(droplet, new Vector3(0, 0), Quaternion.identity, transform);
                if (i == 1)
                {
                    temp.GetComponent<dropletScript>().Blue();
                }
                temp.GetComponent<dropletScript>().Position();
            }
            dropletCount--;
            yield return new WaitForSeconds(2f - difficulty * 0.4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDifficulty()
    {
        return difficulty;
    }

    public void GameOver()
    {
        if (!isGameover)
        {
            StopAllCoroutines();
            StartCoroutine(End(false));
        }
    }

    IEnumerator End(bool win)
    {
        isGameover = true;
        timebar.Stop();
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("Player").GetComponent<mainScript>().EndOfMinigame(10, win);
    }
    public void DropletGone()
    {
        dCtemp--;
        if (dCtemp == 0)
        {
            StopAllCoroutines();
            StartCoroutine(End(true));
        }
    }
}
