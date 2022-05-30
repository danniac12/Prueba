using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
{
     private RectTransform dragRectTransfom;
     public RectTransform ReprobadoRectTransfom;
     public RectTransform AprobadoRectTransfom;
     public RectTransform ParentRectTransfom;
    public float note;
    public string nameStudent;
    private Canvas canvas;
    [SerializeField] private Image backgroundImage;
    private Color backgroundColor;
    void Awake()
    {
        backgroundColor = backgroundImage.color;
        if(dragRectTransfom == null)
        {
            dragRectTransfom= transform.parent.GetComponent<RectTransform>();
        }
        if(canvas == null)
        {
            Transform testCanvasTransform = transform.parent;
            while(testCanvasTransform != null)
            {
                canvas = testCanvasTransform.GetComponent<Canvas>();
                if(canvas != null) { 
                    break;
                }
                testCanvasTransform = testCanvasTransform.parent;   
            }
        }

       
    }
   

   

    public void OnBeginDrag(PointerEventData eventData)
    {
        backgroundColor.a = .4f;  
        backgroundImage.color = backgroundColor;
      //  dragRectTransfom.SetParent(null);
     
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransfom.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        backgroundColor.a = 1f;
        backgroundImage.color = backgroundColor;
        if(Vector3.Distance( dragRectTransfom.position , ReprobadoRectTransfom.position) < 150)
        {
            dragRectTransfom.SetParent(ReprobadoRectTransfom);
        }
        else if(Vector3.Distance(dragRectTransfom.position, AprobadoRectTransfom.position) < 150)
        {
            dragRectTransfom.SetParent(AprobadoRectTransfom);
        }
      
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransfom.SetAsLastSibling();
    }
}
