using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public abstract class TweenActionInspector : ActionBaseInspector
    {
        protected TweenAction mTweenAction
        {
            get { return this.mNode as TweenAction;}
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            mTweenAction.Duration = EditorGUILayout.FloatField("持续时间:", mTweenAction.Duration);
            mTweenAction.Curve = EditorGUILayout.CurveField("动画曲线:", mTweenAction.Curve, GUILayout.Width(170f), GUILayout.Height(62f));
        }

        protected override void CheckNeedComponent()
        {
            if (this.mNode.Target!=null )
            {
                var tween = this.mTweenAction.Tween; //todo 这样获取组件的写法有瑕疵
            }
        }


    }
}