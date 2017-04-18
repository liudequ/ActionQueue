using UnityEditor;

namespace ActionQueue
{
    public class DelayInspector : ActionBaseInspector
    {
        protected DelayAction mAction
        {
            get { return this.mNode as DelayAction; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            this.mAction.DelayTime = EditorGUILayout.FloatField("持续时间：", this.mAction.DelayTime);
        }
    }
}