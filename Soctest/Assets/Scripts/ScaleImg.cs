using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleImg : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    private SpriteRenderer spriteQuad;
    public GameObject quad;

    void Start()
    {
        spriteQuad = quad.GetComponent<SpriteRenderer>();

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
        {
            if (eventData.delta.y > 0)
            {
                gameObject.GetComponent<Animator>().SetTrigger("OutScale");
                gameObject.GetComponent<Animator>().SetTrigger("OutToScale");
            }
        }

    }
    public void OnDrag(PointerEventData eventData) { }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            gameObject.GetComponent<Animator>().SetTrigger("InScale");
        }
    }
}
