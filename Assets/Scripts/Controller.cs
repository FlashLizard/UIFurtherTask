using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //private SectorButtons m_round;
    //private Plant m_plant;
    public static Controller current;
    public SectorButtons Round(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, 1000, LayerMask.GetMask("Round"));
        return raycastHits.Length > 0 ? raycastHits[0].collider.gameObject.GetComponent<SectorButtons>() : null;
    }
    public Plant Plant(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, 1000, LayerMask.GetMask("Plant"));
        return raycastHits.Length > 0 ? raycastHits[0].collider.gameObject.GetComponent<Plant>() : null;
    }
    private void Awake()
    {
        current = this;
    }
    private void Update()
    {

    }
}
