using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenStageEnterBtn : MonoBehaviour
{
    bool first=false;
    bool second = false;
    bool third = false;
    bool fourth = false;
    
    void FirstTrue()
    {
        first = true;
    }

    void SecondTrue() 
    {
        if(first)
            second = true;
    }


    void ThirdTrue()
    {
        if (second)
        {
            third = true;
        }
        else
        {
            first=false;
            second=false;
        }
    }





}
