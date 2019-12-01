using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class BaseLRender
{
    //窗口句柄
    

    protected int _windowWeigh;
    protected int _windowHigh;

    public enum FillType
    {
        FILLTYPE_FILLMODE,         //线框渲染
        FILLTYPE_WIREFRAME,        //颜色填充渲染
        FILLTYPE_UV,               //纹理填充

        FILLTYPE_NUM,
    }

    public enum TriangleType
    {
        TriangleType1 = 1,        //平底
        TriangleType2,            //平顶
    }

    protected FillType _CurFillType;

    public void SetFillTyp(FillType fillType)
    {
        _CurFillType = fillType;
    }

    public FillType GetCurFillType()
    {
        return _CurFillType;
    }

    public abstract void SetUVTexture(LMaterial Material);

    public abstract void LRenderMode(LVector[] vList, LPlaneIndex Index);
}

