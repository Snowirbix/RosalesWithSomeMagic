using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class BehaviorTreeEditor : EditorWindow
{
    protected VisualTreeAsset visualTree;
    protected VisualElement uxmlElement;

    [MenuItem("Window/UIElements/BehaviorTreeEditor")]
    public static void ShowExample()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uxml");
        uxmlElement = visualTree.CloneTree();
        root.Add(uxmlElement);

        // Import USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        root.Q<Button>("CreateNode").RegisterCallback<MouseUpEvent>(x =>
        {
            NodeElement node = new NodeElement();
            root.Add(node);
        });
    }

    private void OnDisable()
    {
        AssetDatabase.CreateAsset(uxmlElement, "Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor_clone.uxml");
    }
}