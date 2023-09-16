using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Core.Editor.Impl
{
    public class CustomEditors : EditorWindow
    {
        protected static void HorizontalLine(Color color)
        {
            var horizontalLine = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.whiteTexture
                },
                margin = new RectOffset(0, 0, 4, 4),
                fixedHeight = 1
            };

            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, horizontalLine);
            GUI.color = c;
        }
    }
}
#endif