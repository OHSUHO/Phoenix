using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board board; // 싱글 톤 
    public GameObject card; // 배치할 카드 오브젝트
    Card cardScript;
    public int cardNum; // 카드 개수
    int raw; // 카드 배치 열의 수
    // Start is called before the first frame update

    private void Awake()
    {
        if (board == null)
        {
            board = GetComponent<Board>();
        }
    }

    void Start()
    {   int []arr = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10 };
        //arr = arr.OrderBy(x => Random.Range(0f,10f)).ToArray();
        raw = 4;
        
        cardNum = 20;
        for(int i = 0; i < cardNum; i++)
        {
            float x = (i % raw)*1.2f +0.25f; 
            float y = (i / raw)*1.2f + 1.2f;
            GameObject go = Instantiate(card,this.transform);
            go.transform.position = new Vector2(go.transform.position.x + x, go.transform.position.y + y);
            cardScript = go.GetComponent<Card>();
            cardScript.Setting(arr[i]);
        }
        
    }

    
}
