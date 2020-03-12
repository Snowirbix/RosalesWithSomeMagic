using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Better.StreamingAssets;
using System.Collections.Generic;

public class BehaviorTreeEditor : EditorWindow
{
    [Obsolete("use a standard visualElement as the root for visualNodes")]
    protected VisualNode rootNode;
    protected VisualNodeTree visualNodeTree;
    protected int idx;

    [Serializable]
    public class VisualNodeTree
    {
        public VisualNodeTree()
        {
            nodesData = new List<VisualNode.Data>();
        }

        public VisualNodeTree(VisualNode node)
        {
            nodesData = new List<VisualNode.Data>();
            Add(node);
        }

        private void Add (VisualNode node)
        {
            nodesData.Add(node.Serialize());

            foreach (VisualNode child in node.Children())
            {
                Add(child);
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
        rootNode = new VisualNode(0);
        rootVisualElement.Add(rootNode);

        // Import VisualTree xml
        try
        {
            VisualNodeTree config = Configuration.DeserializeFromFile<VisualNodeTree>("/BehaviorTreeEditor_clone.xml");
            foreach (VisualNode.Data data in config.nodesData)
            {
                rootNode.Add(new VisualNode(data));
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        // Create new node on click
        rootVisualElement.Q<Button>("CreateNode").RegisterCallback<MouseUpEvent>(x =>
        {
            rootNode.Add(new VisualNode(idx++));
        });
    }

    private void OnDisable()
    {
        visualNodeTree = new VisualNodeTree(rootNode);
        Configuration.SerializeToFile(visualNodeTree, Application.streamingAssetsPath +"/BehaviorTreeEditor_clone.xml");
    }
}