using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class WindowLayout : MonoBehaviour
{
    Rect windowRect = new Rect(20, 20, 120, 50);

    /*void OnGUI()
    {
        // Register the window. Notice the 3rd parameter
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "My Window");
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        // This button will size to fit the window
        if (GUILayout.Button("Hello World"))
        {
            print("Got a click");
        }
    }*/

    //Import the following.
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, string lpString);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern System.IntPtr FindWindow(System.String className, System.String windowName);
    WindManager wind;
    System.IntPtr windowPtr;
    private void Awake()
    {
        //Get the window handle.
        
        //Set the title text using the window handle.
        //if (wind.isMegara && !wind.isPiraeus)
        
        /*if(wind.isPiraeus && !wind.isMegara)
            SetWindowText(windowPtr, wind.btnMuseumPiraeus.GetComponentInChildren<TextMeshProUGUI>() + " - Mnesias");*/
    }
    private void Update()
    {
        windowPtr = FindWindow(null, "Mnesias");
        //SetWindowText(windowPtr, wind.btnMuseumMegara.GetComponentInChildren<TextMeshProUGUI>() + " - Mnesias");
    }


}
