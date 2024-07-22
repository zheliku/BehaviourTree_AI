using System;

/// <summary>
/// 选择节点<br/>
/// 特点：<br/>
/// 1. 按顺序执行子节点<br/>
/// 2. 如果某子节点返回成功，则返回成功，不执行后续结点<br/>
/// 3. 如果某子节点返回失败，则继续执行下一个子节点
/// </summary>
public class BTSelectNode : BTControlNode
{
    public override ENodeState Execute() {
        var childNode = _childList[_currentIndex];
        var result    = childNode.Execute();
        switch (result) {
            case ENodeState.Success: { // 成功，则重置索引，直接返回
                _currentIndex = 0;
                return ENodeState.Success;
            }
            case ENodeState.Failure: { // 失败，则继续下一个节点
                ++_currentIndex;
                if (_currentIndex == _childList.Count) { // 执行到最后，重置索引
                    _currentIndex = 0;
                    return ENodeState.Failure;
                }
                break;
            }
            case ENodeState.Running: {
                return ENodeState.Running;
            }
            default: throw new ArgumentOutOfRangeException();
        }

        // 没有执行完，或者节点失败，才执行该逻辑
        // 此时仍希望下一帧继续往后执行，因此返回成功
        return ENodeState.Success;
    }
}