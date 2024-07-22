using UnityEngine;

public abstract class BaseControl
{
    protected Monster _monster;

    public abstract void Init(Monster monster);
    public abstract bool Update();
    public abstract bool CanControl();
    public abstract void DrawGizmos();
    public abstract void Reset();

    protected float DistanceOfXZ(Vector3 pos1, Vector3 pos2) {
        pos1.y = pos2.y = 0;
        return Vector3.Distance(pos1, pos2);
    }
}