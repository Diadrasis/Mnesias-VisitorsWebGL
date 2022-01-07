using Diadrasis.Mnesias.Control;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diadrasis.Mnesias
{

    public class MainManager : MonoBehaviour
    {

        private SmoothOrbitCam orbitCam;


        private void Awake()
        {
            orbitCam = FindObjectOfType<SmoothOrbitCam>();
            LeanSelectable.OnSelectGlobal += OnHoleSelected;
            LeanSelectable.OnDeselectGlobal += OnHoleDeselected;
        }

        void OnHoleSelected(LeanSelectable ls, LeanFinger lf)
        {
            //Debug.LogWarning(ls.name);
            //SetCameraStatic(true);
        }

        void OnHoleDeselected(LeanSelectable ls)
        {
            //Debug.LogWarning(ls.name);
           // SetCameraStatic(false);
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.25f);
            //SetCameraStatic(true);
        }

        void SetCameraStatic(bool val)
        {
            orbitCam.EnableOrbiting = !val;
            orbitCam.enablePanning = !val;
            orbitCam.enableZooming = !val;
        }
    }


}