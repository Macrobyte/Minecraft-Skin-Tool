using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IDragHandler, IPointerDownHandler
{

    [SerializeField]
    private RectTransform windowDragTransform;

    [SerializeField]
    private Canvas canvas;


    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        windowDragTransform.anchoredPosition += eventData.delta / canvas.scaleFactor ;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        windowDragTransform.SetAsLastSibling();
    }
}
