using StaGeGames.SmartUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diadrasis.Mnesias.UI
{

    public class ViewPanel : MonoBehaviour
    {
        RenderTexture renderTexture;
        public RectTransform viewPanel;
        public Camera camInstrument;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            SetRenderTexture();
            viewPanel.GetComponent<SmartMotion>().ShowPanel();
        }

        public void SetRenderTexture()
        {
            RenderTexture renderTex = new RenderTexture(Mathf.RoundToInt(viewPanel.sizeDelta.x), Mathf.RoundToInt(viewPanel.sizeDelta.y), 16, RenderTextureFormat.ARGB32);
            renderTex.isPowerOfTwo = false;
            renderTex.Create();
            viewPanel.GetComponent<RawImage>().texture = renderTex;
            camInstrument.targetTexture = renderTex;
        }
    }

}
