using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text BestScore;
    public Text NowScore;
    public Text inGameTxt;

    public GameObject Fail;
    public GameObject Victory;
    public GameObject BestScorePanel;

    public RectTransform BestScoreTitle;
    public RectTransform NowScoreTitle;
    public RectTransform BestScoreR;
    public RectTransform NowScoreR;

    public Card firstCard;
    public Card secondCard;

    
    public float totaltime = 45f;

    public int cardNum;
    
    [SerializeField] private AudioClip match; //카드 매치 사운드
    [SerializeField] private AudioClip missMatch; //카드 미쓰매치 사운드
    AudioSource ads;
    string card1Str = "르탄1 카드 모든카드공개!";
    string card2Str = "르탄2 카드 시간10초추가!";
    float time;
    int score;
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
        time = 0f;
        score = 0;
        Debug.Log(Board.board);
        ads = GetComponent<AudioSource>();
        if (TimerBar.Instance != null)
        {
            totaltime = TimerBar.Instance.GettingTotalTime();
            Debug.Log($"현재 totalTime : {totaltime}");
            Debug.Log($"현재 GettingTotalTime : {TimerBar.Instance.GettingTotalTime()}");
        }
        Board.board.BoardSetting(()=>StartFlip(DifficultSetting.Instance.Getting()));

    }

    // Update is called once per frame
    void Update()
    {
        if (TimerBar.Instance != null)
        {
            time = TimerBar.Instance.elapsedTime;

            if (time >= totaltime)
            {
                FailActive();
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
            score += 5;
            NowScore.text = score.ToString();
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
        BestScoreTitle.anchoredPosition = new Vector2(220, 200);
        NowScoreTitle.anchoredPosition = new Vector2(-220, 200);
        BestScoreR.anchoredPosition = new Vector2(220, 110);
        NowScoreR.anchoredPosition = new Vector2(-220, 110);
        Score();
        Victory.SetActive(true) ;
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

    public void Score()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            float best = PlayerPrefs.GetInt("BestScore");
            if (best < score)
            {
                PlayerPrefs.SetInt("BestScore", score);
                BestScore.text = score.ToString();
            }
            else
            {
                BestScore.text = best.ToString();
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", score);
            BestScore.text = score.ToString();
        }
        BestScorePanel.SetActive(true);
    }

    public void CardMissMatch()
    {
        firstCard.InvokeMissMatched();
        secondCard.InvokeMissMatched();
        ads.PlayOneShot(missMatch);
        TimerBar.Instance.elapsedTime += 1.5f;
    }

    public void FailActive()
    {
        Score();
        BestScoreTitle.anchoredPosition = new Vector2(-120, 110);
        NowScoreTitle.anchoredPosition = new Vector2(-120, -30);
        BestScoreR.anchoredPosition = new Vector2(120, 110);
        NowScoreR.anchoredPosition = new Vector2(120, -30);
        Fail.SetActive(true);
        Time.timeScale = 0f;
    }


}
