using System;
using System.Collections.Generic;
using DrawXXL;
using UnityEngine;

/// <summary>
/// 巡逻控制类，处理巡逻逻辑
/// </summary>
[Serializable]
public class PatrolControl : BaseControl
{
    public List<Vector3> PointList = new List<Vector3>(); // 路径点
    public float         WaitTime;                        // 巡逻等待时间

    private Vector3 _targetPos;      // 目标位置
    private Vector3 _lastTargetPos;  // 目标位置
    private int     _pointIndex = 0; // 当前点索引
    private bool    _isWaiting;      // 是否正在等待
    private float   _curTime;        // 计时时间

    public override void Init(Monster monster) {
        _monster = monster;
    }

    public override bool Update() {
        OnMoveUpdate(_monster);
        _monster.CurrentState = 8;
        return true;
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void OnMoveUpdate(Monster monster) {
        if (_curTime <= 0) {
            if (_isWaiting) {
                _lastTargetPos = _targetPos;
                _targetPos     = CalPathTargetPos();
                _isWaiting     = false;
            }
            monster.Move(_targetPos); // 移动中...

            // 判断是否到达目的地
            if (DistanceOfXZ(_targetPos, monster.transform.position) < 0.2f) {
                _isWaiting = true;     // 继续下一个位置
                _curTime   = WaitTime; // 重新计时
                monster.StopMove();    // 停止移动
            }
        }
        else {
            _curTime -= Time.deltaTime; // 计时
        }
    }

    /// <summary>
    /// 更新路径范围目标位置
    /// </summary>
    private Vector3 CalPathTargetPos() {
        var targetPos = PointList[_pointIndex];
        // 更新下一个位置
        _pointIndex = (_pointIndex + 1) % PointList.Count;

        return targetPos;
    }

    public override void DrawGizmos() {
        // 绘制巡逻点
        DrawBasics.PointList(PointList, markingCrossLinesWidth: 0.1f);

        // 绘制脱离范围
        DrawShapes.Circle(_monster.transform.position, _monster.ChaseCtrl.ChaseDistance, Color.green,
                          Vector3.up, lineWidth: 0.05f, outlineStyle: DrawBasics.LineStyle.dotted);

        // 绘制移动路径
        DrawBasics.MovingArrowsLine(_lastTargetPos, _targetPos, Color.blue,
                                    lineWidth: 0.2f, lengthOfArrows: 0.3f,
                                    customAmplitudeAndTextDir: Vector3.forward);
    }

    public override bool CanControl() {
        // 远离出生点，则不巡逻，而是进入新一轮的判断
        if (DistanceOfXZ(_monster.transform.position, _monster.BornPos) > _monster.BackCtrl.BackDistance) {
            _monster.StopMove();
            return false;
        }

        _monster.ClearData(7); // 0111，清除其他状态
        return true;
    }

    public override void Reset() { }
}