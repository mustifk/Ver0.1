﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class haveYou : MonoBehaviour
{
    public int Demo = 0;
    public Animator anim;
    //timebar
    public GameObject TBC;
    timebarScript timebar;
    public AudioSource correct, incorrect, show;
    [SerializeField] private Sprite[] images;
    private int difficulty;
    [SerializeField] private images originalCard;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private Button button2;
    images card;

    bool isTrue = false, isAgain = true;
    private int cardNumbers;
    void Awake()
    {
        images = Resources.LoadAll<Sprite>("Sprites/haveYou");
        images = shuffleImages(images);
    }

    void Start()
    {

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<mainScript>().Language() == 0)
            {
                text.text = "Bunu daha once gormus muydun?";
            }
        }
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
        
        setActives(false);
        originalCard.transform.localScale = new Vector2(1f, 1f);
        originalCard.transform.position = new Vector2(0f, 0f);
        switch (difficulty)
        {
            case 1:
                cardNumbers = 4;
                timebar.SetMax(3);
                break;
            case 2:
                cardNumbers = 5;
                timebar.SetMax(3);
                break;
            case 3:
                cardNumbers = 6;
                timebar.SetMax(3);
                break;
            default:
                break;
        }
        StartCoroutine(listCards());
    }
    private void Update()
    {
        if (timebar.GetTime() == 0)
        {
            StartCoroutine(EndGame(false));
        }
    }

    private IEnumerator listCards()
    {

        for (int i = 0; i < cardNumbers; i++)
        {
            StartCoroutine(showCards(i));
            yield return new WaitForSeconds(1f);

        }

        isAgain = false;

        if (Random.Range(0, 2) == 1)
        {
            True();
        }
        else
        {
            False();
        }
        timebar.Begin();
        yield return new WaitForSeconds(1f);

    }

    private IEnumerator showCards(int i)
    {
        show.Play();
        yield return new WaitForSeconds(0f);

        card = Instantiate(originalCard) as images;
        card.transform.parent = this.gameObject.transform;
        card.ChangeSprite(i, images[i]);
        card.isDestroy(isAgain);
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
    private void True()
    {
        isTrue = true;
        setActives(true);
        originalCard.transform.localScale = new Vector2(0.6f, 0.6f);
        int index = Random.Range(0, difficulty);
        StartCoroutine(showCards(index));
    }
    private void False()
    {
        isTrue = false;
        setActives(true);
        originalCard.transform.localScale = new Vector2(0.6f, 0.6f);
        int index = Random.Range(difficulty, images.Length);
        StartCoroutine(showCards(index));
    }
    public void Press(bool trueorfalse)
    {
        if (isTrue && trueorfalse || !isTrue && !trueorfalse)
        {
            correct.Play();
            StartCoroutine(EndGame(true));
        }
        else
        {
            incorrect.Play();
            StartCoroutine(EndGame(false));
        }
        timebar.Stop();

    }
    private void setActives(bool trueorfalse)
    {
        button.gameObject.SetActive(trueorfalse);
        button2.gameObject.SetActive(trueorfalse);
        text.enabled = trueorfalse;
    }

    IEnumerator EndGame(bool win)
    {
        if (win == true)
        {
            anim.SetBool("true1", true);
        }
        else
        {
            anim.SetBool("false1", true);
        }
        yield return new WaitForSeconds(0.8f);
        if (Demo == 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<mainScript>().EndOfMinigame((timebar.GetTime() / timebar.GetMax()), win);
        }
    }
}
