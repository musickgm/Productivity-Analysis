using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRawInput;



public enum ApplicationType { work, browsing, leisure, inputBased};

[System.Serializable]
public struct TimeTypes
{
    public float work;
    public float browse;
    public float leisure;

}


public class App : MonoBehaviour
{
    public string appName;
    public bool active = false;
    public Color appColor;
    public ApplicationType appType;

    public float idleThreshold = 30;
    public float browsingThreshold = 30;

    public float secondsActive = 0;
    //public int mouseClicks = 0;
    public int keyboardClicks = 0;
    public float mouseMovementTime = 0;
    //public float mouseScrollTime = 0;
    //Specific time designations
    public TimeTypes timeTypes;

    private float mostRecentKeystroke;
    private IEnumerator InputCoroutine;

    private void Start()
    {
        RawKeyInput.Start(true);
    }




    public void Activate()
    {
        active = true;
        RawKeyInput.OnKeyDown += TrackKeyInput;
        InputCoroutine = DetermineInput();

        StartCoroutine(InputCoroutine);
    }

    public void Deactivate()
    {
        active = false;
        RawKeyInput.OnKeyDown -= TrackKeyInput;

        if (InputCoroutine != null)
        {
            StopCoroutine(InputCoroutine);
        }
    }

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

    private void TrackKeyInput(RawKey key)
    {
        keyboardClicks += 1;
        mostRecentKeystroke = Time.time;
    }


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

            yield return new WaitForEndOfFrame();
        }
    }
}
