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

    [SerializeField]
    private RectTransform toolbarTransform;


    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        
        toolbarTransform = (RectTransform)canvas.transform.Find("Toolbar");
    }

    public void OnDrag(PointerEventData eventData)
    {
        windowDragTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        int toolbarIndex = toolbarTransform.GetSiblingIndex();

        windowDragTransform.SetSiblingIndex(toolbarIndex - 1);

    }
}
