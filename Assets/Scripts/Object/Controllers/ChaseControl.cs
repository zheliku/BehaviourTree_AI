using System;
using DrawXXL;
using UnityEngine;

[Serializable]
public class ChaseControl : BaseControl
{
    public float ChaseDistance = 7f; // 与目标小于该距离，则开始追逐

    private bool _isChase; // 追逐标识

    public override void Init(Monster monster) {
        _monster = monster;
    }

    public override bool Update() {
        _monster.Move(_monster.TargetTransform.position);
        _monster.CurrentState = 4;
        return true;
    }

    public override void DrawGizmos() {
        // 绘制攻击范围
        DrawShapes.Circle(_monster.transform.position, _monster.AttackRange, Color.red,
                          Vector3.up, lineWidth: 0.05f);

        // 绘制追逐范围
        DrawShapes.Circle(_monster.BornPos, 15, Color.green,
                          Vector3.up, lineWidth: 0.05f, outlineStyle: DrawBasics.LineStyle.dotted);
    }

    public override bool CanControl() {
        // 远离出生点后，重置 Chase 状态
        if (DistanceOfXZ(_monster.transform.position, _monster.BornPos) > _monster.BackCtrl.BackDistance && _isChase) {
            _isChase = false;
            return false;
        }

        // 距离目标小于追逐距离，则开始追逐
        if (DistanceOfXZ(_monster.transform.position, _monster.TargetTransform.position) < ChaseDistance || _isChase) {
            _monster.ClearData(11); // 1011，清除其他状态
            _isChase = true;
            return true;
        }
        return false;
    }

    public override void Reset() {
        _isChase = false;
    }
}