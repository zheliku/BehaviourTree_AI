using System;

/// <summary>
/// 序列节点<br/>
/// 特点：<br/>
/// 1. 按顺序执行子节点<br/>
/// 2. 只要有一个子节点返回失败，则整个节点返回失败<br/>
/// 3. 所有子节点都返回成功，则整个节点返回成功
/// </summary>
public class BTSequenceNode : BTControlNode
{
    public override ENodeState Execute() {
        var childNode = _childList[_currentIndex];
        var result    = childNode.Execute();
        switch (result) {
            case ENodeState.Success: { // 成功，则继续下一个节点
                ++_currentIndex;
                if (_currentIndex == _childList.Count) { // 执行到最后，重置索引
                    _currentIndex = 0;
                    return ENodeState.Success;
                }
                break;
            }
            case ENodeState.Failure: { // 失败，则重置索引，直接返回
                _currentIndex = 0;
                return ENodeState.Failure;
            }
            case ENodeState.Running: {
                return ENodeState.Running;
            }
            default: throw new ArgumentOutOfRangeException();
        }

        return ENodeState.Success;
    }
}