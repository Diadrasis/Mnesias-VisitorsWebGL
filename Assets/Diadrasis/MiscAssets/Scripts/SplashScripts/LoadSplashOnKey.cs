using Diadrasis.Mnesias;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSplashOnKey : Singleton<LoadSplashOnKey>
{
    protected LoadSplashOnKey() { }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F5))
        {
            SceneManager.LoadScene("Splash");
        }
    }
}
