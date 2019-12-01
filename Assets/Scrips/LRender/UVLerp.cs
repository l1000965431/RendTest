using UnityEngine;
using System.Collections;

//纹理坐标运算（弃用 和ColorLerp合并）
public class UVLerp
{
    Texture2D _t;

    int _textureWidth;
    int _textureHigh;

    LRender.TriangleType _triangleType;

    public void SetTexture(Texture2D t)
    {
        this._t = t;
        _textureWidth = t.width;
        _textureHigh = t.height;
    }

    private Color GetPixel(int x, int y)
    {
        if (x < 0 || y < 0 || x > _textureWidth || y > _textureHigh)
        {
            return Color.blue;
        }

        return _t.GetPixel(x, y);
    }

    public void UVLerpCalculate(LVector v1, LVector v2, LVector v3)
    {

    }

}
