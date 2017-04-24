using System.Collections;
using System.Collections.Generic;
using ActionQueue;
using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class ActionQueueEditor : NodeEditor
    {

        #region 属性

        public static ActionQueueEditor instance;
        private List<ActionBase> Nodes
        {
            get
            {
                if (ActionQueueEditor.Active == null)
                {
                    return new List<ActionBase>();
                }
                if (ActionQueueEditor.Active.Actions == null || ActionQueueEditor.Active.Actions.Count == 0)
                {
                    ActionQueueEditorUtility.AddNode<StartAction>(
                        new Vector2(NodeEditor.Center.x, NodeEditor.Center.y), ActionBase.StartID);
                    ActionQueueEditorUtility.AddNode<EndAction>(
                        new Vector2(NodeEditor.Center.x + 700, NodeEditor.Center.y), ActionBase.EndId);
                }
                return ActionQueueEditor.Active.Actions;
            }
        }
        [SerializeField]
        private List<ActionBase> selection = new List<ActionBase>();
        public static int SelectionCount
        {
            get
            {
                if (ActionQueueEditor.instance != null)
                {
                    return ActionQueueEditor.instance.selection.Count;
                }
                return 0;
            }
        }

        public static List<ActionBase> SelectedNodes
        {
            get
            {
                if (ActionQueueEditor.instance != null)
                {
                    return ActionQueueEditor.instance.selection;
                }
                return null;
            }
        }


        [SerializeField]
        private ActionExecutor active;
        public static ActionExecutor Active
        {
            get
            {
                if (ActionQueueEditor.instance == null)
                {
                    return null;
                }
                return ActionQueueEditor.instance.active;
            }
            set
            {
                if (ActionQueueEditor.instance == null) return;
                ActionQueueEditor.instance.active = value;
            }
        }

        private bool mCenterView;
        private ActionBase mFromNode;
        private ActionsInspector mActionsInspector;
        private Rect mInspectorRect;

        #endregion

        [MenuItem("Magiccube/Tools/ActionQueue")]
        public static void OpenTestNode()
        {
            var window = EditorWindow.GetWindow<ActionQueueEditor>("ActionQueue");
            instance = window;
            ActionQueueEditor.CanvasOffest = new Rect(0, 40, -300, -40);
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            instance = this;
            mCenterView = true;
            if (this.mActionsInspector == null)
                this.mActionsInspector = new ActionsInspector();

        }


        private void Update()
        {
            if (ActionQueueEditor.Active != null)
            {
                if (EditorApplication.isPlaying)
                {
                    ActionQueueEditor.instance.Repaint();
                }
            }
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("居中"))
            {
                this.CenterView();
            }
            GUILayout.EndHorizontal();

            Begin();
            if (ActionQueueEditor.Active != null)
            {
                this.DoNodes();
            }
            End();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Action Queue : ");
            Active = EditorGUILayout.ObjectField(Active, typeof(ActionExecutor), true) as ActionExecutor;
            EditorGUILayout.EndHorizontal();

            this.mInspectorRect = new Rect(this.scaledCanvasSize.width, this.scaledCanvasSize.y,
                -ActionQueueEditor.CanvasOffest.width, this.scaledCanvasSize.height);
            GUILayout.BeginArea(this.mInspectorRect);
            this.mActionsInspector.OnInspectorGUI();
            GUILayout.EndArea();

            if (mCenterView)
            {
                CenterView();
                mCenterView = false;
            }
        }

        protected override void CanvasContextMenu()
        {
            if (currentEvent.type != EventType.MouseDown || currentEvent.button != 1 || currentEvent.clickCount != 1 || Active == null)
            {
                return;
            }

            GenericMenu canvasMenu = new GenericMenu();

            //todo 通过反射实现实例的创建
            canvasMenu.AddItem(new GUIContent("Create Action/Tween/Alpha"), false, delegate ()
            {
                ActionQueueEditorUtility.AddNode<TweenAlphaAction>(this.mousePosition,
                    ActionQueueEditor.Active.GetNextUniqueID());
            });

            canvasMenu.AddItem(new GUIContent("Create Action/Tween/Position"), false, delegate ()
            {
                ActionQueueEditorUtility.AddNode<TweenPositionAction>(this.mousePosition,
                    ActionQueueEditor.Active.GetNextUniqueID());
            });

            canvasMenu.AddItem(new GUIContent("Create Action/Tween/Scale"), false, delegate ()
            {
                ActionQueueEditorUtility.AddNode<TweenScaleAction>(this.mousePosition,
                    ActionQueueEditor.Active.GetNextUniqueID());
            });

            canvasMenu.AddItem(new GUIContent("Create Action/NumberRoll"), false, delegate ()
            {
                ActionQueueEditorUtility.AddNode<NumberRollAction>(this.mousePosition,
                    ActionQueueEditor.Active.GetNextUniqueID());
            });

            canvasMenu.AddItem(new GUIContent("Create Action/Delay"), false, delegate ()
            {
                ActionQueueEditorUtility.AddNode<DelayAction>(this.mousePosition,
                    ActionQueueEditor.Active.GetNextUniqueID());
            });

            canvasMenu.AddItem(new GUIContent("Create Action/Activate"), false, delegate ()
            {
                ActionQueueEditorUtility.AddNode<ActivateAction>(this.mousePosition,
                    ActionQueueEditor.Active.GetNextUniqueID());
            });

            canvasMenu.AddItem(new GUIContent("Create Action/Trigger Actions"), false, delegate ()
            {
                ActionQueueEditorUtility.AddNode<IntervalTriggerAction>(this.mousePosition,
                    ActionQueueEditor.Active.GetNextUniqueID());
            });

            canvasMenu.ShowAsContext();
        }



        #region 绘制节点和关系

        private void DoNodes()
        {
            DoTransitions();
            for (int i = 0; i < Nodes.Count; i++)
            {
                ActionBase node = Nodes[i];
                if (!selection.Contains(node))
                {
                    DoNode(node);
                }
            }

            for (int i = 0; i < selection.Count; i++)
            {
                ActionBase node = selection[i];
                DoNode(node);
            }
            if (!this.isTouchInspaceArea())
            {
                DoNodeEvents();
                NodeContextMenu();
            }
        }


        /// <summary>
        /// 绘制节点
        /// </summary>
        /// <param name="node"></param>
        private void DoNode(ActionBase node)
        {
            GUIStyle style = ActionQueueEditorStyles.GetNodeStyle(node.color, selection.Contains(node), false);
            GUI.Box(node.position, node.Name, style);
        }

        private void DoTransitions()
        {
            if (mFromNode != null)
            {
                DrawConnection(mFromNode.position.center, mousePosition, Color.green, 1, false);
                Repaint();
            }
            for (int i = 0; i < Nodes.Count; i++)
            {
                ActionBase node = Nodes[i];
                DoTransition(node);
            }
        }


        private void DoTransition(ActionBase node)
        {
            if (node.NextIDs == null || node.NextIDs.Count == 0) return;
            foreach (var nextID in node.NextIDs)
            {
                ActionBase nextAction = Active.GetActionByID(nextID);
                if (nextAction == null) continue;
                int arrowCount = 1;
                bool offset = true;
                Color color = Color.cyan;
                DrawConnection(node.position.center, nextAction.position.center, color, arrowCount, offset);
            }

        }

        private void AddTransition(ActionBase fromNode, ActionBase toNode)
        {
            if (fromNode == null || toNode == null)
            {
                return;
            }
            fromNode.AddNextID(toNode.ID);
            toNode.AddPrevID(fromNode.ID);

            //            ActionBaseInspector.SetDirty(mFromNode);
            //            ActionBaseInspector.SetDirty(toNode);
        }

        #endregion

        #region 与节点交互


        /// <summary>
        /// 在节点上点击右键
        /// </summary>
        private void NodeContextMenu()
        {
            if (currentEvent.type != EventType.MouseDown || currentEvent.button != 1 || currentEvent.clickCount != 1)
            {
                return;
            }

            ActionBase node = MouseOverNode();
            if (node == null)
            {
                return;
            }
            GenericMenu nodeMenu = new GenericMenu();


            nodeMenu.AddItem(ActionQueueContent.makeTransition, false, delegate ()
            {
                if (node.ID != ActionBase.EndId)
                    mFromNode = node;
            });



            nodeMenu.AddItem(ActionQueueContent.delete, false, delegate ()
            {
                if (node.ID == ActionBase.EndId || node.ID == ActionBase.StartID) return;
                ActionQueueEditorUtility.DeleteNode(node);
                EditorUtility.SetDirty(ActionQueueEditor.Active);
            });

            nodeMenu.ShowAsContext();
            Event.current.Use();
        }




        private void DoNodeEvents()
        {
            if (currentEvent.button != 0)
            {
                return;
            }
            SelectNodes();
            DragNodes();
        }

        /// <summary>
        /// 选择节点的操作
        /// </summary>
        private void SelectNodes()
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            switch (currentEvent.rawType)
            {
                case EventType.MouseDown:
                    if (!this.isTouchInspaceArea())
                    {
                        GUIUtility.hotControl = controlID;
                        ActionBase node = MouseOverNode();
                        if (node != null)
                        {
                            if (mFromNode != null)
                            {
                                AddTransition(mFromNode, node);
                                mFromNode = null;
                                GUIUtility.hotControl = 0;
                                GUIUtility.keyboardControl = 0;
                                return;
                            }

                            if (!this.selection.Contains(node))
                            {
                                this.selection.Clear();
                                this.selection.Add(node);
                            }

                            GUIUtility.hotControl = 0;
                            GUIUtility.keyboardControl = 0;
                            UpdateUnitySelection();
                            return;
                        }
                        mFromNode = null;
                        if (!EditorGUI.actionKey)
                        {
                            this.selection.Clear();
                            UpdateUnitySelection();
                        }
                    }

                    currentEvent.Use();
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID && !EditorGUI.actionKey && !currentEvent.shift)
                    {
                        currentEvent.Use();
                    }
                    break;
            }
        }

        /// <summary>
        /// 拖动节点
        /// </summary>
        private void DragNodes()
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (currentEvent.rawType)
            {
                case EventType.MouseDown:
                    GUIUtility.hotControl = controlID;
                    currentEvent.Use();
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        for (int i = 0; i < selection.Count; i++)
                        {
                            ActionBase node = selection[i];
                            node.position.position += currentEvent.delta;
                        }
                        currentEvent.Use();
                    }
                    break;
                case EventType.Repaint:
                    if (GUIUtility.hotControl == controlID)
                    {
                        AutoPanNodes(1.5f);
                    }
                    break;
            }
        }


        #endregion


        private void AutoPanNodes(float speed)
        {
            Vector2 delta = Vector2.zero;
            if (mousePosition.x > scaledCanvasSize.width + scrollPosition.x - 50f)
            {
                delta.x += speed;
            }

            if ((mousePosition.x < scrollPosition.x + 50f) && scrollPosition.x > 0f)
            {
                delta.x -= speed;
            }

            if (mousePosition.y > scaledCanvasSize.height + scrollPosition.y - 50f)
            {
                delta.y += speed;
            }

            if ((mousePosition.y < scrollPosition.y + 50f) && scrollPosition.y > 0f)
            {
                delta.y -= speed;
            }

            if (delta != Vector2.zero)
            {

                for (int i = 0; i < selection.Count; i++)
                {
                    ActionBase node = selection[i];
                    node.position.position += delta;
                }
                UpdateScrollPosition(scrollPosition + delta);
                Repaint();
            }
        }



        public void CenterView()
        {
            Vector2 center = Vector2.zero;
            if (Nodes.Count > 0)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    ActionBase node = Nodes[i];
                    center += new Vector2(node.position.center.x - scaledCanvasSize.width * 0.5f, node.position.center.y - scaledCanvasSize.height * 0.5f);
                }
                center /= Nodes.Count;
            }
            else
            {
                center = NodeEditor.Center;
            }
            UpdateScrollPosition(center);
            Repaint();
        }

        private void UpdateUnitySelection()
        {
            //            Selection.objects = selection.ToArray();
            //            if(selection.Count>0)
            //                ActionsInspector.SelectEditorAction = selection[0];
        }

        private ActionBase MouseOverNode()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                ActionBase node = Nodes[i];
                if (node.position.Contains(mousePosition))
                {
                    return node;
                }
            }
            return null;
        }


        private bool isTouchInspaceArea()
        {
            var pos = new Vector2();
            pos.x = this.mousePosition.x - worldViewRect.x;
            pos.y = this.mousePosition.y - worldViewRect.y + CanvasOffest.y * 2;
            //            Debug.Log(pos);
            //            Debug.Log(this.mInspectorRect);
            return this.mInspectorRect.Contains(pos);
        }

    }


}
