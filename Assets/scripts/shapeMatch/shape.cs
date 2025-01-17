﻿using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class shape : MonoBehaviour
{

    private shapeController controller;

    public string shapeName;

    public bool selectable = true;

    private Sprite[] _circleSprite;
    private Sprite[] _hexagonSprite;
    private Sprite[] _rectangleSprite;
    private Sprite[] _starSprite;
    private Sprite[] _triangleSprite;

    internal Sprite[] circleSprite()
    {
        return Resources.LoadAll<Sprite>("Sprites/shapeMatch/circles"); ;
    }

    internal Sprite[] hexagonSprite()
    {
       return Resources.LoadAll<Sprite>("Sprites/shapeMatch/hexagons"); ; 
    }

    internal Sprite[] rectangleSprite()
    {
       return Resources.LoadAll<Sprite>("Sprites/shapeMatch/rectangles"); ;
    }

    internal Sprite[] starSprite()
    {
       return Resources.LoadAll<Sprite>("Sprites/shapeMatch/stars"); ;
    }

    internal Sprite[] triangleSprite()
    {
       return Resources.LoadAll<Sprite>("Sprites/shapeMatch/triangles"); ;
    }

    private void Awake()
    {
        _circleSprite = Resources.LoadAll<Sprite>("Sprites/shapeMatch/circles");
        _hexagonSprite = Resources.LoadAll<Sprite>("Sprites/shapeMatch/hexagons");
        _rectangleSprite = Resources.LoadAll<Sprite>("Sprites/shapeMatch/rectangles");
        _starSprite = Resources.LoadAll<Sprite>("Sprites/shapeMatch/stars");
        _triangleSprite = Resources.LoadAll<Sprite>("Sprites/shapeMatch/triangles");
    }

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<shapeController>();
    }

   
    private void OnMouseDown()
    {
        if (selectable)
        {
         

            if (controller.message != shapeName)     //user selected wrong shape according to message
            {
                controller.playWrong();
                controller.checkShape = false;
                controller.GameOver(false);
            }
            else
            {
                controller.playBubble();
                controller.counterShape++;      //user selected right shape and counter is incremented
                selectable = false;
                StartCoroutine(destroy());
            }
        }
    }
    IEnumerator destroy()
    {
        
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
   
    public int changeSprite(int index)
    {
        switch (shapeName)
        {
            case "daire":
                shapeName = "circle";
                index = (++index) % _circleSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _circleSprite[index];
                break;
            case "circle":
                index = (++index) % _circleSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _circleSprite[index];
                break;
            case "altigen":
                shapeName = "hexagon";
                index = (++index) % _hexagonSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _hexagonSprite[index];
                break;
            case "hexagon":
                index = (++index) % _hexagonSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _hexagonSprite[index];
                break;
            case "dortgen":
                shapeName = "rectangle";
                index = (++index) % _rectangleSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _rectangleSprite[index];
                break;
            case "rectangle":
                index = (++index) % _rectangleSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _rectangleSprite[index];
                break;
            case "yildiz":
                shapeName = "star";
                index = (++index) % _starSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _starSprite[index];
                break;
            case "star":
                index = (++index) % _starSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _starSprite[index];
                break;
            case "ucgen":
                shapeName = "triangle";
                index = (++index) % _triangleSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _triangleSprite[index];
                break;
            case "triangle":
                index = (++index) % _triangleSprite.Length;
                GetComponent<SpriteRenderer>().sprite = _triangleSprite[index];
                break;
        }
        return index;
    }

    private Sprite[] shuffleImages(Sprite[] images)
    {
        Sprite[] temp = images.Clone() as Sprite[];
        for (int i = 0; i < temp.Length; i++)
        {
            Sprite tmp = temp[i];
            int random = Random.Range(i, temp.Length);
            temp[i] = temp[random];
            temp[random] = tmp;
        }
        return temp;
    }

}
