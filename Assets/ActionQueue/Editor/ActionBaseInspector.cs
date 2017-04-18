using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class ActionBaseInspector 
    {

        protected ActionBase mNode;

        public void SetNode(ActionBase node)
        {
            this.mNode = node;
        }

        public virtual void OnInspectorGUI()
        {
            int id = EditorGUILayout.IntField("ID",mNode.ID);
            if (id != mNode.ID)
            {
                if(EditorUtility.DisplayDialog("修改行为ID","轻易不要修改ID,因为该初始会自动生成，且和程序代码关联。","我任性","我错了"))
                    mNode.SetID(id);
            }
            GUILayout.Space(20);
            if (!ActionBase.GetIsStartOrEnd(this.mNode.ID))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("行为目标");
                this.mNode.Target = EditorGUILayout.ObjectField(this.mNode.Target, typeof(GameObject), true) as GameObject;
                EditorGUILayout.EndHorizontal();    
            }
            this.CheckNeedComponent();

            this.DrawFrom();
            this.DrawTo();
            
            this.DrawPrevActions();
//            this.DrawNextActions();

            GUILayout.Space(20);

        }

        public static void SetDirty(Object target)
        {
            EditorUtility.SetDirty(target);
            if (ActionQueueEditor.instance != null)
                ActionQueueEditor.instance.Repaint();
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 绘制是否添加所需的组件
        /// </summary>
        protected virtual void CheckNeedComponent()
        {
            
        }

        protected virtual void DrawFrom()
        {
            
        }

        protected virtual void DrawTo()
        {
            
        }

        /// <summary>
        /// 前置行为
        /// </summary>
        protected virtual void DrawPrevActions()
        {
            EditorGUILayout.LabelField("前置行为：");
            if (this.mNode.PrevIDs != null && this.mNode.PrevIDs.Count > 0)
            {
                this.DrawRelevancyActions(this.mNode.PrevIDs,true);
            }
        }

        /// <summary>
        /// 后置行为
        /// </summary>
        protected virtual void DrawNextActions()
        {
            EditorGUILayout.LabelField("后置行为：");
            if (this.mNode.NextIDs != null && this.mNode.NextIDs.Count > 0)
            {
                this.DrawRelevancyActions(this.mNode.NextIDs,false);
            }
        }

        /// <summary>
        /// 绘制关联行为
        /// </summary>
        protected virtual void DrawRelevancyActions(List<int> actions,bool isPrev)
        {
            int DelID = int.MinValue;
            for (int index = 0; index < actions.Count; index++)
            {
                int actionID = actions[index];
                if (ActionQueueEditor.instance != null && ActionQueueEditor.Active!=null)
                {
                    EditorGUILayout.BeginHorizontal();
                    var action = ActionQueueEditor.Active.GetActionByID(actionID);
                    EditorGUILayout.LabelField(action.Name);
                    if (GUILayout.Button("删除"))
                    {
                        DelID = action.ID;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (DelID != int.MinValue)
            {
                var action = ActionQueueEditor.Active.GetActionByID(DelID);
                if (isPrev)
                {
                    this.mNode.RemovePrevID(DelID);
                    action.RemoveNextID(this.mNode.ID);
                }
                else
                {
                    this.mNode.RemoveNextID(DelID);
                    action.RemovePrevID(this.mNode.ID);
                }
            }
        }


        public void OnHeaderGUI()
        {
            mNode.Name = EditorGUILayout.TextField("Name", this.mNode.Name);
            EditorGUILayout.LabelField("类型: ",this.mNode.GetType().Name);
        }
    }
}