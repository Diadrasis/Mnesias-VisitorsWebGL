using UnityEditor;
using UnityEngine;

namespace StaGeGames.SmartUI
{

    [CustomEditor(typeof(SmartResize))]
    public class SmartResizeEditor : Editor
    {
        bool showVariables = true;
        string btnLabel = "Show Script Variables";

        public override void OnInspectorGUI()
        {
            SmartResize myTarget = (SmartResize)target;

            if (myTarget.target == null) myTarget.target = myTarget.GetComponent<RectTransform>();
            if (myTarget.targetParent == null) myTarget.targetParent = FindObjectOfType<Canvas>().GetComponent<RectTransform>();

            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = GuiUtilities.HexColor("#C2C2C2", Color.gray);

            GUIStyle TextFieldStyles = new GUIStyle(EditorStyles.textField);
            TextFieldStyles.richText = true;

            string s = "©<color=cyan>Sta</color><color=red>Ge</color> 2020 - <color=#006DAB>stagegames.eu</color>";
            if (GUILayout.Button(s, TextFieldStyles))
            {
                Application.OpenURL("http://stagegames.eu");
            }

            TextFieldStyles.fontSize = 14;
            TextFieldStyles.fontStyle = FontStyle.Bold;
            TextFieldStyles.normal.textColor = Color.yellow;
            TextFieldStyles.alignment = TextAnchor.MiddleCenter;

            GUI.backgroundColor = Color.black;

            if (GUILayout.Button(btnLabel, TextFieldStyles))
            {
                showVariables = !showVariables;
                btnLabel = showVariables ? "Hide Script Variables" : "Show Script Variables";
            }
            GUI.backgroundColor = oldColor;
            GUI.color = Color.white;
            if (showVariables) DrawDefaultInspector();


            myTarget.widthPercent = EditorGUILayout.FloatField("Width Percent", myTarget.widthPercent);
            EditorGUILayout.LabelField("Final Width", myTarget.WidthFinal.ToString());

            myTarget.heightPercent = EditorGUILayout.FloatField("Height Percent", myTarget.heightPercent);
            EditorGUILayout.LabelField("Final Height", myTarget.HeightFinal.ToString());

            if(!showVariables) myTarget.isMovable = EditorGUILayout.Toggle("Is Panel Movable?", myTarget.isMovable);
            if (!showVariables) myTarget.isVisibleOnStart = EditorGUILayout.Toggle("Should Panel be Visible on Start?", myTarget.isVisibleOnStart);

            GUI.color = Color.cyan;
            if (GUILayout.Button("Set Panel Size")) { myTarget.Init(); }

        }
    }

}
