using System.Collections.Generic;

public abstract class BTControlNode : BTBaseNode
{
    // 存储子节点的 List
    protected List<BTBaseNode> _childList = new List<BTBaseNode>();

    protected int _currentIndex = 0; // 当前执行到的子节点索引

    /// <summary>
    /// 添加子节点
    /// </summary>
    public virtual void AddChild(params BTBaseNode[] node) {
        _childList.AddRange(node);
    }
}