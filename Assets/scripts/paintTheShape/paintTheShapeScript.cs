﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class paintTheShapeScript : MonoBehaviour
{
    public Animator anim;
    public AudioSource tap, incorrect,correct;
    public int Demo = 0;
    //timebar
    public GameObject TBC;
    timebarScript timebar;

    bool isGameover = false;
    public GameObject block, blockM,BlocksRight,BlocksLeft;
    GameObject currentBlock,tempGO;
    GameObject[,] blockList;
    public int[,] blocks1, blocks2, validBlocks;
    int difficulty;
    int gap = 1,arrLength;
    float positioner,scaler;
    // Start is called before the first frame update
    void Start()
    {
        //timebar
        GameObject temp = Instantiate(TBC);
        timebar = temp.GetComponent<TBCscript>().timebar();
        if (Demo == 0)
        {
            difficulty = GameObject.FindGameObjectWithTag("Player").GetComponent<mainScript>().Difficulty();
        }
        else
        {
            difficulty = Demo;
        }
        blockList = new GameObject[5,5];
        blocks1 = new int[5,5];
        blocks2 = new int[5,5];
        validBlocks = new int[5,5];
       
        switch (difficulty) 
        {
            
            case 2:
                currentBlock = blockM;
                arrLength = 4;
                positioner = 1.5f;
                scaler = 0.5f;
                timebar.SetMax(9);
                break;
            case 3:
                currentBlock = blockM;
                arrLength = 5;
                positioner = 2f;
                scaler = 0.5f;
                timebar.SetMax(13);
                break;
            default:
                gap = 2;
                currentBlock = block;
                arrLength = 3;
                positioner = 2f;
                scaler = 0.6f;
                timebar.SetMax(5);
                break;
        }

        do
        {
            Randomizer(blocks1);
            Randomizer(blocks2);
        } 
        while (!Validator());
        
        for (int x = 0; x < arrLength; x++)
        {
            for (int y = 0; y < arrLength; y++)
            {
                blockList[x, y] = Instantiate(currentBlock, new Vector3((x * gap) - positioner,( y * gap) - positioner, 1), Quaternion.identity, this.transform);
                tempGO = Instantiate(currentBlock, new Vector3(x * gap, y * gap, 1), Quaternion.identity, BlocksLeft.transform) as GameObject;
                tempGO.GetComponent<blockScript>().ChangeState(blocks1[x, y]);
                tempGO = Instantiate(currentBlock, new Vector3(x * gap, y * gap, 1), Quaternion.identity, BlocksRight.transform) as GameObject;
                tempGO.GetComponent<blockScript>().ChangeState(blocks2[x, y]);
            }
        }
        BlocksRight.transform.localScale = new Vector3(scaler, scaler, 1);
        BlocksLeft.transform.localScale = new Vector3(scaler, scaler, 1);
        if (difficulty == 2)
        {
            foreach (var cam in Camera.allCameras)
            {
                cam.orthographicSize = 3;
            }
            BlocksRight.transform.position = new Vector3(2.5f, -0.75f, 3);
            BlocksLeft.transform.position = new Vector3(-4f, -0.75f, 3);
            this.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        }
        else if(difficulty == 1)
        {
            BlocksRight.transform.position = new Vector3(4.75f, -1.175f, 3);
            BlocksLeft.transform.position = new Vector3(-7.25f, -1.175f, 3);
        }
        else
        {
            foreach (var cam in Camera.allCameras)
            {
                cam.orthographicSize = 3.5f;
            }
            BlocksRight.transform.position = new Vector3(3.5f, -1f, 3);
            BlocksLeft.transform.position = new Vector3(-5.5f, -1f, 3);
        }
        timebar.Begin();
    }

    void Update()
    {
        if (timebar.GetTime() == 0 && !isGameover)
        {
            StartCoroutine(GameOver(false));
        }
    }


    public void Press()
    {
        int complete = 0;
        for (int x = 0; x < arrLength; x++)
        {
            for (int y = 0; y < arrLength; y++)
            {
                if (blockList[x,y].GetComponent<blockScript>().CurrentState() < validBlocks[x,y])
                {
                    incorrect.Play();
                    StartCoroutine(GameOver(false));
                }
                else if (blockList[x,y].GetComponent<blockScript>().CurrentState() == validBlocks[x,y])
                {
                    tap.Play();
                    complete++;
                }
            }
        }
        if (complete == arrLength * arrLength)
        {
            StartCoroutine(GameOver(true));
        }
    }

    bool Validator()
    {
        int count = arrLength * arrLength;
        for (int x = 0; x < arrLength; x++)
        {
            for (int y = 0; y < arrLength; y++)
            {
                if (blocks1[x,y] >= blocks2[x,y])
                {
                    validBlocks[x, y] = blocks1[x, y];
                }
                else
                {
                    validBlocks[x, y] = blocks2[x, y];
                }
                if (validBlocks[x, y] == 1)
                {
                    count--;
                }
            }
        }
        return !(count == 0);
    }

    void Randomizer(int[,] blocks)
    {
        int temp = Random.Range(0, 3);
        int[] states;
        states = new int[3];
        switch (difficulty)
        {
            case 2:
                states[0] = 9;
                states[1] = 7;
                //states[2] = 6;
                break;
            case 3:
                states[0] = 13;
                states[1] = 12;
                //states[2] = 9;
                break;
            default:
                states[0] = 5;
                states[1] = 4;
                //states[2] = 3;
                break;
        }
        for (int x = 0; x < arrLength; x++)
        {
            for (int y = 0; y < arrLength; y++)
            {
                temp = Random.Range(0, 2);
                while (states[temp] == 0)
                {
                    temp = Random.Range(0, 2);
                }
                states[temp]--;
                blocks[x, y] = temp;
            }
        }
    }

    void Terminate()
    {
        for (int i = 0; i < arrLength; i++)
        {
            for (int k = 0; k < arrLength; k++)
            {
                blockList[i, k].GetComponent<blockScript>().ColliderOff();
            }
        }
    }


    IEnumerator GameOver(bool win)
    {
        isGameover = true;
        Terminate();
        timebar.Stop();
        if (win == true)
        {
            correct.Play();
            anim.SetBool("true1", true);
        }
        else
        {
            anim.SetBool("false1", true);
        }
        yield return new WaitForSeconds(0.8f);
        foreach (var cam in Camera.allCameras)
        {
            cam.orthographicSize = 5;
        }
        if (Demo == 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<mainScript>().EndOfMinigame((timebar.GetTime() / timebar.GetMax()), win);
        }
    }
}
