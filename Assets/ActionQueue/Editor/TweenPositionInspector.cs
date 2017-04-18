using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class TweenPositionInspector :TweenActionInspector
    {
        protected TweenPositionAction action
        {
            get
            {
                return this.mNode as TweenPositionAction;
            }
        }
        protected override void DrawFrom()
        {
            if (this.action.From == null)
            {
                this.action.From = Vector3.zero;
            }
            this.action.From = EditorGUILayout.Vector3Field("动作从：", (Vector3)this.action.From);
        }

        protected override void DrawTo()
        {
            if (this.action.To == null)
                this.action.To = Vector3.zero;
            this.action.To = EditorGUILayout.Vector3Field("动作到:", (Vector3)this.action.To);
        }
    }
}