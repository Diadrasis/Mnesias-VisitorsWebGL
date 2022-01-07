//Stathis Georgiou ©2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diadrasis.Mnesias 
{

	public class ListenersHolder : MonoBehaviour
	{

        public delegate void ScaleAction(Collider col);
        public static ScaleAction OnBoreScaled, OnMouthScaled, OnBellScaledLength;
        public delegate void ScaleAction2(float bellLength, float bellGama);
        public static ScaleAction2 OnBellScaledGama;

        public delegate void PivotAction(Vector3 p, float size);
        public static PivotAction OnPivotChanged;
        public delegate void AppInitialization();
        public static AppInitialization OnAppInited;
        public delegate void JsonAction();
        public static JsonAction OnJsonApply;

        public delegate void ScreenAction();
        public static ScreenAction OnScreenRoationOccured;

        public delegate void AudioAction(float time, string txt);
        public static AudioAction OnClipPlayShowInfo;

        public delegate void InputHolesAction(int count);
        public static InputHolesAction OnHolesActivate, OnTubesActivate;

        public delegate void ScenarioAction(string msg);
        public static ScenarioAction OnScenarioSelected;

        public delegate void GlobalActions();
        public static GlobalActions OnGloballyWakeUpOnValueChanged, OnGlobalResetInputValues;

        public delegate void MessageAction(string message);
        public static MessageAction OnShowError;
    }

}
