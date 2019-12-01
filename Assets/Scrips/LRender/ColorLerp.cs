using UnityEngine;
using System.Collections;

//线性颜色插值
public class ColorLerp
{
    LMaterial _m;

    LVector _v1;
    LVector _v2;
    LVector _v3;

    float dy;
    float slope_start_r, slope_start_g, slope_start_b, slope_end_r, slope_end_g, slope_end_b;

    //颜色计算
    float dr_start = 0;
    float dg_start = 0;
    float db_start = 0;

    float dr_end = 0;
    float dg_end = 0;
    float db_end = 0;

    //UV 计算
    float slope_start_u;
    float slope_start_v;
    float slope_end_u;
    float slope_end_v;

    float du_strat;
    float dv_strat;
    float du_end;
    float dv_end;

    float lenth;

    int _textureWidth;
    int _textureHigh;

    LRender.TriangleType _triangleType;

    ZBuffer _zBuffer;

    public ColorLerp(int ScreenWidth, int ScreenHight, ZBuffer z)
    {
        _zBuffer = z;
    }

    public void SetTexture(LMaterial Material)
    {
        _m = Material;
        _textureWidth = _m.t.width;
        _textureHigh = _m.t.height;
    }

    private Color GetPixel(int x, int y)
    {
        return _m.GetColor(x, y);
    }

    private float slop(float f1, float f2, float dy)
    {
        return (f1 - f2) * dy;
    }

    private void LerpX(Color cs, Color ce, float zs, float ze, float y, float xs, float xe,
        float us, float vs, float ue, float ve, float lenth,
        LRender.FillType FillType, bool Isuv)
    {
        if (lenth > Test.ScreenWidth)
        {
            return;
        }

        

        if (FillType == LRender.FillType.FILLTYPE_WIREFRAME)
        {
            _zBuffer.ZBufferTestFillColor((int)(xs), (int)y, zs, cs);
            _zBuffer.ZBufferTestFillColor((int)(xs + lenth), (int)y, ze, ce);
        }
        else
        {
            if(lenth < 0.1f)
            {
                if (Isuv)
                {
                    int u = Mathf.RoundToInt(us* _textureWidth);
                    int v = Mathf.RoundToInt(vs* _textureHigh);
                    cs = cs* GetPixel(u, v);
                }
                _zBuffer.ZBufferTestFillColor((int)(xs), (int)y, zs, cs);
                return;
            }

            float lenth1 = 1 / lenth;
            float dr_x = (ce.r - cs.r) * lenth1;
            float dg_x = (ce.g - cs.g) * lenth1;
            float db_x = (ce.b - cs.b) * lenth1;
            float dr_u = 0;
            float dr_v = 0;

            float dr_z = (ze - zs) * lenth1;
            //float slop_z = 0;
            float z = zs;

            float g = ue;
            float g1 = us;

            float f = ve;
            float f1 = vs;

            float fg_d = (g - g1) * lenth1 * _textureWidth;
            float fg_s = (f - f1) * lenth1 * _textureHigh;

            if (Isuv)
            {
                dr_u = (ue - us) * lenth1 * _textureWidth;
                dr_v = (ve - vs) * lenth1 * _textureHigh;
                us *= _textureWidth;
                vs *= _textureHigh;
            }

            int ilenth = (int)(lenth);
            int ux = 0;
            int uy = 0;
            Color c = cs;
            for (int i = (int)xs; i <= (int)(xe+0.5f); ++i)
            {
                c = cs;
                //z = zs + slop_z;
                if (Isuv)
                {
                    ux = Mathf.RoundToInt(us);
                    uy = Mathf.RoundToInt(vs);
                    c *= GetPixel(ux, uy);
                    us += dr_u;
                    vs += dr_v;
                }

                //深度测试
                _zBuffer.ZBufferTestFillColor((i), (int)(y), z, c);
                z += dr_z;
                cs.r += dr_x;
                cs.g += dg_x;
                cs.b += db_x;
            }
        }
    }

    private void LerpY(Color cs, Color ce, float zs, float ze, float x, float ys, float ye,
    float us, float vs, float ue, float ve, float lenth,
    LRender.FillType FillType, bool Isuv)
    {
        if (lenth > Test.ScreenWidth)
        {
            return;
        }

        

        if (FillType == LRender.FillType.FILLTYPE_WIREFRAME)
        {
            _zBuffer.ZBufferTestFillColor((int)(x), (int)ys, zs, cs);
            _zBuffer.ZBufferTestFillColor((int)(x), (int)(ys+lenth), ze, ce);
        }
        else
        {
            float lenth1 = 1 / lenth;
            if (lenth < 0.1f)
            {
                if (Isuv)
                {
                    int u = Mathf.RoundToInt(us * _textureWidth);
                    int v = Mathf.RoundToInt(vs * _textureHigh);
                    cs = cs * GetPixel(u, v);
                }
                _zBuffer.ZBufferTestFillColor((int)(x), (int)ys, zs, cs);
                return;
            }

            float dr_x = (ce.r - cs.r) * lenth1;
            float dg_x = (ce.g - cs.g) * lenth1;
            float db_x = (ce.b - cs.b) * lenth1;
            float dr_u = 0;
            float dr_v = 0;

            float dr_z = (ze - zs) * lenth1;
            //float slop_z = 0;
            float z = zs;

            if (Isuv)
            {
                dr_u = (ue - us) * lenth1 * _textureWidth;
                dr_v = (ve - vs) * lenth1 * _textureHigh;
                us *= _textureWidth;
                vs *= _textureHigh;
            }

            int ilenth = Mathf.RoundToInt(lenth);
            int ux = 0;
            int uy = 0;

            Color c = cs;
            for (int i = (int)ys; i <= (int)(ye+0.5f); ++i)
            {
                c = cs;
                if (Isuv)
                {
                    ux = Mathf.RoundToInt(us);
                    uy = Mathf.RoundToInt(vs);
                    c *= GetPixel(ux, uy);
                    us += dr_u;
                    vs += dr_v;
                }

                //深度测试
                _zBuffer.ZBufferTestFillColor((int)(x),(i), z, c);
                z += dr_z;
                cs.r += dr_x;
                cs.g += dg_x;
                cs.b += db_x;
            }
        }
    }

    public void beginY(LVector v1, LVector v2, LVector v3, LRender.TriangleType triangleType, LRender.FillType curFillType)
    {
        this._v1 = v1;
        this._v2 = v2;
        this._v3 = v3;

        _triangleType = triangleType;

        if (triangleType == LRender.TriangleType.TriangleType1)
        {
            dy = 1 / (v2.Pos.y - v1.Pos.y);

            //颜色插值数值
            slope_start_r = slop(v2.color.r, v1.color.r, dy);
            slope_start_g = slop(v2.color.g, v1.color.g, dy);
            slope_start_b = slop(v2.color.b, v1.color.b, dy);
            slope_end_r = slop(v2.color.r, v3.color.r, dy);
            slope_end_g = slop(v2.color.g, v3.color.g, dy);
            slope_end_b = slop(v2.color.b, v3.color.b, dy);

            if (curFillType == LRender.FillType.FILLTYPE_UV)
            {
                //uv插值系数
                slope_start_u = slop(_v2.u, _v1.u, dy); ;
                slope_start_v = slop(_v2.v, _v1.v, dy); ;
                slope_end_u = slop(_v2.u, _v3.u, dy); ;
                slope_end_v = slop(_v2.v, _v3.v, dy); ;
            }

        }
        else if (triangleType == LRender.TriangleType.TriangleType2)
        {
            dy = 1 / (v1.Pos.y - v2.Pos.y);

            //颜色插值数值
            slope_start_r = slop(v1.color.r, v2.color.r, dy);
            slope_start_g = slop(v1.color.g, v2.color.g, dy);
            slope_start_b = slop(v1.color.b, v2.color.b, dy);
            slope_end_r = slop(v3.color.r, v2.color.r, dy);
            slope_end_g = slop(v3.color.g, v2.color.g, dy);
            slope_end_b = slop(v3.color.b, v2.color.b, dy);

            if (curFillType == LRender.FillType.FILLTYPE_UV)
            {
                //UV插值系数
                slope_start_u = slop(_v1.u, _v2.u, dy); ;
                slope_start_v = slop(_v1.v, _v2.v, dy); ;
                slope_end_u = slop(_v3.u, _v2.u, dy); ;
                slope_end_v = slop(_v3.v, _v2.v, dy); ;
            }

        }
    }

    public void beginX(LVector v1, LVector v2, LVector v3, LRender.TriangleType triangleType, LRender.FillType curFillType)
    {
        this._v1 = v1;
        this._v2 = v2;
        this._v3 = v3;

        _triangleType = triangleType;

        if (triangleType == LRender.TriangleType.TriangleType1)
        {
            dy = 1 / (v2.Pos.x - v1.Pos.x);

            //颜色插值数值
            slope_start_r = slop(v2.color.r, v1.color.r, dy);
            slope_start_g = slop(v2.color.g, v1.color.g, dy);
            slope_start_b = slop(v2.color.b, v1.color.b, dy);
            slope_end_r = slop(v2.color.r, v3.color.r, dy);
            slope_end_g = slop(v2.color.g, v3.color.g, dy);
            slope_end_b = slop(v2.color.b, v3.color.b, dy);

            if (curFillType == LRender.FillType.FILLTYPE_UV)
            {
                //uv插值系数
                slope_start_u = slop(_v2.u, _v1.u, dy); ;
                slope_start_v = slop(_v2.v, _v1.v, dy); ;
                slope_end_u = slop(_v2.u, _v3.u, dy); ;
                slope_end_v = slop(_v2.v, _v3.v, dy); ;
            }

        }
        else if (triangleType == LRender.TriangleType.TriangleType2)
        {
            dy = 1 / (v1.Pos.x - v2.Pos.x);

            //颜色插值数值
            slope_start_r = slop(v1.color.r, v2.color.r, dy);
            slope_start_g = slop(v1.color.g, v2.color.g, dy);
            slope_start_b = slop(v1.color.b, v2.color.b, dy);
            slope_end_r = slop(v3.color.r, v2.color.r, dy);
            slope_end_g = slop(v3.color.g, v2.color.g, dy);
            slope_end_b = slop(v3.color.b, v2.color.b, dy);

            if (curFillType == LRender.FillType.FILLTYPE_UV)
            {
                //UV插值系数
                slope_start_u = slop(_v1.u, _v2.u, dy); ;
                slope_start_v = slop(_v1.v, _v2.v, dy); ;
                slope_end_u = slop(_v3.u, _v2.u, dy); ;
                slope_end_v = slop(_v3.v, _v2.v, dy); ;
            }

        }
    }

    public void end()
    {
        _v1 = null;
        _v2 = null;
        _v3 = null;

        dr_start = 0;
        dg_start = 0;
        db_start = 0;

        dr_end = 0;
        dg_end = 0;
        db_end = 0;

        dy = 0.0f;

        slope_start_u = 0.0f;
        slope_start_v = 0.0f;
        slope_end_u = 0.0f;
        slope_end_v = 0.0f;

        du_strat = 0.0f;
        dv_strat = 0.0f;
        du_end = 0.0f;
        dv_end = 0.0f;
    }

    public void ColorLerpCalculateY(float xe, float ze, float xs, float zs, float y, LRender.FillType FillType, bool IsUpdate = false)
    {
        if (IsUpdate)
        {
            dr_start = 0;
            dg_start = 0;
            db_start = 0;

            dr_end = 0;
            dg_end = 0;
            db_end = 0;
        }

        float lenth = Mathf.Abs(xe - xs);

        if (_triangleType == LRender.TriangleType.TriangleType1)
        {
            //if (lenth < 0.1f)
            //{
            //    Color c = _v2.color;
            //    if (FillType == LRender.FillType.FILLTYPE_UV)
            //    {
            //        c *= GetPixel((int)(_v2.u * _textureWidth), (int)(_v2.v * _textureHigh));
            //        _zBuffer.ZBufferTestFillColor((int)xs, (int)y, zs, c);
            //    }
            //}
            //else
            //{
                Color cs = _v1.color;
                Color ce = _v3.color;
                cs.r += dr_start;
                cs.g += dg_start;
                cs.b += db_start;

                ce.r += dr_end;
                ce.g += dg_end;
                ce.b += db_end;

                float us = _v1.u + du_strat;
                float vs = _v1.v + dv_strat;
                float ue = _v3.u + du_end;
                float ve = _v3.v + dv_end;

                LerpX(cs, ce, zs, ze, y, xs, xe, us, vs, ue, ve, lenth, FillType, FillType == LRender.FillType.FILLTYPE_UV);
            //}
        }
        else if (_triangleType == LRender.TriangleType.TriangleType2)
        {
            //if (lenth < 0.9999999f)
            //{
            //    Color c = _v2.color;
            //    if (FillType == LRender.FillType.FILLTYPE_UV)
            //    {
            //        c *= GetPixel((int)(_v2.u * _textureWidth), (int)(_v2.v * _textureHigh));
            //        _zBuffer.ZBufferTestFillColor((int)xs, (int)y, zs, c);
            //    }
            //}
            //else
            //{
                Color cs = Color.white;
                Color ce = Color.white;
                cs.r = (_v2.color.r + dr_start);
                cs.g = (_v2.color.g + dg_start);
                cs.b = (_v2.color.b + db_start);

                ce.r = (_v2.color.r + dr_end);
                ce.g = (_v2.color.g + dg_end);
                ce.b = (_v2.color.b + db_end);

                float us = _v2.u + du_strat;
                float vs = _v2.v + dv_strat;
                float ue = _v2.u + du_end;
                float ve = _v2.v + dv_end;

                LerpX(cs, ce, zs, ze, y, xs, xe, us, vs, ue, ve, lenth, FillType, FillType == LRender.FillType.FILLTYPE_UV);

            //}
        }

        dr_start += slope_start_r;
        dg_start += slope_start_g;
        db_start += slope_start_b;
        dr_end += slope_end_r;
        dg_end += slope_end_g;
        db_end += slope_end_b;

        du_strat += slope_start_u;
        dv_strat += slope_start_v;
        du_end += slope_end_u;
        dv_end += slope_end_v;
    }

    public void ColorLerpCalculateX(float ye, float ze, float ys, float zs, float x, LRender.FillType FillType, bool IsUpdate = false)
    {
        if (IsUpdate)
        {
            dr_start = 0;
            dg_start = 0;
            db_start = 0;

            dr_end = 0;
            dg_end = 0;
            db_end = 0;
        }

        float lenth = Mathf.Abs(ye - ys);

        if (_triangleType == LRender.TriangleType.TriangleType1)
        {
            //if (lenth < 0.1f)
            //{
            //    Color c = _v2.color;
            //    if (FillType == LRender.FillType.FILLTYPE_UV)
            //    {
            //        c *= GetPixel((int)(_v2.u * _textureWidth), (int)(_v2.v * _textureHigh));
            //        _zBuffer.ZBufferTestFillColor((int)x, (int)ys, zs, c);
            //    }
            //}
            //else
            //{
                Color cs = _v1.color;
                Color ce = _v3.color;
                cs.r += dr_start;
                cs.g += dg_start;
                cs.b += db_start;

                ce.r += dr_end;
                ce.g += dg_end;
                ce.b += db_end;

                float us = _v1.u + du_strat;
                float vs = _v1.v + dv_strat;
                float ue = _v3.u + du_end;
                float ve = _v3.v + dv_end;

                LerpY(cs, ce, zs, ze, x, ys, ye, us, vs, ue, ve, lenth, FillType, FillType == LRender.FillType.FILLTYPE_UV);
            //}
        }
        else if (_triangleType == LRender.TriangleType.TriangleType2)
        {
            //if (lenth < 0.9999999f)
            //{
            //    Color c = _v2.color;
            //    if (FillType == LRender.FillType.FILLTYPE_UV)
            //    {
            //        c *= GetPixel((int)(_v2.u * _textureWidth), (int)(_v2.v * _textureHigh));
            //        _zBuffer.ZBufferTestFillColor((int)x, (int)ys, zs, c);
            //    }
            //}
            //else
            //{
                Color cs = Color.white;
                Color ce = Color.white;
                cs.r = (_v2.color.r + dr_start);
                cs.g = (_v2.color.g + dg_start);
                cs.b = (_v2.color.b + db_start);

                ce.r = (_v2.color.r + dr_end);
                ce.g = (_v2.color.g + dg_end);
                ce.b = (_v2.color.b + db_end);

                float us = _v2.u + du_strat;
                float vs = _v2.v + dv_strat;
                float ue = _v2.u + du_end;
                float ve = _v2.v + dv_end;

                LerpY(cs, ce, zs, ze, x, ys, ye, us, vs, ue, ve, lenth, FillType, FillType == LRender.FillType.FILLTYPE_UV);

            //}
        }

        dr_start += slope_start_r;
        dg_start += slope_start_g;
        db_start += slope_start_b;
        dr_end += slope_end_r;
        dg_end += slope_end_g;
        db_end += slope_end_b;

        du_strat += slope_start_u;
        dv_strat += slope_start_v;
        du_end += slope_end_u;
        dv_end += slope_end_v;
    }

    public LVector CalculateMiddlePointY(LVector va, LVector vb, LVector vc, LRender.FillType fillType)
    {
        LVector Xmiddle = new LVector();
        Color CMiddle = Color.white;

        Xmiddle.Pos.y = va.Pos.y;
        Xmiddle.Pos.w = va.Pos.w;
        Xmiddle.Nor = va.Nor;

        float dy = 1 / (vc.Pos.y - vb.Pos.y);
        float ey = vc.Pos.y - Xmiddle.Pos.y;

        Xmiddle.Pos.x = vc.Pos.x + ey * (vb.Pos.x - vc.Pos.x) * dy;
        Xmiddle.Pos.z = vc.Pos.z + ey * (vb.Pos.z - vc.Pos.z) * dy;
        CMiddle.r = vc.color.r + ey * (vb.color.r - vc.color.r) * dy;
        CMiddle.g = vc.color.g + ey * (vb.color.g - vc.color.g) * dy;
        CMiddle.b = vc.color.b + ey * (vb.color.b - vc.color.b) * dy;
        Xmiddle.color = CMiddle;

        if (fillType == BaseLRender.FillType.FILLTYPE_UV)
        {
            Xmiddle.u = vc.u + ey * (vb.u - vc.u) * dy;
            Xmiddle.v = vc.v + ey * (vb.v - vc.v) * dy;
        }

        return Xmiddle;
    }

    public LVector CalculateMiddlePointX(LVector va, LVector vb, LVector vc, LRender.FillType fillType)
    {
        LVector Xmiddle = new LVector();
        Color CMiddle = Color.white;

        Xmiddle.Pos.x = va.Pos.x;
        Xmiddle.Pos.w = va.Pos.w;
        Xmiddle.Nor = va.Nor;

        float dy = 1 / (vc.Pos.x - vb.Pos.x);
        float ey = vc.Pos.x - Xmiddle.Pos.x;

        Xmiddle.Pos.y = vc.Pos.y + ey * (vb.Pos.y - vc.Pos.y) * dy;
        Xmiddle.Pos.z = vc.Pos.z + ey * (vb.Pos.z - vc.Pos.z) * dy;
        CMiddle.r = vc.color.r + ey * (vb.color.r - vc.color.r) * dy;
        CMiddle.g = vc.color.g + ey * (vb.color.g - vc.color.g) * dy;
        CMiddle.b = vc.color.b + ey * (vb.color.b - vc.color.b) * dy;
        Xmiddle.color = CMiddle;

        if (fillType == BaseLRender.FillType.FILLTYPE_UV)
        {
            Xmiddle.u = vc.u + ey * (vb.u - vc.u) * dy;
            Xmiddle.v = vc.v + ey * (vb.v - vc.v) * dy;
        }

        return Xmiddle;
    }

    public void Clear()
    {
        _zBuffer.ClearZBuffer();
    }

    //画横线
    public void DrawLineX(LVector startp,LVector endp, BaseLRender.FillType fillType)
    {
        LerpX(startp.color, endp.color, startp.Pos.z, endp.Pos.z,
            startp.Pos.y, startp.Pos.x, endp.Pos.x, 
            startp.u, startp.v, endp.u, endp.v, Mathf.Abs(endp.Pos.x - startp.Pos.x),
            fillType, fillType == BaseLRender.FillType.FILLTYPE_UV);
    }

    //画竖线
    public void DrawLineY(LVector startp, LVector endp, BaseLRender.FillType fillType)
    {
        LerpY(startp.color, endp.color, startp.Pos.z, endp.Pos.z,
            startp.Pos.x, startp.Pos.y, endp.Pos.y,
            startp.u, startp.v, endp.u, endp.v, Mathf.Abs(endp.Pos.y - startp.Pos.y),
            fillType, fillType == BaseLRender.FillType.FILLTYPE_UV);
    }
}
