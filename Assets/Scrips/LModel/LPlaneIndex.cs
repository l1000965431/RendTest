using UnityEngine;
using System.Collections;

//平面索引列表
public class LPlaneIndex
{

    //平面索引
    LPlane[] _PlaneIndex;

    //平面数量
    int _PlaneNum;

    public LPlane[] getPlaneIndex()
    {
        return _PlaneIndex;
    }

    public int getPlaneNum()
    {
        return _PlaneNum;
    }

    public LPlaneIndex(int PlaneNum, int PointNum, ref LVector[] v, int[] Index)
    {
        _PlaneNum = PlaneNum;
        _PlaneIndex = new LPlane[PlaneNum];
        int j = 0;
        for (int i = 0; i < Index.Length; i += 3)
        {
            _PlaneIndex[j] = new LPlane(Index[i], Index[i+1], Index[i+2]);
            _PlaneIndex[j].SetNor(ref v);
            j++;
        }
    }

    public int[] getPointIndex(int Index)
    {
        return _PlaneIndex[Index].getPointIndex();
    }

    public void EnabelPalne(int Index)
    {
        _PlaneIndex[Index].SetEnabel(true);
    }

    public void Disabel(int Index)
    {
        _PlaneIndex[Index].SetEnabel(false);
    }

    public bool getPlaneEnabel(int Index)
    {
        return _PlaneIndex[Index].getEnabel();
    }

    public void BackCullFace(int Index,ref LVector[] v)
    {
        _PlaneIndex[Index].BackCullFace(ref v);
    }
}
