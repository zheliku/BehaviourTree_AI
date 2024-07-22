using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Monster : MonoBehaviour
{
    public Transform  TargetTransform; // 目标位置
    public GameObject Bullet;          // 子弹
    public float      AttackRange = 3; // 攻击范围
    public Vector3    BornPos;         // 出生位置
    public int        CurrentState = 0b1000;

    // 行为控制器
    public PatrolControl PatrolCtrl;
    public ChaseControl  ChaseCtrl;
    public BackControl   BackCtrl;
    public AttackControl AttackCtrl;

    private BTControlNode _btAIRoot;     // 行为树根节点
    private NavMeshAgent  _navMeshAgent; // 导航代理

    private Quaternion _startRotation;  // 记录开始攻击时的角度
    private Quaternion _targetRotation; // 记录目标角度
    private float      _rotateTime;     // 旋转计时
    
    public void ClearData(int state) {
        if ((state & 1) == 1)
            AttackCtrl.Reset();

        if ((state & 2) == 2)
            BackCtrl.Reset();

        if ((state & 4) == 4)
            ChaseCtrl.Reset();

        if ((state & 8) == 8)
            PatrolCtrl.Reset();
    }

    // Start is called before the first frame update
    void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        BornPos       = transform.position;

        PatrolCtrl.Init(this);
        ChaseCtrl.Init(this);
        BackCtrl.Init(this);
        AttackCtrl.Init(this);

        // 创建行为树
        _btAIRoot = new BTSelectNode();
        var atkNode    = CreateSequenceNode(AttackCtrl.CanControl, AttackCtrl.Update);
        var backNode   = CreateSequenceNode(BackCtrl.CanControl, BackCtrl.Update);
        var moveNode   = CreateSequenceNode(ChaseCtrl.CanControl, ChaseCtrl.Update);
        var patrolNode = CreateSequenceNode(PatrolCtrl.CanControl, PatrolCtrl.Update);
        _btAIRoot.AddChild(atkNode, backNode, moveNode, patrolNode);
    }

    // Update is called once per frame
    void Update() {
        _btAIRoot.Execute(); // 执行行为树

        switch (CurrentState) { // 依据当前行为绘制辅助线
            case 1:
                AttackCtrl.DrawGizmos();
                break;
            case 2:
                BackCtrl.DrawGizmos();
                break;
            case 4:
                ChaseCtrl.DrawGizmos();
                break;
            case 8:
                PatrolCtrl.DrawGizmos();
                break;
        }
    }

    /// <summary>
    /// 封装方法
    /// </summary>
    private BTSequenceNode CreateSequenceNode(Func<bool> condition, Func<bool> action) {
        var node          = new BTSequenceNode();
        var conditionNode = new BTConditionNode(condition);
        var actionNode    = new BTActionNode(action);

        node.AddChild(conditionNode, actionNode);
        return node;
    }

    public void Move(Vector3 targetPos) {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(targetPos);
    }

    public void StopMove() {
        _navMeshAgent.isStopped = true;
    }

    public bool LookAt(Vector3 targetPos) {
        // 实现匀速看向目标
        var newTargetRotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
        if (_targetRotation != newTargetRotation) {
            _targetRotation = newTargetRotation;
            _rotateTime     = 0;
            _startRotation  = transform.rotation;
        }
        _rotateTime        += Time.deltaTime * _navMeshAgent.angularSpeed / 60;
        transform.rotation =  Quaternion.Slerp(_startRotation, _targetRotation, _rotateTime);
        return Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f; // 角度 < 0.1 认为已经看向了目标
    }

    public void Attack() {
        Debug.Log("Attack");
        Instantiate(Bullet,
                    transform.position + transform.forward * 0.5f,
                    transform.rotation);
    }
}