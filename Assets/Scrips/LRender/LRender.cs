using UnityEngine;
using System.Collections;
using System;

//线性插值渲染组件(横向(弃用)和竖向扫描)
public class LRender: BaseLRender
{
    //窗口句柄
    ColorLerp colorLerp;

    public LRender(ZBuffer z)
    {
        _windowWeigh = Test.ScreenWidth;
        _windowHigh = Test.ScreenHight;
        colorLerp = new ColorLerp(_windowWeigh, _windowWeigh, z);
    }

    public override void LRenderMode(LVector[] vList, LPlaneIndex Index)
    {
        for (int i = 0; i < Index.getPlaneNum(); ++i)
        {
            if (!Index.getPlaneEnabel(i))
            {
                continue;
            }

            LRenderModeY(vList, Index.getPlaneIndex()[i].getPointIndex());
        }
    }

    //渲染函数
    public bool LRenderModeY(LVector[] vList, int[] Index)
    {
        //三角形索引

        int index1 = Index[0];
        int index2 = Index[1];
        int index3 = Index[2];

        LVector v1 = vList[index1];
        LVector v2 = vList[index2];
        LVector v3 = vList[index3];

        if (!DrawSpecialY(v1, v2, v3, false))
        {
            //找三角形特殊点划分平底和平顶三角形
            float y21 = Mathf.Abs(v2.Pos.y - v1.Pos.y);
            float y23 = Mathf.Abs(v2.Pos.y - v3.Pos.y);
            float y13 = Mathf.Abs(v3.Pos.y - v1.Pos.y);

            float max = Mathf.Max(y21, y23, y13);
            LVector Xmiddle = null;
            if (max == y21)
            {
                Xmiddle = colorLerp.CalculateMiddlePointY(v3, v1, v2, _CurFillType);

                DrawSpecialY(v1, Xmiddle, v3, false);
                DrawSpecialY(Xmiddle, v2, v3, false);
            }
            else if (max == y23)
            {
                Xmiddle = colorLerp.CalculateMiddlePointY(v1, v3, v2, _CurFillType);

                DrawSpecialY(v1, Xmiddle, v2, false);
                DrawSpecialY(Xmiddle, v1, v3, false);
            }
            else
            {
                Xmiddle = colorLerp.CalculateMiddlePointY(v2, v1, v3, _CurFillType);

                DrawSpecialY(v1, Xmiddle, v2, false);
                DrawSpecialY(Xmiddle, v2, v3, false);
            }
        }

        return true;
    }

    public void LRenderModeX(LVector[] vList, int[] Index)
    {
        //三角形索引

        int index1 = Index[0];
        int index2 = Index[1];
        int index3 = Index[2];

        LVector v1 = vList[index1];
        LVector v2 = vList[index2];
        LVector v3 = vList[index3];

        if (!DrawSpecialX(v1, v2, v3, true))
        {
            //找三角形特殊点划分平底和平顶三角形
            float y21 = Mathf.Abs(v2.Pos.x - v1.Pos.x);
            float y23 = Mathf.Abs(v2.Pos.x - v3.Pos.x);
            float y13 = Mathf.Abs(v3.Pos.x - v1.Pos.x);

            float max = Mathf.Max(y21, y23, y13);
            LVector Xmiddle = null;
            if (max == y21)
            {
                Xmiddle = colorLerp.CalculateMiddlePointX(v3, v1, v2, _CurFillType);

                DrawSpecialX(v1, Xmiddle, v3, false);
                DrawSpecialX(Xmiddle, v2, v3, false);
            }
            else if (max == y23)
            {
                Xmiddle = colorLerp.CalculateMiddlePointX(v1, v3, v2, _CurFillType);

                DrawSpecialX(v1, Xmiddle, v2, false);
                DrawSpecialX(Xmiddle, v1, v3, false);
            }
            else
            {
                Xmiddle = colorLerp.CalculateMiddlePointX(v2, v1, v3, _CurFillType);

                DrawSpecialX(v1, Xmiddle, v2, false);
                DrawSpecialX(Xmiddle, v2, v3, false);
            }
        }
    }

    //画特殊三角形(按Y轴扫描)
    private bool DrawSpecialY(LVector v1, LVector v2, LVector v3, bool IsDrawClosed)
    {
        SortPointY(ref v1, ref v2, ref v3);

        //退化成直线
        if ((Mathf.Abs(v1.Pos.y - v2.Pos.y) < 0.1f && (Mathf.Abs(v3.Pos.y - v2.Pos.y) < 0.1f)))
        {
            if (_CurFillType == FillType.FILLTYPE_WIREFRAME)
            {
                colorLerp.DrawLineX(v1, v3, FillType.FILLTYPE_FILLMODE);
            }
            else
            {
                colorLerp.DrawLineX(v1, v3, _CurFillType);
            }
            return true;
        }

        if ((Mathf.Abs(v1.Pos.y - v3.Pos.y) <= 0.1f) && v2.Pos.y < v3.Pos.y)
        {
            DrawTriangleY2(v1, v2, v3, _CurFillType, IsDrawClosed);
            return true;
        }
        else if ((Mathf.Abs(v1.Pos.y - v3.Pos.y) <= 0.1f) && v2.Pos.y > v3.Pos.y)
        {
            DrawTriangleY1(v1, v2, v3, _CurFillType, IsDrawClosed);
            return true;
        }

        return false;
    }

    //画特殊三角形(按X轴扫描)
    private bool DrawSpecialX(LVector v1, LVector v2, LVector v3, bool IsDrawClosed)
    {
        SortPointX(ref v1, ref v2, ref v3);

        //退化成直线
        if ((Mathf.Abs(v1.Pos.x - v2.Pos.x) < 1.0f && (Mathf.Abs(v3.Pos.x - v2.Pos.x) < 1.0f)))
        {
            if (_CurFillType == FillType.FILLTYPE_WIREFRAME)
            {
                colorLerp.DrawLineY(v1, v3, FillType.FILLTYPE_FILLMODE);
            }
            else
            {
                colorLerp.DrawLineY(v1, v3, _CurFillType);
            }
            return true;
        }

        if ((Mathf.Abs(v1.Pos.x - v3.Pos.x) <= 0.1f) && v2.Pos.x < v3.Pos.x)
        {
            DrawTriangleX2(v1, v2, v3, _CurFillType, IsDrawClosed);
            return true;
        }
        else if ((Mathf.Abs(v1.Pos.x - v3.Pos.x) <= 0.1f) && v2.Pos.x > v3.Pos.x)
        {
            DrawTriangleX1(v1, v2, v3, _CurFillType, IsDrawClosed);
            return true;
        }

        return false;
    }

    private void DrawTriangle(LVector v1, LVector v2, LVector v3, bool IsDrawClosed)
    {
        SortPointY(ref v1, ref v2, ref v3);

        //退化成直线
        if (Mathf.Abs(v1.Pos.y - v3.Pos.y) <= 0.3f && Mathf.Abs(v2.Pos.y - v3.Pos.y) <= 0.3f)
        {
            colorLerp.DrawLineX(v1, v3, _CurFillType);
            return;
        }


        if ((Mathf.Abs(v1.Pos.y - v3.Pos.y) <= 0.1f) && v2.Pos.y < v3.Pos.y)
        {
            DrawTriangleY2(v1, v2, v3, _CurFillType, IsDrawClosed);
            return;
        }
        else if ((Mathf.Abs(v1.Pos.y - v3.Pos.y) <= 0.1f) && v2.Pos.y > v3.Pos.y)
        {
            DrawTriangleY1(v1, v2, v3, _CurFillType, IsDrawClosed);
            return;
        }
        else
        {
            //找三角形特殊点划分平底和平顶三角形
            float y21 = Mathf.Abs(v2.Pos.y - v1.Pos.y);
            float y23 = Mathf.Abs(v2.Pos.y - v3.Pos.y);
            float y13 = Mathf.Abs(v3.Pos.y - v1.Pos.y);

            float max = Mathf.Max(y21, y23, y13);
            LVector Xmiddle = null;
            if (max == y21)
            {
                Xmiddle = colorLerp.CalculateMiddlePointY(v3, v1, v2, _CurFillType);

                DrawTriangle(v1, Xmiddle, v3, false);
                DrawTriangle(Xmiddle, v2, v3, false);
            }
            else if (max == y23)
            {
                Xmiddle = colorLerp.CalculateMiddlePointY(v1, v3, v2, _CurFillType);

                DrawTriangle(v1, Xmiddle, v2, false);
                DrawTriangle(Xmiddle, v1, v3, false);
            }
            else
            {
                Xmiddle = colorLerp.CalculateMiddlePointY(v2, v1, v3, _CurFillType);

                DrawTriangle(v1, Xmiddle, v2, false);
                DrawTriangle(Xmiddle, v2, v3, false);
            }


        }
    }

    //平底三角形
    private void DrawTriangleY1(LVector v1, LVector v2, LVector v3, FillType filltype, bool IsDrawClose)
    {
        this.beginY(v1, v2, v3, TriangleType.TriangleType1, filltype);

        float dy = 1 / (v2.Pos.y - v1.Pos.y);

        float slope_start, slope_end, slopz_start, slopz_end;

        slope_start = (v2.Pos.x - v1.Pos.x) * dy;
        slope_end = (v2.Pos.x - v3.Pos.x) * dy;
        slopz_start = (v2.Pos.z - v1.Pos.z) * dy;
        slopz_end = (v2.Pos.z - v3.Pos.z) * dy;

        float x1 = v1.Pos.x;
        float x2 = v3.Pos.x+0.5f;
        float z1 = v1.Pos.z;
        float z2 = v3.Pos.z;
        float xs = 0;
        float xe = 0;
        float zs = 0;
        float ze = 0;
        float dx_start = 0;
        float dx_end = 0;
        float dz_start = 0;
        float dz_end = 0;

        if (v2.Pos.y - v1.Pos.y >= _windowHigh)
        {
            return;
        }

        for (int y = (int)v1.Pos.y; y < (int)(v2.Pos.y); y++)
        {
            xs = (x1 + dx_start);
            zs = (z1 + dz_start);
            xe = (x2 + dx_end);
            ze = (z2 + dz_end);

            colorLerp.ColorLerpCalculateY(xe, ze, xs, zs, y, filltype);

            dx_start += slope_start;
            dx_end += slope_end;
            dz_start += slopz_start;
            dz_end += slopz_end;
        }

        //画封闭线
        if (filltype == FillType.FILLTYPE_WIREFRAME && IsDrawClose)
        {
            colorLerp.DrawLineX(v1, v3, FillType.FILLTYPE_FILLMODE);
        }

        this.end();
    }

    //平顶三角形
    private void DrawTriangleY2(LVector v1, LVector v2, LVector v3, FillType filltype, bool IsDrawClose)
    {
        this.beginY(v1, v2, v3, TriangleType.TriangleType2, filltype);

        float dy = 1 / (v1.Pos.y - v2.Pos.y);

        float slope_start, slope_end, slopz_start, slopz_end;
        slope_start = (v1.Pos.x - v2.Pos.x) * dy;
        slope_end = (v3.Pos.x - v2.Pos.x) * dy;
        slopz_start = (v1.Pos.z - v2.Pos.z) * dy;
        slopz_end = (v3.Pos.z - v2.Pos.z) * dy;

        float x1 = v2.Pos.x;
        float z1 = v2.Pos.z;
        float xs = 0;
        float xe = 0;
        float zs = 0;
        float ze = 0;
        float dx_start = 0;
        float dx_end = 0;
        float dz_start = 0;
        float dz_end = 0;
        if (v3.Pos.y - v2.Pos.y >= _windowHigh)
        {
            return;
        }

        for (int y = (int)v2.Pos.y; y < (int)(v3.Pos.y+0.5f); y++)
        {
            xs = (x1 + dx_start);
            zs = (z1 + dz_start);

            xe = (x1 + dx_end);
            ze = (z1 + dz_end);

            colorLerp.ColorLerpCalculateY(xe, ze, xs, zs, y, filltype);

            dx_start += slope_start;
            dx_end += slope_end;
            dz_start += slopz_start;
            dz_end += slopz_end;
        }

        //画封闭线
        if (filltype == FillType.FILLTYPE_WIREFRAME && IsDrawClose)
        {
            colorLerp.DrawLineX(v1, v3, FillType.FILLTYPE_FILLMODE);
        }

        this.end();
    }

    private void DrawTriangleX1(LVector v1, LVector v2, LVector v3, FillType filltype, bool IsDrawClose)
    {
        this.beginX(v1, v2, v3, TriangleType.TriangleType1, filltype);

        float dy = 1 / (v2.Pos.x - v1.Pos.x);
        float slope_start, slope_end, slopz_start, slopz_end;

        slope_start = ((v2.Pos.y - v1.Pos.y) * dy);
        slope_end = ((v2.Pos.y - v3.Pos.y) * dy);
        slopz_start = ((v2.Pos.z - v1.Pos.z) * dy);
        slopz_end = ((v2.Pos.z - v3.Pos.z) * dy);

        float x1 = v1.Pos.y;
        float x2 = v3.Pos.y+0.5f;
        float z1 = v1.Pos.z;
        float z2 = v3.Pos.z;
        float xs = 0;
        float xe = 0;
        float zs = 0;
        float ze = 0;
        float dx_start = 0;
        float dx_end = 0;
        float dz_start = 0;
        float dz_end = 0;

        if (v2.Pos.x - v1.Pos.x >= _windowWeigh)
        {
            return;
        }

        for (int y = (int)v1.Pos.x; y < (int)(v2.Pos.x+0.5f); y++)
        {
            xs = (x1 + dx_start);
            zs = (z1 + slopz_start);
            xe = (x2 + dx_end);
            ze = (z2 + slopz_end);

            colorLerp.ColorLerpCalculateX(xe, ze, xs, zs, y, filltype);

            dx_start += slope_start;
            dx_end += slope_end;
            dz_start += slopz_start;
            dz_end += slopz_end;
        }

        //画封闭线
        if (filltype == FillType.FILLTYPE_WIREFRAME && IsDrawClose)
        {
            colorLerp.DrawLineY(v1, v3, FillType.FILLTYPE_FILLMODE);
        }

        this.end();
    }

    private void DrawTriangleX2(LVector v1, LVector v2, LVector v3, FillType filltype, bool IsDrawClose)
    {
        this.beginX(v1, v2, v3, TriangleType.TriangleType2, filltype);

        float dy = 1 / (v1.Pos.x - v2.Pos.x);
        float slope_start, slope_end, slopz_start, slopz_end;
        slope_start = ((v1.Pos.y - v2.Pos.y) * dy);
        slope_end = ((v3.Pos.y - v2.Pos.y) * dy);
        slopz_start = ((v1.Pos.z - v2.Pos.z) * dy);
        slopz_end = ((v3.Pos.z - v2.Pos.z) * dy);

        float x1 = v2.Pos.y;
        float z1 = v2.Pos.z;
        float xs = 0;
        float xe = 0;
        float zs = 0;
        float ze = 0;
        float dx_start = 0;
        float dx_end = 0;
        float dz_start = 0;
        float dz_end = 0;

        if (v3.Pos.x - v2.Pos.x >= _windowWeigh)
        {
            return;
        }

        for (int y = (int)v2.Pos.x; y < Mathf.RoundToInt(v3.Pos.x); y++)
        {
            xs = (x1 + dx_start);
            zs = (z1 + dz_start);

            xe = (x1 + dx_end);
            ze = (z1 + dz_end);

            colorLerp.ColorLerpCalculateX(xe, ze, xs, zs, y, filltype);

            dx_start += slope_start;
            dx_end += slope_end;
            dz_start += slopz_start;
            dz_end += slopz_end;
        }

        //画封闭线
        if (filltype == FillType.FILLTYPE_WIREFRAME && IsDrawClose)
        {
            colorLerp.DrawLineY(v1, v3, FillType.FILLTYPE_FILLMODE);
        }

        this.end();
    }

    private void beginY(LVector v1, LVector v2, LVector v3, TriangleType triangleType, FillType fillType)
    {
        colorLerp.beginY(v1, v2, v3, triangleType, fillType);
    }

    private void beginX(LVector v1, LVector v2, LVector v3, TriangleType triangleType, FillType fillType)
    {
        colorLerp.beginX(v1, v2, v3, triangleType, fillType);
    }

    private void end()
    {
        colorLerp.end();
    }

    public override void SetUVTexture(LMaterial Material)
    {
        colorLerp.SetTexture(Material);
    }

    //排序三角形定点(按Y)
    private void SortPointY(ref LVector v1, ref LVector v2, ref LVector v3)
    {
        LVector temp;
        //根据X交换
        if (v1.Pos.x > v2.Pos.x)
        {
            temp = v2;
            v2 = v1;
            v1 = temp;
        }

        if (v1.Pos.x > v3.Pos.x)
        {
            temp = v3;
            v3 = v1;
            v1 = temp;
        }

        if (v2.Pos.x > v3.Pos.x)
        {
            temp = v3;
            v3 = v2;
            v2 = temp;
        }

        //根据Y交换
        if (Mathf.Abs(v1.Pos.y - v2.Pos.y) <= 0.1f)
        {
            temp = v3;
            v3 = v2;
            v2 = temp;
        }
        else if (Mathf.Abs(v2.Pos.y - v3.Pos.y) <= 0.1f)
        {
            temp = v1;
            v1 = v2;
            v2 = temp;
        }
    }

    //排序三角形定点(按X)
    private void SortPointX(ref LVector v1, ref LVector v2, ref LVector v3)
    {
        LVector temp;
        //根据Y交换
        if (v1.Pos.y > v2.Pos.y)
        {
            temp = v2;
            v2 = v1;
            v1 = temp;
        }

        if (v1.Pos.y > v3.Pos.y)
        {
            temp = v3;
            v3 = v1;
            v1 = temp;
        }

        if (v2.Pos.y > v3.Pos.y)
        {
            temp = v3;
            v3 = v2;
            v2 = temp;
        }

        //根据X交换
        if (Mathf.Abs(v1.Pos.x - v2.Pos.x) <= 0.1f)
        {
            temp = v3;
            v3 = v2;
            v2 = temp;
        }
        else if (Mathf.Abs(v2.Pos.x - v3.Pos.x) <= 0.1f)
        {
            temp = v1;
            v1 = v2;
            v2 = temp;
        }
    }
}
