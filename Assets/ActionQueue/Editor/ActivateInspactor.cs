using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class ActivateInspactor : ActionBaseInspector
    {
        protected ActivateAction mAction
        {
            get
            {
                return this.mNode as ActivateAction;
            }
        }
        protected override void DrawFrom()
        {
            base.DrawFrom();
            Color color = GUI.color;
            GUI.color = Color.red;
            EditorGUILayout.LabelField("如果动作从值与动作到值相等，则不会有效果");
            GUI.color = color;

            this.mAction.From = EditorGUILayout.Toggle("动作从：", this.mAction.From);
        }

        protected override void DrawTo()
        {
            base.DrawTo();
            this.mAction.To = EditorGUILayout.Toggle("动作到：", this.mAction.To);
        }
    }
}