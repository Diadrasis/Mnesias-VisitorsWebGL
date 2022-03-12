using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diadrasis.Mnesias.Settings
{

    public class PlatformSettings : MonoBehaviour
    {
        public delegate void LanguageAction();
        public static LanguageAction OnChangeLanguage;

        public static bool isEditor;
        public static bool isMobile;
        public static bool isDesctop;
        public static bool isAndroid;
        public static bool isWindows;
        public static bool isMac;

        public static bool hasMobileGyro;

        //mouse over switch
        public static bool isMouseOverEnabled;

        public static string Language = "en";

        public enum LangMode { ENGLISH, GREEK };
        public LangMode langMode = LangMode.GREEK;

        private void Awake()
        {
            Init();
            OnChangeLanguage += ChangeLanguage;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.I)) isMouseOverEnabled = true;

            if (Input.GetKeyUp(KeyCode.I)) isMouseOverEnabled = false;
        }

        private void Init()
        {
            Language = Application.systemLanguage == SystemLanguage.English ? "en" : "el";
            isEditor = Application.platform == RuntimePlatform.WindowsEditor ? true : false;
            isWindows = Application.platform == RuntimePlatform.WindowsPlayer ? true : false;
            isMac = Application.platform == RuntimePlatform.OSXPlayer ? true : false;
            isAndroid = Application.platform == RuntimePlatform.Android ? true : false;
            isMobile = Application.platform == RuntimePlatform.Android ? true : false;
            if (!isMobile) isMobile = Application.platform == RuntimePlatform.IPhonePlayer ? true : false;
            if (isMobile) { hasMobileGyro = SystemInfo.supportsGyroscope; }
            if (isMac || isWindows) { isDesctop = true; } else { isDesctop = false; }
        }


        private void ChangeLanguage()
        {
            Language = Language == "el" ? "en" : "el";
            langMode = Language == "el" ? LangMode.GREEK : LangMode.ENGLISH;
        }

    }

}
