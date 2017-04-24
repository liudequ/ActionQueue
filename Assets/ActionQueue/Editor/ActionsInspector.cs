using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ActionQueue
{
    public class ActionsInspector
    {



        private Dictionary<int, ActionBaseInspector> map = new Dictionary<int, ActionBaseInspector>();

        public void OnInspectorGUI()
        {

            if (ActionQueueEditor.instance != null && ActionQueueEditor.SelectedNodes != null && ActionQueueEditor.SelectedNodes.Count > 0)
            {
                var selectAction = ActionQueueEditor.SelectedNodes[0];
                if (selectAction != null)
                {
                    ActionBaseInspector inspector = null;
                    if (map.ContainsKey(selectAction.ID))
                    {
                        inspector = map[selectAction.ID];
                    }
                    else
                    {
                        inspector = GetMatchInspector(selectAction);
                        inspector.SetNode(selectAction);
                    }

                    inspector.OnHeaderGUI();
                    inspector.OnInspectorGUI();
                }
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
                return new NumberRollInspector();
            }
            else if (action is DelayAction)
            {
                return new DelayInspector();
            }
            else if (action is IntervalTriggerAction)
            {
                return new IntervalTriggerInspector();
            }
            else if (action is ActivateAction)
            {
                return new ActivateInspactor();
            }
            else if (action is TweenPositionAction)
            {
                return new TweenPositionInspector();
            }
            return new ActionBaseInspector();
        }
    }
}

