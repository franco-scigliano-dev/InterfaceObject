using System;
using NUnit.Compatibility;
using UnityEngine;
using UnityEditor;

namespace com.fscigliano.InterfaceObject.Editor
{
    /// <summary>
    /// Creation Date:   10/4/2020 2:35:18 PM
    /// Product Name:    Interface Object
    /// Developers:      Franco Scigliano
    /// Description:
    /// </summary>
    [CustomPropertyDrawer(typeof(InterfaceObjectBase), true)]
    public class InterfaceObjectBaseDrawer : UnityEditor.PropertyDrawer
    {
        #region Methods
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty dataProperty = property.FindPropertyRelative("_data");
            EditorGUI.BeginProperty(position, label, dataProperty);
            var o = ReflectionExtensions.Editor.ReflectionExtensions.GetTargetObjectOfProperty(property);
            InterfaceObjectBase instance = o as InterfaceObjectBase;
            if (instance == null) return;
            
            EditorGUI.BeginChangeCheck();
            dataProperty.objectReferenceValue =
                EditorGUI.ObjectField(position, label, dataProperty.objectReferenceValue, instance.FilterType, true);

            if (EditorGUI.EndChangeCheck())
            {
                instance.Changed = true;
                if (dataProperty.objectReferenceValue != null)
                {
                    if (dataProperty.objectReferenceValue is GameObject)
                    {
                        var c = (dataProperty.objectReferenceValue as GameObject)?.GetComponent(instance.FilterType);
                        if (c != null)
                        {
                            dataProperty.objectReferenceValue = c;
                        }
                        else
                        {
                            dataProperty.objectReferenceValue = null;
                        }
                    }
                    else
                    {
                        System.Type newType = dataProperty.objectReferenceValue.GetType();
                        if (!instance.FilterType.IsCastableFrom(newType))
                        {
                            dataProperty.objectReferenceValue = null;
                        }    
                    }
                }
                dataProperty.serializedObject.ApplyModifiedProperties();
            }

            if (HandleDragAndDrop(position, dataProperty, instance.FilterType))
            {
                instance.Changed = true;
            }
            EditorGUI.EndProperty();
        }
        private bool HandleDragAndDrop(Rect r, SerializedProperty property, Type t)
        {
            if (!r.Contains(Event.current.mousePosition))
            {
                return false;
            }

            bool result = false;
            
            UnityEngine.Object firstComp = null;
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    DragAndDrop.PrepareStartDrag();
                    break;
 
                case EventType.MouseDrag:
                    DragAndDrop.StartDrag("drag-test");
                    break;
 
                case EventType.DragUpdated:
                    firstComp = GetComponentFromDragged(DragAndDrop.objectReferences, t);
                    if (firstComp!= null)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    }
                    break;
 
                case EventType.Repaint:
                    firstComp = GetComponentFromDragged(DragAndDrop.objectReferences, t);
                    if (firstComp!= null)
                    {
                        if (DragAndDrop.visualMode == DragAndDropVisualMode.None ||
                            DragAndDrop.visualMode == DragAndDropVisualMode.Rejected)
                            break;

                        EditorGUI.DrawRect(r, new Color(0.5f, 0.5f, 0.7f, 0.3f));
                    }
                    break;
                case EventType.DragPerform:
                    firstComp = GetComponentFromDragged(DragAndDrop.objectReferences, t);
                    if (firstComp!= null)
                    {
                        property.objectReferenceValue = firstComp;
                        DragAndDrop.AcceptDrag();
                        result = true;
                    }
                    break;
            }
            property.serializedObject.ApplyModifiedProperties();
            return result;
        }

        private UnityEngine.Object GetComponentFromDragged(UnityEngine.Object[] objectReferences, Type t)
        {
            for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
            {
                var o = DragAndDrop.objectReferences[i];
                if (o is GameObject go)
                {
                    var c = go.GetComponent(t);
                    if (c != null)
                    {
                        return c;
                    }
                }
            }
            return null;
        }
 
        #endregion
    }
}