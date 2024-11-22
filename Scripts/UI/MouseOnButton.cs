using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject SelectImage;
    public Image BackGroundImage;
    public bool isClicked = false;

    void Start()
    {
        BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectImage.SetActive(true);
        BackGroundImage.color = new Color(1f, 1f, 1f, 1);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(isClicked == false)
        {
            SelectImage.SetActive(false);
            BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);
        }
    }
}
