using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Better.StreamingAssets;
using System.Collections.Generic;
using System.Linq;

public class BehaviorTreeEditor : EditorWindow
{
    public VectorImage image;

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
            
            // What's happening here
            foreach (VisualElement child in root.Children())
            {
                VisualNode vn = child.Q<VisualNode>();
                if (vn != null)
                    nodesData.Add(vn.Serialize());
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

        // Import window UXML
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uxml");
        VisualElement visualTreeClone = visualTree.CloneTree();
        rootVisualElement.Add(visualTreeClone);

        // Import global USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviorTree/Editor/BehaviorTreeEditor.uss");
        rootVisualElement.styleSheets.Add(styleSheet);
        
        // Create root node
        rootVisualNode = new VisualElement();
        rootVisualNode.name = "Root Visual Node";
        rootVisualElement.Add(rootVisualNode);

        // Import node UXML and USS
        VisualTreeAsset visualNodeTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BehaviorTree/Editor/VisualNode.uxml");
        var nodeStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviorTree/Editor/VisualNode.uss");

        // Set link image
        NodeLinkImage.image = image;

        // Import VisualTree xml
        try
        {
            VisualNodeTree config = Configuration.DeserializeFromFile<VisualNodeTree>("/BehaviorTreeEditor_clone.xml");
            VisualNode.UxmlFactory nodeFactory = new VisualNode.UxmlFactory();
            foreach (VisualNode.Data data in config.nodesData)
            {
                VisualElement visualNodeClone = visualNodeTree.CloneTree();
                visualNodeClone.styleSheets.Add(nodeStyleSheet);
                rootVisualNode.Add(visualNodeClone);
        
                //rootVisualNode.Add(new VisualNode(data));
            }
        }
        catch (System.IO.FileNotFoundException e)
        {
            Debug.Log(e);
        }

        // Create new node on click
        rootVisualElement.Q<Button>("CreateNode").RegisterCallback<MouseUpEvent>(x =>
        {
            VisualElement visualNodeClone = visualNodeTree.CloneTree();
            visualNodeClone.styleSheets.Add(nodeStyleSheet);
            rootVisualNode.Add(visualNodeClone);
        });
    }

    private void OnDisable()
    {
        visualNodeTree = new VisualNodeTree(rootVisualNode);
        Configuration.SerializeToFile(visualNodeTree, Application.streamingAssetsPath +"/BehaviorTreeEditor_clone.xml");
    }
}