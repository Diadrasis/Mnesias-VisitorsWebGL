using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace StaGeGames.SmartUI
{
    [AddComponentMenu("UI/#SmartUI/SmartResize")]
    [RequireComponent(typeof(RectTransform))]
    public class SmartResize : MonoBehaviour
    {

        public RectTransform target, targetParent;
        [Tooltip("If is been set width or height percentage keep aspect ratio")]
        public bool keepAspectRatio;
        [Tooltip("Keep position x,y as is")]
        public bool keepRelativePosition;

        [HideInInspector]
        public float widthPercent, heightPercent;

        public float WidthFinal
        {
            get {
                if (widthPercent == 0) { return FinalTargetWidth(); }// targetParent.sizeDelta.x; }
                return (targetParent.sizeDelta.x * widthPercent) / 100f;
            }
        }

        private float FinalTargetWidth()
        {
            if (keepAspectRatio) {
                if (widthPercent == 0 && heightPercent != 0)
                {
                    float targetAspectRatio = target.sizeDelta.x > target.sizeDelta.y ? target.sizeDelta.x / target.sizeDelta.y : target.sizeDelta.y / target.sizeDelta.x;
                    return targetParent.sizeDelta.y * targetAspectRatio;
                }
            }
            return targetParent.sizeDelta.x; 
        }

        public float HeightFinal
        {
            get {
                if (heightPercent == 0) { return FinalTargetHeight(); }// targetParent.sizeDelta.y; }
                return (targetParent.sizeDelta.y * heightPercent) / 100f;
            }
        }

        private float FinalTargetHeight()
        {
            if (keepAspectRatio)
            {
                if (heightPercent == 0 && widthPercent != 0)
                {
                    float targetAspectRatio = target.sizeDelta.x > target.sizeDelta.y ? target.sizeDelta.x / target.sizeDelta.y : target.sizeDelta.y / target.sizeDelta.x;
                    return targetParent.sizeDelta.x * targetAspectRatio;
                }
            }
            return targetParent.sizeDelta.y;
        }

        public bool isMovable, isVisibleOnStart;

        //the size of canvas during development
        Vector2 initParentSize;
        bool hasLayoutElement;
        LayoutElement layOutElem;

        [Header("Pivot point")]
        public GuiUtilities.PivotMode pivotMode = GuiUtilities.PivotMode.LeftCenter;

        //offset

        private void Start()
        {
            if (!HasParentSmartResize(out float waitTime)) { Init(); } else { Invoke("Init", waitTime); }
        }

        private bool HasParentSmartResize(out float val)
        {
            val = 0f;
            if (targetParent)
            {
                int p = 0;
                if (targetParent.GetComponent<SmartResize>() != null)
                {
                    p++;
                    Transform tp = targetParent.parent;
                    if (tp)
                    {
                        if (tp.GetComponent<SmartResize>() != null) { p++; }

                        tp = targetParent.parent.parent;
                        if (tp)
                        {
                            if (tp.GetComponent<SmartResize>() != null) { p++; }
                            tp = targetParent.parent.parent.parent;
                            if (tp)
                            {
                                if (tp.GetComponent<SmartResize>() != null) { p++; }
                                tp = targetParent.parent.parent.parent.parent;
                                if (tp)
                                {
                                    if (tp.GetComponent<SmartResize>() != null) { p++;  }
                                    tp = targetParent.parent.parent.parent.parent.parent;
                                    if (tp)
                                    {
                                        if (tp.GetComponent<SmartResize>() != null) { p++;  }
                                        tp = targetParent.parent.parent.parent.parent.parent.parent;
                                        if (tp)
                                        {
                                            if (tp.GetComponent<SmartResize>() != null) { p++; }
                                            tp = targetParent.parent.parent.parent.parent.parent.parent.parent;
                                            if (tp)
                                            {
                                                if (tp.GetComponent<SmartResize>() != null) { p++; }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    val = p * 0.1f;
                    return true;
                }
            }
            return false;
        }

        public void Init()
        {
            
            if (target == null) target = GetComponent<RectTransform>();
            if (targetParent == null) targetParent = FindObjectOfType<Canvas>().GetComponent<RectTransform>();

            initParentSize = targetParent.sizeDelta;

            layOutElem = target.gameObject.GetComponent<LayoutElement>();

            if (layOutElem && !hasLayoutElement) hasLayoutElement = true;
            
            Vector2 targetSize = initParentSize;
            bool isWidthBigger = target.sizeDelta.x > target.sizeDelta.y;
            float targetAspectRatio = target.sizeDelta.x / target.sizeDelta.y;

           // Debug.Log(gameObject.name+": "+ isWidthBigger + " - " + targetAspectRatio);

            if (widthPercent > 0) targetSize.x = (targetSize.x * widthPercent) / 100f;
            if (heightPercent > 0) targetSize.y = (targetSize.y * heightPercent) / 100f;

            if (keepAspectRatio)
            {
                Vector2 mySize = targetSize;
                if (widthPercent == 0 && heightPercent != 0)
                {
                    targetSize.x = isWidthBigger ? mySize.y / targetAspectRatio : mySize.y / targetAspectRatio;
                }

                if (heightPercent == 0 && widthPercent != 0)
                {
                    targetSize.y = isWidthBigger ? mySize.x / targetAspectRatio : mySize.x * targetAspectRatio;
                }
            }

            #region testing

            /*

            if (isWidthRelative)
            {

                float val = (targetSize.x / initParentSize.x) * target.sizeDelta.x;

                if (hasLayoutElement)
                {
                    layOutElem.minWidth = val;
                    layOutElem.preferredWidth = val;
                }
                else
                {
                    targetSize.x = val;
                }
            }

            if (isHeightRelative)
            {

                float val = (targetSize.y / initParentSize.y) * target.sizeDelta.y;

                if (hasLayoutElement)
                {
                    layOutElem.minHeight = val;
                    layOutElem.preferredHeight = val;
                }
                else
                {
                    targetSize.y = val;
                }
            }

            */
            #endregion

            target.sizeDelta = targetSize;

            GuiUtilities.SetPivot(target, pivotMode);

            target.anchoredPosition = Vector3.zero;

            GuiUtilities.ForceRebuildLayout(target);

            if (!isMovable) isMovable = GetComponent<SmartMotion>() != null;

            if (isMovable)
            {
                SmartMotion transitionClass = target.gameObject.GetComponent<SmartMotion>();

                if(transitionClass==null) transitionClass = target.gameObject.AddComponent<SmartMotion>();

                transitionClass.Init(target, pivotMode, isVisibleOnStart);
            }
        }

        private void Reset()
        {
            if (!GetComponent<CanvasRenderer>())
            {
                Debug.LogWarning("Can't add \"SmartResize\" to non-UI objects");
                DestroyImmediate(this);
                return;
            }
            if (gameObject.GetComponents<SmartResize>().Length > 1)
            {
                Debug.LogWarning("Can't add more than one \"SmartResize\" to " + gameObject.name);
                DestroyImmediate(this);
            }
        }
    }

}