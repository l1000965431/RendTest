using UnityEngine;
using System.Collections;
using System;
//Z-Buffer 测试
public class ZBuffer
{
    float[,] _ZBuffer;

    int _ScreenWidth;
    int _ScreenHight;

    Texture2D _windowhandle;

    public ZBuffer(int ScreenWidth, int ScreenHight, Texture2D windowhandle)
    {
        _ZBuffer = new float[ScreenWidth, ScreenHight];

        _ScreenWidth = ScreenWidth;
        _ScreenHight = ScreenHight;
        _windowhandle = windowhandle;
    }

    public void ClearZBuffer()
    {
        Array.Clear(_ZBuffer, 0, _ScreenWidth * _ScreenHight);
    }

    //ZbufferTest xs:起点 xe:终点 colors:颜色数组 return:长度
    public void ZBufferTestPixels(int xs, int ys, float zxs, int xe, float zxe, Color calutcolors)
    {
        int lengh = Mathf.Abs(xs - xe);
        float dr_z = (zxe - zxs) / lengh;
        float slop_z = 0;

        float z = 0;
        for (int i = 0; i < lengh; ++i)
        {
            z = zxs + slop_z;
            slop_z += dr_z;

        }
    }

    public void ZBufferTestPixel(int xs, int ys, float zxs, Color calutcolors)
    {
        ZBufferTestFillColor(xs, ys, zxs, calutcolors);
    }

    public void ZBufferTestFillColor(int x, int y, float z, Color calutcolors)
    {
        if(x >= _ScreenWidth || y >= _ScreenHight || x < 0 || y < 0 || z < 0)
        {
            return;
        }

        float zcatch = _ZBuffer[x, y];

        if (z > zcatch)
        {
            _ZBuffer[x, y] = z;
            _windowhandle.SetPixel(x, y, calutcolors);
        }
    }
}
