//Stathis Georgiou ©2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diadrasis.Mnesias {

	public class ReplaceTextsToTMPro : MonoBehaviour
	{
		[ContextMenu("Replace All Texts to TMPro texts")]
		public void ReplaceAll()
        {
			Canvas[] canvases = FindObjectsOfType<Canvas>();

			foreach(Canvas canv in canvases)
            {
				Text[] txts = canv.GetComponentsInChildren<Text>(true);

				foreach(Text txt in txts)
                {
					GameObject target = txt.gameObject;

                    string val = txt.text;
                    int fontSize = txt.fontSize;
                    TextAnchor al = txt.alignment;
                    FontStyle fontStyle = txt.fontStyle;
                    Color color = txt.color;

                    InputField inputField = target.transform.parent.GetComponent<InputField>();
                    if (inputField != null)
                    {
                        if(inputField.textComponent == txt || inputField.placeholder.GetComponent<Text>() == txt)
                        {
                            Debug.Log("this is inside input!");
                            continue;
                        }

                    }

                    DestroyImmediate(txt);

					TMPro.TextMeshProUGUI newtxt = target.AddComponent<TMPro.TextMeshProUGUI>();
                    newtxt.text = val;
                    newtxt.fontSize = fontSize;
                    SetTextAlignment(al, newtxt);
                    newtxt.fontStyle = GetFontStyle(fontStyle);
                    newtxt.color = color;
                }

            }

        }

        void SetTextAlignment(TextAnchor textAnchor, TMPro.TextMeshProUGUI tmproTarget)
        {
            switch (textAnchor)
            {
                case TextAnchor.UpperLeft:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Top;
                    break;
                case TextAnchor.UpperCenter:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Top;
                    break;
                case TextAnchor.UpperRight:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Top;
                    break;
                case TextAnchor.MiddleLeft:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
                    break;
                case TextAnchor.MiddleCenter:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
                    break;
                case TextAnchor.MiddleRight:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
                    break;
                case TextAnchor.LowerLeft:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Bottom;
                    break;
                case TextAnchor.LowerCenter:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Bottom;
                    break;
                case TextAnchor.LowerRight:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Bottom;
                    break;
                default:
                    tmproTarget.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                    tmproTarget.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
                    break;
            }
        }

        TMPro.FontStyles GetFontStyle(FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case FontStyle.Normal:
                    return TMPro.FontStyles.Normal;
                case FontStyle.Bold:
                    return TMPro.FontStyles.Bold;
                case FontStyle.Italic:
                    return TMPro.FontStyles.Italic;
                case FontStyle.BoldAndItalic:
                    return TMPro.FontStyles.Bold | TMPro.FontStyles.Italic ;
                default:
                    return TMPro.FontStyles.Normal;
            }
        }

    }

}
