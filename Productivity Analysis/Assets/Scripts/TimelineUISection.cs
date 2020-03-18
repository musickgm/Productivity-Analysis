using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class for each section of the timeline - handles hover over behaviour
/// </summary>
public class TimelineUISection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text sectionText;                    //Text to display when hovered over

    private TimelineSection mySection;
    private Image myImage;
    private Color hoverColor;

    /// <summary>
    /// Initialize the timeline section
    /// </summary>
    /// <param name="_newSection"></param> section of timeline with appropriate data
    /// <param name="position"></param>Where to position this timeline? Right after previous section
    /// <param name="width"></param>How long is this section (width)
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

    /// <summary>
    /// Set the appropraite color after initialization
    /// </summary>
    /// <param name="newColor"></param>
    public void ChangeColor(Color newColor)
    {
        mySection.color = newColor;
        myImage.color = mySection.color;
        hoverColor = mySection.color;
        hoverColor.a = .5f;
    }

    /// <summary>
    /// When the pointer enters, highlight and activate text
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        sectionText.gameObject.SetActive(true);
        myImage.color = hoverColor;
    }

    /// <summary>
    /// When pointer exits, unhighlight and remove text
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        sectionText.gameObject.SetActive(false);
        myImage.color = mySection.color;
    }
}
