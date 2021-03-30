﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class shapeController : MonoBehaviour
{
    //Easy -> 1
    //Normal -> 2
    //Hard -> 3
    public int diffLevel = 1;

    public shape circle;
    public shape hexagon;
    public shape rectangle;
    public shape star;
    public shape triangle;

    public Text text;

    public string message;
    private shape[] otherShapes;        //otherShapes is an array that does not contain the shape in the message

    private Sprite[] otherSprites;
    int[] otherSpriteArray;         //for different shape
    int[] sameSpriteArray;          //array filled depends on message

    public bool checkShape = true;
    public int counterShape = 0;

    private int numberOfObjects;
    private int beforeTextIndex = -1;
    int lastSprite = 0;

    private string[] texts = { "circle", "hexagon", "rectangle", "star", "triangle" };

    void Start()
    {
        switch (diffLevel)
        {
            case 1:
                numberOfObjects = 4;
                break;
            case 2:
                numberOfObjects = 6;
                break;
            case 3:
                numberOfObjects = 8;
                break;
        }

        otherSpriteArray = new int[numberOfObjects];
        sameSpriteArray = new int[diffLevel];

        prepareGame();
    }

    private void Update()
    {
        if (counterShape == diffLevel)       //load other shapes
        {
            int childs = transform.childCount;
            for (int i = childs - 1; i > 0; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }

            GameObject.Destroy(transform.GetChild(0).gameObject);

            counterShape = 0;
            lastSprite = 0;
            prepareGame();
        }
    }

    void prepareGame()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            otherSpriteArray[i] = 0;
        }

        for (int i = 0; i < diffLevel; i++)
        {
            sameSpriteArray[i] = 0;
        }

        int index = Random.Range(0, texts.Length);

        while(beforeTextIndex == index)     //prevent 2 same messages in successive sections
            index = Random.Range(0, texts.Length);
        beforeTextIndex = index;

        text.text = texts[index];

        message = texts[index];

        switch (message)
        {
            case "circle":
                otherShapes = new shape[] { hexagon, rectangle, star, triangle };
                break;
            case "hexagon":
                otherShapes = new shape[] { circle, rectangle, star, triangle };
                break;
            case "rectangle":
                otherShapes = new shape[] { circle, hexagon, star, triangle };
                break;
            case "star":
                otherShapes = new shape[] { circle, hexagon, rectangle, triangle };
                break;
            case "triangle":
                otherShapes = new shape[] { circle, hexagon, rectangle, star };
                break;
        }

        Sprite[] sprites = new Sprite[numberOfObjects];

        createShapes(sprites);
    }

    private void createShapes(Sprite[] sprites)
    {
        float distanceX = 0, distanceY = 0;
        float startPosX = -6f;
        float startPosY = -4f;
        switch (diffLevel)
        {
            case 1:
                distanceX = Random.Range(3f, 4f);
                distanceY = Random.Range(1f, 1.5f);
                break;
            case 2:
                distanceX = Random.Range(2f, 2.5f);
                distanceY = Random.Range(0.5f, 1f);
                break;
            case 3:
                startPosX = -8f;
                distanceX = Random.Range(2f,2.3f);
                distanceY = Random.Range(0.5f, 0.9f);
                break;
        }

        float posx = startPosX;
        float posy = startPosY;


        ArrayList Xpos = new ArrayList(sprites.Length);
        ArrayList Ypos = new ArrayList(sprites.Length);

        for (int i = 0;i<sprites.Length;i++)
        {
            Xpos.Add(posx);
            Ypos.Add(posy);
            posx += distanceX;
            posy += distanceY;
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            shape shape;

            if (i < diffLevel)
            {
                switch (message)
                {
                    case "circle":
                        shape = circle;
                        break;
                    case "hexagon":
                        shape = hexagon;
                        break;
                    case "rectangle":
                        shape = rectangle;
                        break;
                    case "star":
                        shape = star;
                        break;
                    default:
                        shape = triangle;
                        break;
                }
                shape = Instantiate(shape) as shape;
                lastSprite = shape.changeSprite(lastSprite);
            }
            else
            {
                int index = Random.Range(0, otherShapes.Length);
                shape = Instantiate(otherShapes[index]) as shape;
                
                switch (shape.shapeName)
                {
                    case "circle":
                        otherSprites = shape.circleSprite();
                        break;
                    case "hexagon":
                        otherSprites = shape.hexagonSprite();
                        break;
                    case "rectangle":
                        otherSprites = shape.rectangleSprite();
                        break;
                    case "star":
                        otherSprites = shape.starSprite();
                        break;
                    default:
                        otherSprites = shape.triangleSprite();
                        break;
                }

                while(otherSpriteArray[index] == 1)
                {
                    if(otherSpriteArray.Length > otherSprites.Length)
                        index = (index + 1) % otherSprites.Length;
                    else
                        index = (index + 1) % otherSpriteArray.Length;

                }

                shape.GetComponent<SpriteRenderer>().sprite = otherSprites[index];
                otherSpriteArray[index] = 1;
            }

            shape.transform.parent = gameObject.transform;

            int indexOfPos = Random.Range(0,Xpos.Count);
            posx = (float) Xpos[indexOfPos];
            Xpos.RemoveAt(indexOfPos);

            indexOfPos = Random.Range(0, Ypos.Count);
            posy = (float) Ypos[indexOfPos];
            Ypos.RemoveAt(indexOfPos);
            shape.transform.position = new Vector3(posx, posy, circle.transform.position.z);

            
        }
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        int childs = transform.childCount;
        for (int i = childs - 1; i > 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        GameObject.Destroy(transform.GetChild(0).gameObject);
        GameObject.Destroy(text);
        //SceneManager.LoadScene("cardMatch");
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