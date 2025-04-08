using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelect : MonoBehaviour
{
    public Animator anim;
    public GameObject front;
    public GameObject back;
    private void Start()
    {
        
    }
    public void SelectedCard()
    {
        anim.SetBool("isSelected", true);
        InvokeSetActiveFront();
        InvokeSetUnActiveBack();
        Invoke("MissMatched", 1.0f);
        
        
    }

    void MissMatched()
    {
        anim.SetBool("isSelected", false);
        InvokeSetUnActiveFront();
        InvokeSetActiveBack();
    }

    
    void InvokeSetActiveFront()
    {
        Invoke("SetActiveFront", 0.5f);
    }

    void SetActiveFront()
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
}
