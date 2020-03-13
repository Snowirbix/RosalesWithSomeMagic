using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Better.StreamingAssets;
using System.Collections.Generic;

public class BehaviorTreeEditor : EditorWindow
{
    protected VisualElement rootVisualNode;
    protected VisualNodeTree visualNodeTree;
    protected int idx;

    [Serializable]
    public class VisualNodeTree
    {
        public VisualNodeTree()
        {
            nodesData = new List<VisualNode.Data>();
        }

        public VisualNodeTree(VisualElement root)
        {
            nodesData = new List<VisualNode.Data>();
            
            foreach (VisualNode child in root.Children())
            {
                nodesData.Add(child.Serialize());
            }
        }

        public List<VisualNode.Data> nodesData;
    }

    [MenuItem("Window/UIElements/BehaviorTreeEditor")]
    public static void ShowExample()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    public void OnEnable()
    {
        // Init
        idx = 1;
        BetterStreamingAssets.Initialize();

        // Import UXML
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uxml");
        VisualElement visualTreeClone = visualTree.CloneTree();
        rootVisualElement.Add(visualTreeClone);

        // Import USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uss");
        rootVisualElement.styleSheets.Add(styleSheet);
        
        // Create root node
        rootVisualNode = new VisualElement();
        rootVisualElement.Add(rootVisualNode);

        // Import VisualTree xml
        try
        {
            VisualNodeTree config = Configuration.DeserializeFromFile<VisualNodeTree>("/BehaviorTreeEditor_clone.xml");
            foreach (VisualNode.Data data in config.nodesData)
            {
                rootVisualNode.Add(new VisualNode(data));
            }
        }
        catch (System.IO.FileNotFoundException e)
        {
            Debug.Log(e);
        }

        // Create new node on click
        rootVisualElement.Q<Button>("CreateNode").RegisterCallback<MouseUpEvent>(x =>
        {
            rootVisualNode.Add(new VisualNode(idx++));
        });
    }

    private void OnDisable()
    {
        visualNodeTree = new VisualNodeTree(rootVisualNode);
        Configuration.SerializeToFile(visualNodeTree, Application.streamingAssetsPath +"/BehaviorTreeEditor_clone.xml");
    }
}