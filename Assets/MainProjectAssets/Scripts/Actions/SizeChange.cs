//Stathis Georgiou ©2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Diadrasis.Mnesias.EnumsHolder;

namespace Diadrasis.Mnesias 
{
    /// <summary>
    /// change position from collider size
    /// </summary>
	public class SizeChange : MonoBehaviour
	{
        [SerializeField]
        private ColliderBoundPosition boundPosition = ColliderBoundPosition.Center;
        public float threshold = 0f;

        Vector3 pos, posNew;

        void OnBoreSizeChanged(Collider col)
        {
            pos = transform.position;
            switch (boundPosition)
            {
                case ColliderBoundPosition.Center:
                    posNew = col.bounds.center;
                    break;
                case ColliderBoundPosition.Start:
                    posNew = col.bounds.min;
                    break;
                case ColliderBoundPosition.End:
                    posNew = col.bounds.max;
                    break;
                default:
                    posNew = col.bounds.center;
                    break;
            }

            transform.position = new Vector3(posNew.x + threshold, pos.y, pos.z);
        }

        private void OnEnable()
        {
            ListenersHolder.OnBoreScaled += OnBoreSizeChanged;
            //ListenersHolder.OnConeScaled += OnConeSizeChanged;
        }

        private void OnDisable()
        {
            ListenersHolder.OnBoreScaled -= OnBoreSizeChanged;
        }

        private void OnDestroy()
        {
            ListenersHolder.OnBoreScaled -= OnBoreSizeChanged;
            //ListenersHolder.OnConeScaled -= OnConeSizeChanged;
        }

        /*void OnConeSizeChanged(Collider col)
        {
            pos = transform.position;
            switch (boundPosition)
            {
                case ColliderBoundPosition.Center:
                    posNew = col.bounds.center;
                    //posNew = col.bounds.extents.y;
                    break;
                case ColliderBoundPosition.Start:
                    posNew = col.bounds.min;
                    break;
                case ColliderBoundPosition.End:
                    posNew = col.bounds.max;
                    break;
                default:
                    posNew = col.bounds.center;
                    break;
            }

            transform.position = new Vector3(posNew.x, pos.y + threshold, pos.z);
        }*/
        
    }

}
