using System;
using DrawXXL;
using UnityEngine;

[Serializable]
public class AttackControl : BaseControl
{
    public float NextAttackTime;          // 下次攻击的时间
    public float AttackIntervalTime = 1f; // 攻击间隔时间

    public override void Init(Monster monster) {
        _monster = monster;
    }

    public override bool Update() {
        // 看向目标并且达到计时，才进行攻击
        if (_monster.LookAt(_monster.TargetTransform.position) &&
            Time.time >= NextAttackTime) {
            _monster.Attack();                                           // 执行攻击
            NextAttackTime = Time.time + AttackIntervalTime; // 更新下次攻击的时间
        }

        _monster.CurrentState = 1;
        return true;
    }

    public override void DrawGizmos() {
        // 绘制攻击范围
        DrawShapes.Circle(_monster.transform.position, _monster.AttackRange, Color.red,
                          Vector3.up, lineWidth: 0.05f);
    }

    public override bool CanControl() {
        // 与目标距离小于攻击范围，则进行攻击
        if (DistanceOfXZ(_monster.transform.position, _monster.TargetTransform.position) < _monster.AttackRange) {
            _monster.ClearData(8); // 1110，清除其他状态
            _monster.StopMove();
            return true;
        }
        return false;
    }

    public override void Reset() { }
}