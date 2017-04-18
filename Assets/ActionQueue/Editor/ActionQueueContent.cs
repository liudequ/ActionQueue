using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionQueue
{
    /// <summary>
    /// 内容名称
    /// </summary>
    public static class ActionQueueContent
    {

        public static GUIContent makeTransition;
        public static GUIContent copy;
        public static GUIContent paste;
        public static GUIContent delete;
        public static GUIContent bindToGameObject;
        public static GUIContent moveToSubStateMachine;
        public static GUIContent moveToParentStateMachine;

        static ActionQueueContent()
        {
            makeTransition = new GUIContent("Make Transition");
            copy = new GUIContent("Copy");
            delete = new GUIContent("Delete");
            paste = new GUIContent("Paste");
            bindToGameObject = new GUIContent("Bind To Selection");
            moveToSubStateMachine = new GUIContent("Move To Sub-State Machine");
            moveToParentStateMachine = new GUIContent("Move To Parent-Sate Machine");
        }
    }

}
