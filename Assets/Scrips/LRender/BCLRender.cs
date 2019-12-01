using UnityEngine;
using System.Collections;
using System;


//Barycentric Coordinates 扫描算法
public class BCLRender : BaseLRender
{
    ZBuffer _zbuffer;

    LMaterial _Material;

    public BCLRender(ZBuffer z)
    {
        _windowWeigh = Test.ScreenWidth;
        _windowHigh = Test.ScreenHight;
        _zbuffer = z;
    }


    public override void SetUVTexture(LMaterial Material)
    {
        _Material = Material;
    }

    public override void LRenderMode(LVector[] vList, LPlaneIndex Index)
    {
        for (int i = 0; i < Index.getPlaneNum(); ++i)
        {
            if (!Index.getPlaneEnabel(i))
            {
                continue;
            }

            int index1 = Index.getPlaneIndex()[i].getPointIndex()[0];
            int index2 = Index.getPlaneIndex()[i].getPointIndex()[1];
            int index3 = Index.getPlaneIndex()[i].getPointIndex()[2];

            LVector v1 = vList[index1];
            LVector v2 = vList[index2];
            LVector v3 = vList[index3];

            RenderTriangle(ref v1, ref v2, ref v3);
        }
    }

    private void RenderTriangle(ref LVector v1, ref LVector v2, ref LVector v3)
    {
        float maxWidth, minWidth, maxHigh, minHigh;

        maxWidth = MaxWidth(ref v1.Pos, ref v2.Pos, ref v3.Pos);
        minWidth = MinWidth(ref v1.Pos, ref v2.Pos, ref v3.Pos);
        maxHigh = MaxHigh(ref v1.Pos, ref v2.Pos, ref v3.Pos);
        minHigh = MinHigh(ref v1.Pos, ref v2.Pos, ref v3.Pos);

        //计算参数
        float a, b, c, z;
        Color color;

        float FixedParameter1 = CalculateFixedParameter(ref v1.Pos, ref v2.Pos, ref v3.Pos);
        float FixedParameter2 = CalculateFixedParameter(ref v1.Pos, ref v3.Pos, ref v2.Pos);

        for (int i = (int)(minWidth); i < (maxWidth + 0.5f); ++i)
        {
            for (int j = (int)(minHigh); j < (maxHigh + 0.5f); ++j)
            {
                c = CalculateParameter(ref v1.Pos, ref v2.Pos, ref v3.Pos, i, j, FixedParameter1);
                b = CalculateParameter(ref v1.Pos, ref v3.Pos, ref v2.Pos, i, j, FixedParameter2);
                a = 1 - b - c;

                if (CalculateXY(a, b, c))
                {
                    color = CalculateColor(ref v1.color, ref v2.color, ref v3.color, a, b, c);
                    z = CalculateZ(ref v1.Pos, ref v2.Pos, ref v3.Pos, a, b, c);
                    if (_CurFillType == FillType.FILLTYPE_UV)
                    {
                        CalculateUV(ref v1, ref v2, ref v3, a, b, c, ref color);
                    }

                    _zbuffer.ZBufferTestFillColor((i), (j), (z), color);
                }
            }
        }
    }

    private float MaxWidth(ref LVector4 v1, ref LVector4 v2, ref LVector4 v3)
    {
        return Max(v1.x, v2.x, v3.x);
    }

    private float MinWidth(ref LVector4 v1, ref LVector4 v2, ref LVector4 v3)
    {
        return Min(v1.x, v2.x, v3.x);
    }

    private float MaxHigh(ref LVector4 v1, ref LVector4 v2, ref LVector4 v3)
    {
        return Max(v1.y, v2.y, v3.y);
    }

    private float MinHigh(ref LVector4 v1, ref LVector4 v2, ref LVector4 v3)
    {
        return Min(v1.y, v2.y, v3.y);
    }

    private float Max(float a, float b, float c)
    {
        float max = a;
        if( max < b)
        {
            max = b;
        }

        if( max < c)
        {
            max = c;
        }


        return max;
    }

    private float Min(float a, float b,float c)
    {
        float min = a;
        if (min > b)
        {
            min = b;
        }

        if (min > c)
        {
            min = c;
        }

        return min;
    }

    //计算参数
    private float CalculateParameter(ref LVector4 v1, ref LVector4 v2, ref LVector4 v3, float x, float y,float fixedParameter)
    {
        return ((v1.y - v2.y) * x + (v2.x - v1.x) * y + v1.x * v2.y - v2.x * v1.y) / fixedParameter;
    }

    private float CalculateFixedParameter(ref LVector4 v1, ref LVector4 v2, ref LVector4 v3)
    {
        return ((v1.y - v2.y) * v3.x + (v2.x - v1.x) * v3.y + v1.x * v2.y - v2.x * v1.y);
    }


    //计算位置
    private bool CalculateXY(float a, float b, float c)
    {
        if ((a + 0.001) >= 0 && a <= 1.0f
            && (b + 0.001) >= 0 && b <= 1.0f
            && (c + 0.001) >= 0 && c <= 1.0f)
        {
            return true;
        }

        return false;
    }

    private float CalculateZ(ref LVector4 v1, ref LVector4 v2, ref LVector4 v3, float a, float b, float c)
    {
        return (a * (v1.z) + b * (v2.z) + c * (v3.z));
    }


    //计算颜色
    private Color CalculateColor(ref Color c1, ref Color c2, ref Color c3, float a, float b, float c)
    {
        Color color = Color.white;

        color.r = a * c1.r + b * c2.r + c * c3.r;
        color.g = a * c1.g + b * c2.g + c * c3.g;
        color.b = a * c1.b + b * c2.b + c * c3.b;

        return color;
    }

    //计算UV
    private void CalculateUV(ref LVector v1, ref LVector v2, ref LVector v3, float a, float b, float c, ref Color out_PointC)
    {
        int u = (int)((a * (v1.u) + b * (v2.u) + c * (v3.u)) * (_Material.t.width - 1));
        int v = (int)((a * (v1.v) + b * (v2.v) + c * (v3.v)) * (_Material.t.height - 1));

        Color uvcolor = _Material.GetColor(u, v);

        out_PointC.r *= uvcolor.r;
        out_PointC.g *= uvcolor.g;
        out_PointC.b *= uvcolor.b;
        out_PointC.a = uvcolor.a;
    }
}
