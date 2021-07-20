#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace TestInvaders.UI
{
    [CustomEditor(typeof(HighScoresWindow))]
    public class HighScoresWindowEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Setup Score Items"))
            {
                var highScoresWindow = (HighScoresWindow) target;
                Undo.RecordObject(highScoresWindow, "Setup Score Items");
                highScoresWindow.SetupScoreItems();
            }
        }
    }
}

#endif