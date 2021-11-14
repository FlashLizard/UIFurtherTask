using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SectorButtons : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private int nums = 4;
    [SerializeField]
    private Color[] colors = { Color.black, Color.white };
    private List<Image> images = new List<Image>();
    private List<GameObject> cards = new List<GameObject>();
    float interval;
    private void Awake()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;//设置点击有效区域
        float angle = 1;
        interval = 1f / nums;
        for (int i = 0; i < nums; i++, angle -= interval)
        {
            GameObject nObject = Data.Generate(@"UIs\SectorButton", gameObject);
            images.Add(nObject.GetComponent<Image>());
            cards.Add(null);
            images[i].fillAmount = angle;
            images[i] = images[i].transform.GetChild(0).gameObject.GetComponent<Image>();
            images[i].color = colors[i & 1];
        }
    }

    public void TryInsert(Vector2 eventPos/*为了区分屏幕坐标和世界坐标，建议写详细*/, GameObject card)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(eventPos);
        int index = GetIndex(pos);
        if (cards[index])
        {
            //如果原来有card，就交换card
            cards[index].SetActive(true);
            cards[index].GetComponent<Card>().MoveToExitPos(card.GetComponent<Card>().exitPos);//通过设置出发点实现交换
        }
        Insert(index, card);
    }

    private int GetIndex(Vector2 pos)//获取点击的是哪一块
    {
        Vector2 direction = (pos - (Vector2)transform.position).normalized;
        float angle = Mathf.Acos(Vector2.Dot(direction, new Vector2(1, 0)));
        if (direction.y < 0) angle = 2 * Mathf.PI - angle;
        return (int)(angle / (interval * Mathf.PI * 2));
    }
    private void Insert(int index, GameObject card)//
    {
        cards[index] = card;
        cards[index].SetActive(false);
        images[index].color = cards[index].GetComponent<Image>().color;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        int index = GetIndex(pos);
        if (cards[index])
        {
            cards[index].GetComponent<Card>().ExitFromRound(eventData.position);
            cards[index].GetComponent<Card>().OnBeginDrag(eventData);//模拟拖动。因为突然激活的，所以不会触发拖动
            cards[index] = null;
            images[index].color = colors[index & 1];
        }
    }
}
