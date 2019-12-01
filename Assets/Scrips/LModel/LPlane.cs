using UnityEngine;
using System.Collections;

//平面属性
public class LPlane
{

    //定点索引数组
    int[] _PointIndex;

    //是否启用该平面
    bool IsEnabel;

    //面法向量
    LVector4 Nor;

    public LPlane(int index1, int index2, int index3)
    {
        _PointIndex = new int[3];
        _PointIndex[0] = index1;
        _PointIndex[1] = index2;
        _PointIndex[2] = index3;

        IsEnabel = true;
    }

    //排序定点索引
    public void SortPoint(ref LVector[] v)
    {
        int temp;

        int index1 = _PointIndex[0];
        int index2 = _PointIndex[1];
        int index3 = _PointIndex[2];

        if (v[index1].Pos.x > v[index2].Pos.x)
        {
            temp = _PointIndex[1];
            _PointIndex[1] = _PointIndex[0];
            _PointIndex[0] = temp;
        }

        if (v[index1].Pos.x > v[index3].Pos.x)
        {
            temp = _PointIndex[0];
            _PointIndex[0] = _PointIndex[2];
            _PointIndex[2] = temp;
        }

        if (v[index2].Pos.x > v[index3].Pos.x)
        {
            temp = _PointIndex[1];
            _PointIndex[1] = _PointIndex[2];
            _PointIndex[2] = temp;
        }
    }

    public void SetNor(ref LVector[] v)
    {
        Nor = v[_PointIndex[0]].Pos.Cross(v[_PointIndex[1]].Pos);
    }

    public void SetEnabel(bool IsEnabel)
    {
        this.IsEnabel = IsEnabel;
    }

    public bool getEnabel()
    {
        return this.IsEnabel;
    }

    public int[] getPointIndex()
    {
        return _PointIndex;
    }

    //背面剔除
    public void BackCullFace(ref LVector[] v)
    {

        LVector4 a = v[_PointIndex[0]].Pos;
        LVector4 b = v[_PointIndex[1]].Pos;
        LVector4 c = v[_PointIndex[2]].Pos;

        if (!v[_PointIndex[0]].GetIsEnabel() || !v[_PointIndex[1]].GetIsEnabel() || !v[_PointIndex[2]].GetIsEnabel())
        {
            SetEnabel(false);
            return;
        }
        else
        {
            SetEnabel(true);
        }

        float area = a.x * b.y + a.y * c.x + b.x * c.y - b.y * c.x - a.y * b.x - a.x * c.y;

        if (area >= 0)
        {
            SetEnabel(false);
        }
        else
        {
            SetEnabel(true);
        }
    }
}
