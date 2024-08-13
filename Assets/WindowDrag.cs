using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IDragHandler
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
}
