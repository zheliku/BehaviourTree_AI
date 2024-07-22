using System;

/// <summary>
/// 条件节点，评估一个条件，并返回成功 / 失败
/// </summary>
public class BTConditionNode : BTBaseNode
{
    private Func<bool> _action; // 返回值表示执行是否成功

    public BTConditionNode(Func<bool> action) { _action = action; }

    public override ENodeState Execute() {
        if (_action == null) return ENodeState.Failure;
        
        // 执行行为
        return _action.Invoke() ? ENodeState.Success : ENodeState.Failure; 
    }
}
