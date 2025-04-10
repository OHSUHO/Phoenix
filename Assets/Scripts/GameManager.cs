using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Fail;
    public GameObject Victory;
    public Text inGameTxt;

    public Card firstCard;
    public Card secondCard;

    [HideInInspector]
    public float totaltime;

    public int cardNum;
    
    [SerializeField] private AudioClip match; //ī�� ��ġ ����
    [SerializeField] private AudioClip missMatch; //ī�� �̾���ġ ����
    AudioSource ads;
    string card1Str = "��ź1 ī�� ���ī�����!";
    string card2Str = "��ź2 ī�� �ð�10���߰�!";
    float time = 0f;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Board.board);
        ads = GetComponent<AudioSource>();
        if (TimerBar.Instance != null)
            totaltime = TimerBar.Instance.GettingTotalTime();
        Board.board.BoardSetting(()=>StartFlip(DifficultSetting.Instance.Getting()));

    }

    // Update is called once per frame
    void Update()
    {   if(TimerBar.Instance != null) { 
        time = TimerBar.Instance.elapsedTime;
       
        if (time >= totaltime)
        {
            time = totaltime;
            Fail.SetActive(true);
            Time.timeScale = 0f;
        }
        }
    }

    public void StartFlip(DifficultSetting.Difficulty difficulty)
    {
        FlipAll();
        
        switch (difficulty)
        {
            case DifficultSetting.Difficulty.Easy:
                InvokeBackFlipAll(5f);
                TimerBar.Instance.TimeFreezing(6f);
                break;
            case DifficultSetting.Difficulty.Normal:
                InvokeBackFlipAll(3.5f);
                TimerBar.Instance.TimeFreezing(4.5f);
                break;
            case DifficultSetting.Difficulty.Hard:
                InvokeBackFlipAll(2f);
                TimerBar.Instance.TimeFreezing(3f);
                break;
        }

        


    }


    public void Matched()
    {
        if (firstCard.idx == secondCard.idx && (firstCard!=secondCard))
        {
            if(firstCard.idx == 1)
            {
                FlipAll();
                InvokeBackFlipAll(2f);
                TimerBar.Instance.TimeFreezing(3f);
                StartCoroutine(EffectTxt(inGameTxt,10,card1Str));
                
            }
            if (firstCard.idx == 2)
            {
                TimerBar.Instance.AddTime(10f);
                totaltime += 10f;
                StartCoroutine(EffectTxt(inGameTxt, 10, card2Str));
            }

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardNum -= 2;
            ads.PlayOneShot(match);
            if(cardNum == 0)
            {
                TimerBar.Instance.PauseTimer();
                InvokeVictiory();
            }
        }
        else
        {
            CardMissMatch();
           
        }

        firstCard = null;
        secondCard = null;
    }



    IEnumerator EffectTxt(Text txt, int n,string str)
    {
        for (int i = 0; i < n; i++)
        {
            
            if(i %2 == 0)
            {
                txt.text = str;
            }
            else
            {
                txt.text = "";
                
            }
            yield return new WaitForSeconds(0.25f);
            
        }
    }

    

    void InvokeVictiory()
    {
        Invoke("Vict", 1f);
    }

    void Vict()
    {
        Victory.SetActive(true ) ;
        Time.timeScale = 0f;
    }

    public void FlipAll()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Card");
        
        for (int i = 0; i < go.Length; i++) 
        {
           
            Card cardScript = go[i].GetComponent<Card>();
            cardScript.SelectedCard ();
            
        }
        
    }

    void InvokeBackFlipAll(float time)
    {
        Invoke("BackFlipAll",time);
    }

    void BackFlipAll()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Card");
        for (int i = 0; i < go.Length; i++)
        {

            Card cardScript = go[i].GetComponent<Card>();
            cardScript.MissMatched();

        }

    }

  

    public void CardMissMatch()
    {
        firstCard.InvokeMissMatched();
        secondCard.InvokeMissMatched();
        ads.PlayOneShot(missMatch);
        TimerBar.Instance.elapsedTime += 1.5f;
    }




}
