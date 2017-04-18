using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class IntervalTriggerInspector : ActionBaseInspector
    {
        protected IntervalTriggerAction mAction
        {
            get { return this.mNode as IntervalTriggerAction;}
        }

        protected int mAddID = -1;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            this.mAction.IntervalTime = EditorGUILayout.FloatField("触发间隔时间:", this.mAction.IntervalTime);

            EditorGUILayout.LabelField("触发具体行为");

            ActionBase del = null;
            if(this.mAction.TriggerActions == null) this.mAction.TriggerActions = new List<ActionBase>();
            for (int i = 0; i < this.mAction.TriggerActions.Count; i++)
            {
                var action = this.mAction.TriggerActions[i];
                if (action == null)
                {
                    del = action;
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
//                    EditorGUILayout.ObjectField(action, typeof(ActionBase), false);
                    EditorGUILayout.LabelField(action.Name);
                    if (GUILayout.Button("删除"))
                    {
                        del = action;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (del != null)
                this.mAction.TriggerActions.Remove(del);

            if (ActionQueueEditor.instance != null)
            {
                EditorGUILayout.BeginHorizontal();
                this.mAddID = EditorGUILayout.IntField("添加的行为ID", this.mAddID);
                if (GUILayout.Button("添加"))
                {
                    var action = ActionQueueEditor.Active.GetActionByID(this.mAddID);
                    if (action != null)
                    {
                        action.AddPrevID(this.mAction.ID);
                        this.mAction.TriggerActions.Add(action);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}