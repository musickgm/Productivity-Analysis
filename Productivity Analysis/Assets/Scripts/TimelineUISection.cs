using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineUISection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text sectionText;

    private TimelineSection mySection;
    private Image myImage;
    private Color hoverColor;

    public void Initialize(TimelineSection _newSection, Vector2 position, float width)
    {
        mySection = _newSection;
        sectionText.text = mySection.appName + "\n" + mySection.startTime + " - " + mySection.endTime;
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        myImage = GetComponent<Image>();
        myImage.color = mySection.color;
        hoverColor = mySection.color;
        hoverColor.a = .5f;
    }

    public void ChangeColor(Color newColor)
    {
        mySection.color = newColor;
        myImage.color = mySection.color;
        hoverColor = mySection.color;
        hoverColor.a = .5f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sectionText.gameObject.SetActive(true);
        myImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sectionText.gameObject.SetActive(false);
        myImage.color = mySection.color;
    }
}
