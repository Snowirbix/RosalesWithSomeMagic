using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Better.StreamingAssets;
using System.Collections.Generic;

public class BehaviorTreeEditor : EditorWindow
{
    protected VisualTreeAsset visualTree;
    protected VisualElement uxmlElement;
    protected BehaviorTreeConfiguration btConfig;

    [MenuItem("Window/UIElements/BehaviorTreeEditor")]
    public static void ShowExample()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    public void OnEnable()
    {
        BetterStreamingAssets.Initialize();

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uxml");
        uxmlElement = visualTree.CloneTree();
        root.Add(uxmlElement);

        // Import USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        BehaviorTreeConfiguration config = Configuration.DeserializeFromFile<BehaviorTreeConfiguration>("/BehaviorTreeEditor_clone.xml");

        btConfig = new BehaviorTreeConfiguration();

        root.Q<Button>("CreateNode").RegisterCallback<MouseUpEvent>(x =>
        {
            NodeElement node = new NodeElement();
            root.Add(node);
            btConfig.nodes.Add(new BehaviorTreeConfiguration.NodeConfiguration(btConfig) { name = "root", position = new Vector2(10, 10), parentId = -1 });
        });
    }

    private void OnDisable()
    {
        Configuration.SerializeToFile(btConfig, Application.streamingAssetsPath +"/BehaviorTreeEditor_clone.xml");
    }
}