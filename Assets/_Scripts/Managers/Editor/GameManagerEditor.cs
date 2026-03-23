using System.Linq;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public bool showFoldout;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager fsm = (GameManager)target;

        EditorGUILayout.Space(30f);
        EditorGUILayout.LabelField("State Machine");

        if (fsm.stateMachine == null) return;
        if (fsm.stateMachine.CurrentState != null)
            EditorGUILayout.LabelField("Current State: ", fsm.stateMachine.CurrentState.ToString());

        showFoldout = EditorGUILayout.Foldout(showFoldout, "Available States");

        if (showFoldout)
        {
            if (fsm.stateMachine.statesDictionary != null)
            {
                var keys = fsm.stateMachine.statesDictionary.Keys.ToArray();
                var vals = fsm.stateMachine.statesDictionary.Values.ToArray();

                for (int i = 0; i < keys.Length; i++)
                {
                    EditorGUILayout.LabelField($"{keys[i]} :: {vals[i]}");
                }
            }
        }
    }
}
