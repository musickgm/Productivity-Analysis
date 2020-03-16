using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Windows;

public struct Point
{
    public int x;
    public int y;
}

public class MouseInput : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Point pos);

    Point cursorPos = new Point();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCursorPos(out cursorPos);

        Debug.Log(cursorPos);
    }
}
