using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRawInput;


/// <summary>
/// Enum for application type
/// </summary>
public enum ApplicationType { work, browsing, leisure, misc};

/// <summary>
/// Struct for types of time
/// </summary>
[System.Serializable]
public struct TimeTypes
{
    public float work;
    public float browse;
    public float leisure;
    public float misc;
}

/// <summary>
/// Class for handling each app's data (both predefined and made during use)
/// </summary>
public class App : MonoBehaviour
{
    public string appName;                              //Name of app
    public string searchKey;                            //String to be searched for (lowercase)
    public bool active = false;                         //Currently being used?
    public Color appColor;                              //App color
    public ApplicationType appType;                     //App type

    public float idleThreshold = 30;                    //How long before considered idle for this app?
    public float browsingThreshold = 30;                //How long without keyboard input to be considered browsing?

    public float secondsActive = 0;
    public float sectionActive = 0;
    public int keyboardClicks = 0;
    public int spaceHits = 0;
    public int enterHits = 0;
    public float mouseMovementTime = 0;
    //Specific time designations
    public TimeTypes timeTypes;

    private float mostRecentKeystroke;
    private IEnumerator InputCoroutine;
    private TimelineSection timelineSection;

    private void Start()
    {
        RawKeyInput.Start(true);
        timelineSection.color = appColor;
        timelineSection.appName = appName;
        timelineSection.appType = appType;
    }


    /// <summary>
    /// Called when becoming active - start coroutines and log data
    /// </summary>
    public void Activate()
    {
        active = true;
        sectionActive = 0;
        timelineSection.startTime = System.DateTime.Now.ToString("hh:mm:ss");
        RawKeyInput.OnKeyDown += TrackKeyInput;
        InputCoroutine = DetermineInput();

        StartCoroutine(InputCoroutine);
    }

    /// <summary>
    /// Called when another app becomes active
    /// </summary>
    public void Deactivate()
    {
        active = false;
        timelineSection.endTime = System.DateTime.Now.ToString("hh:mm:ss");
        timelineSection.secLength = sectionActive;
        AnalyticsManager.Instance.AddTimelineSection(timelineSection);

        RawKeyInput.OnKeyDown -= TrackKeyInput;

        if (InputCoroutine != null)
        {
            StopCoroutine(InputCoroutine);
        }
    }

    /// <summary>
    /// Track keyboard clicks
    /// </summary>
    private void Update()
    {
        if(active)
        {
            //Determine number of clicks and keyboard presses
            if(Input.anyKeyDown)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    //mouseClicks++;
                }
                else
                {
                    keyboardClicks++;
                }
            }


        }
    }

    /// <summary>
    /// Using Unity raw input open source project to track input outside of unity
    /// </summary>
    /// <param name="key"></param>
    private void TrackKeyInput(RawKey key)
    {
        keyboardClicks += 1;
        if(key == RawKey.Return)
        {
            enterHits++;
        }
        else if(key == RawKey.Space)
        {
            spaceHits++;
        }

        mostRecentKeystroke = Time.time;
    }


    /// <summary>
    /// Coroutine to determine input (how long since mouse, how long since keyboard, etc.) -> categorize
    /// </summary>
    /// <returns></returns>
    private IEnumerator DetermineInput()
    {
        float currentIdleTime = 0;
        float timeSinceLastType = 0;
        bool browsing = false;
        bool idling = false;
        while (active)
        {

            float elapsedTime = Time.deltaTime;
            bool typing = false;
            bool mousing = false;

            if(MouseInput.Instance.IsMouseMoving())
            {
                mouseMovementTime += elapsedTime;
                mousing = true;
            }

            if(Time.time - mostRecentKeystroke <= 1)
            {
                typing = true;
                browsing = false;
                timeSinceLastType = 0;
            }


            //Determine if idle
            if (typing || mousing)
            {
                idling = false;
                currentIdleTime = 0;
            }
            else            //Check to see if idle time should increase
            {
                currentIdleTime += elapsedTime;

                if(currentIdleTime > idleThreshold)
                {
                    if (idling == false)
                    {
                        idling = true;
                    }
                    else
                    {
                        timeTypes.leisure += elapsedTime;
                    }
                }
            }

            //Check to see if browsing time should increase
            if (!typing)
            {
                timeSinceLastType += elapsedTime;
                if(timeSinceLastType > browsingThreshold && ! idling)
                {
                    if (browsing == false)
                    {
                        browsing = true;
                    }
                    else
                    {
                        timeTypes.browse += elapsedTime;
                    }
                }
            }

            //Add to work time
            if(!idling && !browsing)
            {
                timeTypes.work += elapsedTime;
            }
            secondsActive += elapsedTime;
            sectionActive += elapsedTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
