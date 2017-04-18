using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class TweenAlphaInspector : TweenActionInspector
    {
        protected TweenAlphaAction action
        {
            get
            {
                return this.mNode as TweenAlphaAction;
            }
        }

        protected override void DrawFrom()
        {
            if (this.action.From == null)
            {
                this.action.From = 0.0f;
            }
            this.action.From = EditorGUILayout.FloatField("动作从：", (float)this.action.From);
        }

        protected override void DrawTo()
        {
            if (this.action.To == null)
                this.action.To = 0.0f;
            this.action.To = EditorGUILayout.FloatField("动作到:", (float)this.action.To);
        }
    }
}