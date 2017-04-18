using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ActionQueue
{
    public class ActionQueueEditorStyles
    {
        public const float StateWidth = 150f;
        public const float StateHeight = 30f;

        public static GUIStyle canvasBackground;
        public static GUIStyle selectionRect;
        public static GUIStyle elementBackground;
        public static GUIStyle breadcrumbLeft;
        public static GUIStyle breadcrumbMiddle;
        public static GUIStyle wrappedLabel;
        public static GUIStyle wrappedLabelLeft;
        public static GUIStyle variableHeader;
        public static GUIStyle label;
        public static GUIStyle centeredLabel;
        public static GUIStyle inspectorTitle;
        public static GUIStyle inspectorTitleText;
        public static GUIStyle stateLabelGizmo;
        public static GUIStyle instructionLabel;
        public static GUIStyle shortcutLabel;
        public static GUIStyle browserPopup;

        public static Texture2D popupIcon;
        public static Texture2D helpIcon;
        public static Texture2D errorIcon;
        public static Texture2D warnIcon;
        public static Texture2D infoIcon;
        public static Texture2D toolbarPlus;
        public static Texture2D toolbarMinus;
        public static Texture2D iCodeLogo;

        public static Color gridMinorColor;
        public static Color gridMajorColor;

        public static int fsmColor;
        public static int startNodeColor;
        public static int anyStateColor;
        public static int defaultNodeColor;

        static ActionQueueEditorStyles()
        {
            ActionQueueEditorStyles.nodeStyleCache = new Dictionary<string, GUIStyle>();
            ActionQueueEditorStyles.gridMinorColor = EditorGUIUtility.isProSkin ? new Color(0f, 0f, 0f, 0.18f) : new Color(0f, 0f, 0f, 0.1f);
            ActionQueueEditorStyles.gridMajorColor = EditorGUIUtility.isProSkin ? new Color(0f, 0f, 0f, 0.28f) : new Color(0f, 0f, 0f, 0.15f);

            ActionQueueEditorStyles.popupIcon = EditorGUIUtility.FindTexture("_popup");
            ActionQueueEditorStyles.helpIcon = EditorGUIUtility.FindTexture("_help");
            ActionQueueEditorStyles.errorIcon = EditorGUIUtility.FindTexture("d_console.erroricon.sml");
            ActionQueueEditorStyles.warnIcon = EditorGUIUtility.FindTexture("console.warnicon");
            ActionQueueEditorStyles.infoIcon = EditorGUIUtility.FindTexture("console.infoicon");
            ActionQueueEditorStyles.toolbarPlus = EditorGUIUtility.FindTexture("Toolbar Plus");
            ActionQueueEditorStyles.toolbarMinus = EditorGUIUtility.FindTexture("Toolbar Minus");

            ActionQueueEditorStyles.canvasBackground = "flow background";
            ActionQueueEditorStyles.selectionRect = "SelectionRect";
            ActionQueueEditorStyles.elementBackground = new GUIStyle("PopupCurveSwatchBackground")
            {
                padding = new RectOffset()
            };
            ActionQueueEditorStyles.breadcrumbLeft = "GUIEditor.BreadcrumbLeft";
            ActionQueueEditorStyles.breadcrumbMiddle = "GUIEditor.BreadcrumbMid";
            ActionQueueEditorStyles.wrappedLabel = new GUIStyle("label")
            {
                fixedHeight = 0,
                wordWrap = true
            };
            ActionQueueEditorStyles.wrappedLabelLeft = new GUIStyle("label")
            {
                fixedHeight = 0,
                wordWrap = true,
                alignment = TextAnchor.UpperLeft
            };
            ActionQueueEditorStyles.variableHeader = "flow overlay header lower left";
            ActionQueueEditorStyles.label = "label";
            ActionQueueEditorStyles.inspectorTitle = "IN Title";
            ActionQueueEditorStyles.inspectorTitleText = "IN TitleText";
            ActionQueueEditorStyles.iCodeLogo = (Texture2D)Resources.Load("ICodeLogo");
            ActionQueueEditorStyles.stateLabelGizmo = new GUIStyle("HelpBox")
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = 21
            };
            ActionQueueEditorStyles.centeredLabel = new GUIStyle("Label")
            {
                alignment = TextAnchor.UpperCenter,
            };
            ActionQueueEditorStyles.instructionLabel = new GUIStyle("TL Selection H2")
            {
                padding = new RectOffset(3, 3, 3, 3),
                contentOffset = ActionQueueEditorStyles.wrappedLabel.contentOffset,
                alignment = TextAnchor.UpperLeft,
                fixedHeight = 0,
                wordWrap = true
            };
            ActionQueueEditorStyles.shortcutLabel = new GUIStyle("ObjectPickerLargeStatus")
            {
                padding = new RectOffset(3, 3, 3, 3),
                alignment = TextAnchor.UpperLeft
            };
            ActionQueueEditorStyles.browserPopup = new GUIStyle("label")
            {
                contentOffset = new Vector2(0, 2)
            };

            ActionQueueEditorStyles.fsmColor = (int)NodeColor.Blue;
            ActionQueueEditorStyles.startNodeColor = (int)NodeColor.Orange;
            ActionQueueEditorStyles.anyStateColor = (int)NodeColor.Aqua;
            ActionQueueEditorStyles.defaultNodeColor = (int)NodeColor.Grey;
        }

        private static Dictionary<string, GUIStyle> nodeStyleCache;

        private static string[] styleCache =
        {
            "flow node 0",
            "flow node 1",
            "flow node 2",
            "flow node 3",
            "flow node 4",
            "flow node 5",
            "flow node 6"
        };

        private static string[] styleCacheHex =
        {
            "flow node hex 0",
            "flow node hex 1",
            "flow node hex 2",
            "flow node hex 3",
            "flow node hex 4",
            "flow node hex 5",
            "flow node hex 6"
        };


        /// <summary>
        /// 获得节点的风格
        /// </summary>
        /// <param name="color"></param>
        /// <param name="on"></param>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static GUIStyle GetNodeStyle(int color, bool on, bool hex)
        {
            return GetNodeStyle(hex ? styleCacheHex[color] : styleCache[color], on, hex ? 8f : 2f);
        }

        /// <summary>
        /// 获得节点的风格
        /// </summary>
        /// <param name="styleName"></param>
        /// <param name="on"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static GUIStyle GetNodeStyle(string styleName, bool on, float offset)
        {
            string str = on ? string.Concat(styleName, " on") : styleName;
            if (!ActionQueueEditorStyles.nodeStyleCache.ContainsKey(str))
            {
                GUIStyle style = new GUIStyle(str);
                style.contentOffset = new Vector2(0, style.contentOffset.y - offset);
                if (on)
                {
                    style.fontStyle = FontStyle.Bold;
                }
                nodeStyleCache[str] = style;
            }
            return nodeStyleCache[str];
        }

        public enum NodeColor
        {
            Grey = 0,
            Blue = 1,
            Aqua = 2,
            Green = 3,
            Yellow = 4,
            Orange = 5,
            Red = 6
        }


    }
}