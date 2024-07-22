using System;
using DrawXXL;
using UnityEngine;

[Serializable]
public class BackControl : BaseControl
{
    public float BackDistance = 15f; // 与出生点大于该距离，则返回

    private bool _isBack = false; // 返回出生点标识

    public override void Init(Monster monster) {
        _monster = monster;
    }

    public override bool Update() {
        Debug.Log("BackState");
        _monster.CurrentState = 2;

        return true;
    }

    public override void DrawGizmos() {
        // 绘制脱离范围
        DrawShapes.Circle(_monster.BornPos, BackDistance, Color.green,
                          Vector3.up, lineWidth: 0.05f, outlineStyle: DrawBasics.LineStyle.dotted);

        DrawBasics.Point(_monster.BornPos, markingCrossLinesWidth: 0.1f);

        DrawBasics.MovingArrowsLine(_monster.transform.position, _monster.BornPos, Color.blue,
                                    lineWidth: 0.2f, lengthOfArrows: 0.3f,
                                    customAmplitudeAndTextDir: Vector3.forward);
        
        // 绘制攻击范围
        DrawShapes.Circle(_monster.transform.position, _monster.AttackRange, Color.red,
                          Vector3.up, lineWidth: 0.05f);
    }

    public override bool CanControl() {
        // 回到出生点，则停止
        if (DistanceOfXZ(_monster.transform.position, _monster.BornPos) < 0.5f && _isBack) {
            _isBack = false;
            _monster.StopMove();
            return false;
        }

        // 如果与出生点的距离大于 BackDistance，则返回
        if (DistanceOfXZ(_monster.transform.position, _monster.BornPos) > BackDistance || _isBack) {
            _monster.ClearData(13); // 1101，清除其他状态
            _isBack = true;
            _monster.Move(_monster.BornPos);
            return true;
        }
        return false;
    }

    public override void Reset() {
        _isBack = false;
    }
}