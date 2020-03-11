using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BehaviorTreeConfiguration
{
    public int idx = 0;

    [Serializable]
    public class NodeConfiguration
    {
        [Obsolete("use nodes idx instead")]
        public int id;
        public int parentId;
        public Vector2 position;
        public string  name;
        
        public NodeConfiguration ()
        {
            //
        }
        public NodeConfiguration (BehaviorTreeConfiguration btConfig)
        {
            id = btConfig.idx++;
        }
    }

    public List<NodeConfiguration> nodes = new List<NodeConfiguration>();
}
