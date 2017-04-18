using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ArrayUtility = UnityEditor.ArrayUtility;

namespace ActionQueue
{
    public class ActionQueueEditorUtility
    {

        public static void DestroyImmediate(ScriptableObject obj)
        {
            if (obj == null)
            {
                return;
            }
            UnityEngine.Object.DestroyImmediate(obj, true);
            AssetDatabase.SaveAssets();
        }

        public static void DeleteNode(ActionBase node)
        {
//            ActionQueueEditorUtility.DestroyImmediate(node);
            ActionQueueEditor.Active.Actions.Remove(node);
            foreach (ActionBase action in ActionQueueEditor.Active.Actions)
            {
                action.RemoveNextID(node.ID);
                action.RemovePrevID(node.ID);
            }
        }


        public static T AddNode<T>(Vector2 position,int ID) where  T: ActionBase
        {
//            ActionBase node = (ActionBase)ScriptableObject.CreateInstance(typeof(T));
            Type type = typeof(T);
            T node = type.Assembly.CreateInstance(type.FullName) as T;
            node.position = new Rect(position.x, position.y, ActionQueueEditorStyles.StateWidth,
                ActionQueueEditorStyles.StateHeight);
            node.Name = GenerateUniqueNodeName<T>(ID);
            node.SetID(ID);
            node.color = 0;
            if (ActionQueueEditor.Active == null)
            {
                ActionBase.LogError("Action == null can't create instance : " + typeof(T));
                return null;
            }
            if (ActionQueueEditor.Active.Actions == null)
                ActionQueueEditor.Active.Actions = new List<ActionBase>();
            ActionQueueEditor.Active.Actions.Add(node);
            AssetDatabase.SaveAssets();
            return (T)(object)node;
        }
    

        public static EventType ReserveEvent(params Rect[] areas)
        {
            EventType eventType = Event.current.type;
            foreach (Rect area in areas)
            {
                if ((area.Contains(Event.current.mousePosition) && (eventType == EventType.MouseDown || eventType == EventType.ScrollWheel)))
                {
                    Event.current.type = EventType.Ignore;
                }
            }
            return eventType;
        }

        public static void ReleaseEvent(EventType type)
        {
            if (Event.current.type != EventType.Used)
            {
                Event.current.type = type;
            }
        }


        public static string GenerateUniqueNodeName<T>(int ID)
        {
            Type type = typeof(T);
            return type.Name + ID;
        }

        public static List<T> FindInScene<T>() where T : Component
        {
            T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

            List<T> list = new List<T>();

            foreach (T comp in comps)
            {
                if (comp.gameObject.hideFlags == 0)
                {
                    string path = AssetDatabase.GetAssetPath(comp.gameObject);
                    if (string.IsNullOrEmpty(path)) list.Add(comp);
                }
            }
            return list;
        }

    }
}