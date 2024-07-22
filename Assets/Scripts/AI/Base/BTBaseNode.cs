/// <summary>
/// 行为树结点基类
/// </summary>
public abstract class BTBaseNode
{
    /// <summary>
    /// 执行节点逻辑的抽象方法
    /// </summary>
    public abstract ENodeState Execute();
}
