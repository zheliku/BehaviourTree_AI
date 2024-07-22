using System;

/// <summary>
/// 动作节点，执行具体行为，没有子节点
/// </summary>
public class BTActionNode : BTBaseNode
{
    private Func<bool> _action; // 返回值表示执行是否成功

    public BTActionNode(Func<bool> action) { _action = action; }

    public override ENodeState Execute() {
        if (_action == null) return ENodeState.Failure;
        
        // 执行行为
        return _action.Invoke() ? ENodeState.Success : ENodeState.Failure; 
    }
}