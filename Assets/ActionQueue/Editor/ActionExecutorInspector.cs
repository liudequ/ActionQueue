using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ActionQueue
{

    [CustomEditor(typeof(ActionExecutor))]
    public class ActionExecutorInspector : Editor
    {

        public static ActionBase SelectEditorAction;


        private Dictionary<int, ActionBaseInspector> map = new Dictionary<int, ActionBaseInspector>();

        public override void OnInspectorGUI()
        {
            var actionExecutor = this.target as ActionExecutor;
            if (SelectEditorAction != null && ActionQueueEditor.instance != null)
            {
                ActionBaseInspector inspector = null;
                if (map.ContainsKey(SelectEditorAction.ID))
                {
                    inspector = map[SelectEditorAction.ID];
                }
                else
                {
                    inspector = GetMatchInspector(SelectEditorAction);
                }
                
                inspector.SetNode(SelectEditorAction);
                inspector.OnHeaderGUI();
                inspector.OnInspectorGUI();
            }
            else
            {
//                foreach (var action in actionExecutor.Actions)
//                {
//                    EditorGUILayout.LabelField(action.Name);
//                }
                base.OnInspectorGUI();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this.target);
            }
        }

        public ActionBaseInspector GetMatchInspector(ActionBase action)
        {
            if (action is TweenAlphaAction)
            {
                return new TweenAlphaInspector();
            }
            else if (action is NumberRollAction)
            {
                return  new NumberRollInspector();
            }
            else if (action is DelayAction)
            {
                return new DelayInspector();
            }
            else if (action is IntervalTriggerAction)
            {
                return  new IntervalTriggerInspector();
            }
            else if (action is ActivateAction)
            {
                return  new ActivateInspactor();
            }
            else if (action is TweenPositionAction)
            {
                return new TweenPositionInspector();
            }
            return new ActionBaseInspector();
        }
    }
}

