using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Fail;
    public GameObject Victory;

    public Card firstCard;
    public Card secondCard;

    public float totaltime;
    [SerializeField] private AudioClip match; //카드 매치 사운드
    AudioSource ads;
    float time = 0f;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        ads = GetComponent<AudioSource>();
        totaltime = 50f;
        TimerBar.Instance.TimeSetting(50f);
    }

    // Update is called once per frame
    void Update()
    {
        time = TimerBar.Instance.elapsedTime;

        if (time >= totaltime)
        {
            time = totaltime;
            GameOver();
        }
    }
    public void GameOver()
    {
        if (time >= totaltime)
        {
            Fail.SetActive(true);
        }
        else
        {
            Victory.SetActive(true);
        }
        Time.timeScale = 0;
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx && (firstCard!=secondCard))
        {
            if(firstCard.idx == 1)
            {
                FlipAll();
                InvokeBackFlipAll();
                TimerBar.Instance.TimeFreezing(3f);
            }
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            ads.PlayOneShot(match);
        }
        else
        {
            CardMissMatch();
           
        }

        firstCard = null;
        secondCard = null;
    }

    public void FlipAll()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Card");
        Debug.Log(go.Length);
        for (int i = 0; i < go.Length; i++) 
        {
           
            Card cardScript = go[i].GetComponent<Card>();
            cardScript.SelectedCard ();
            
        }
        
    }

    void InvokeBackFlipAll()
    {
        Invoke("BackFlipAll",2f);
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
        TimerBar.Instance.elapsedTime += 1.5f;
    }




}
