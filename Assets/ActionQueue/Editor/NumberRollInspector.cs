using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class NumberRollInspector : ActionBaseInspector
    {

        protected NumberRollAction mAction
        {
            get { return this.mNode as NumberRollAction;}
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.mAction.Type = (NumberRollAction.RollType)EditorGUILayout.EnumPopup("表现类型", this.mAction.Type);

            if (this.mAction.Type == NumberRollAction.RollType.Step)
            {
                this.mAction.Steps = EditorGUILayout.FloatField("每一步变化多少", this.mAction.Steps);
                this.mAction.StepSpeed = EditorGUILayout.IntField("每几帧执行一步", this.mAction.StepSpeed);
            }
            else if (this.mAction.Type == NumberRollAction.RollType.TotalTime)
            {
                this.mAction.TotalTime = EditorGUILayout.FloatField("总共播放几秒", this.mAction.TotalTime);
                this.mAction.IsIntChange = EditorGUILayout.Toggle("是否为整理变化", this.mAction.IsIntChange);
            }
        }

        protected override void CheckNeedComponent()
        {
            base.CheckNeedComponent();
            if (this.mNode.Target != null)
            {
                UILabel label = this.mNode.Target.GetComponent<UILabel>();
                if (label == null)
                {
                    Color color = GUI.color;
                    GUI.color = Color.red;
                    GUILayout.Label("当前目标上没有Label组件！");
                    GUI.color = color;
                }
            }
        }

        protected override void DrawFrom()
        {
            base.DrawFrom();
            this.mAction.From = EditorGUILayout.FloatField("动作从：", this.mAction.From);
        }

        protected override void DrawTo()
        {
            base.DrawTo();
            this.mAction.To = EditorGUILayout.FloatField("动作到：", this.mAction.To);
        }
    }
}