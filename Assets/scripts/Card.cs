using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int idx = 0;

    public Animator anim;
    public GameObject front;
    public GameObject back;
    public SpriteRenderer frontImage;
    
    [SerializeField] private AudioClip flip; //카드 뒤집기 사운드
    
    AudioSource ads;


    private void Start()
    {
        ads = GetComponent<AudioSource>();  


    }
    public void SelectedCard()
    {
        anim.SetBool("isSelected", true);
        InvokeSetActiveFront();
        InvokeSetUnActiveBack();
        ads.PlayOneShot(flip);


    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"Phoenix{num}");
    }


    public void InvokeMissMatched()
    {
        Invoke("MissMatched", 1f);
    }
    public void MissMatched()
    {

        anim.SetBool("isSelected", false);
        InvokeSetUnActiveFront();
        InvokeSetActiveBack();
        

    }



    void InvokeSetActiveFront()
    {
        Invoke("SetActiveFront", 0.5f);
    }

    public void SetActiveFront()
    {
        front.SetActive(true);
    }

    void InvokeSetUnActiveFront()
    {
        Invoke("SetUnActiveFront", 0.5f);
    }

    void SetUnActiveFront()
    {
        front.SetActive(false);
    }

    void InvokeSetUnActiveBack()
    {
        Invoke("SetUnActiveBack", 0.5f);
    }
    void SetUnActiveBack()
    {
        back.SetActive(false);
    }

    void InvokeSetActiveBack()
    {
        Invoke("SetActiveBack", 0.5f);
    }
    void SetActiveBack()
    {
        back.SetActive(true);
    }

    public void OpenCard()
    {
        SelectedCard();
        
            if (GameManager.Instance.firstCard == null)
            {
                GameManager.Instance.firstCard = this;
                
        }
            else
            {  
            
                GameManager.Instance.secondCard = this;
              
                GameManager.Instance.Matched();
                
            }
    }

    public void DestroyCard()
    {
        Invoke("DestoryCardInvoke", 1.0f);
    }

    void DestoryCardInvoke()
    {
        Destroy(gameObject);
    }


}