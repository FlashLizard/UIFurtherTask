using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject plant;
    private GameObject restCards;
    private GameObject round;
    private GameObject nowParent;
    public Vector2 exitPos=Vector2.zero;
    private bool isDraging = false;

    private void Awake()
    {
        gameObject.GetComponent<Image>().color = new Color(Random.Range(0.6f,1f), Random.Range(0.6f, 1f), Random.Range(0.6f, 1f));
        nowParent = plant = gameObject.transform.parent.gameObject;
        restCards = GameObject.Find("ExtraCards");
    }
    public void ExitFromRound(Vector3 eventPos)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventPos);
        pos.z = transform.position.z;//设置成最初的z，否则是eventPos转化后的z
        gameObject.SetActive(true);
        gameObject.transform.position = pos;
        gameObject.transform.SetParent(restCards.transform);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
        exitPos = eventData.position;//记录出发点，以便后面不能插入时还原
        gameObject.transform.SetParent(restCards.transform);
    }

    public void OnDrag(PointerEventData eventData)//没有这个不能触发OnBeginDrag
    {
    }

    public void Update()
    {
        //模拟拖拽，因为如果突然setactive，是不会触发普通拖拽的
        if (Input.GetKey(KeyCode.Mouse0) && isDraging)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = transform.position.z;
            transform.position = pos;
        }
        //模拟停止拖拽
        if (Input.GetKeyUp(KeyCode.Mouse0) && isDraging)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            OnEndDrag(eventData);
        }
    }

    public void MoveToExitPos()
    {
        PointerEventData eventData= new PointerEventData(EventSystem.current);
        eventData.position = exitPos;
        OnEndDrag(eventData);
    }
    public void MoveToExitPos(Vector2 pos)
    {
        exitPos = pos;
        MoveToExitPos();
    }

    private void MoveToParent(GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        nowParent = parent;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        if (Controller.current.Round(eventData.position))
        {
            round = Controller.current.Round(eventData.position).gameObject;
            MoveToParent(round);
            round.GetComponent<SectorButtons>().TryInsert(eventData.position, gameObject);
        }
        else if (Controller.current.Plant(eventData.position))
        {
            plant = Controller.current.Plant(eventData.position).gameObject;
            MoveToParent(plant);
        }
        else
        {
            //如果上面都不可，则回到出发点
            MoveToExitPos();
        }
        isDraging = false;
    }
}
