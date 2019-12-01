using UnityEngine;
using System.Collections;

//定点格式
public class LVector{
    //位置
    public LVector4 Pos;
    //颜色
    public Color color;
    //UV坐标
    public float u, v;
    //定点法线
    public LVector4 Nor;
    //是否启用
    bool _IsEnabel;

    public LVector()
    {
        Pos = new LVector4();
        Nor = new LVector4();
        u = 0.0f;
        v = 0.0f;
        color = Color.white;
        _IsEnabel = true;
    }

    public LVector(LVector v1)
    {
        Pos = new LVector4();
        Clear();
    }

    public void Clear()
    {
        Pos.Clear();
        u = 0.0f;
        v = 0.0f;
        color = Color.white;
        _IsEnabel = true;
    }

    public void SetUV(float u, float v)
    {
        this.u = u;
        this.v = v;
    }

    public void SetColor(Color c)
    {
        color = c;
    }

    public void SetPos(LVector4 v)
    {
        Pos = v;
    }

    public void SetEnabel(bool IsEnabel)
    {
        _IsEnabel = IsEnabel;
    }

    public bool GetIsEnabel()
    {
        return _IsEnabel;
    }
}
