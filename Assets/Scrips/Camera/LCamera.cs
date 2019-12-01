using UnityEngine;
using System.Collections;

public class LCamera
{
    //相机世界位置
    LVector4 _Pos;

    LVector4 _Angel;

    //相机位置矩阵
    LMatrix4X4 _PosMatrix;

    //相机朝向矩阵
    LMatrix4X4 _AngelX;
    LMatrix4X4 _AngelY;
    LMatrix4X4 _AngelZ;

    public LMatrix4X4 ViewMatrix;
    public LMatrix4X4 ProjectionMatrix;

    //远平面距离
    float _fFactor;
    //近平面距离
    float _near;
    //屏幕的宽高比
    float _aspect;
    //视角
    float _fov;

    static LCamera _sLCamera;

    public static float getNear()
    {
        return _sLCamera._near;
    }

    public LCamera(float near, float fFactor, float fov, int ScreenWidth, int ScreenHight)
    {
        _sLCamera = this;
        _Pos = new LVector4();
        _Angel = new LVector4();
        _PosMatrix = new LMatrix4X4();
        _AngelX = new LMatrix4X4();
        _AngelY = new LMatrix4X4();
        _AngelZ = new LMatrix4X4();
        ViewMatrix = new LMatrix4X4();
        ProjectionMatrix = new LMatrix4X4();

        _PosMatrix.UnitMatrix4X4();
        _AngelX.UnitMatrix4X4();
        _AngelY.UnitMatrix4X4();
        _AngelZ.UnitMatrix4X4();
        ViewMatrix.UnitMatrix4X4();

        //初始化投影矩阵
        this._near = near;
        this._fFactor = fFactor;
        this._aspect = (float)ScreenWidth / (float)ScreenHight;
        this._fov = fov;

        float Rad = Mathf.Deg2Rad * _fov;

        this.ProjectionMatrix[1, 1] = 1.0f / (Mathf.Tan(Rad / 2.0f) * _aspect);
        this.ProjectionMatrix[2, 2] = 1.0f / Mathf.Tan(Rad / 2.0f);
        this.ProjectionMatrix[3, 3] = -((_fFactor + _near) / (_fFactor - _near));
        this.ProjectionMatrix[3, 4] = (-2.0f * _fFactor * _near) / (_fFactor - _near);
        this.ProjectionMatrix[4, 3] = -1.0f;
    }

    public void SetCameraPos(LVector4 v)
    {
        LCameraWalk(v);
        _Pos.AdditionSelf(v);
    }

    //相机平移
    public void LCameraWalk(LVector4 v)
    {
        _PosMatrix.UnitMatrix4X4();
        _PosMatrix.GetTranslationMatrix(-(v.x + _Pos.x), -(v.y + _Pos.y), -(v.z + _Pos.z));
    }
    //相机旋转 只有左右和上下旋转 没有翻滚
    public void LCameraRoation(float alphax, float alphay, float alphaz)
    {
        _Angel.x += alphax;
        _Angel.y += alphay;
        _Angel.z += alphaz;

        _AngelX.UnitMatrix4X4();
        _AngelX.SetRotaionX(-_Angel.x);

        _AngelY.UnitMatrix4X4();
        _AngelY.SetRotaionY(-_Angel.y);

        _AngelZ.UnitMatrix4X4();
        _AngelZ.SetRotaionZ(-_Angel.z);
    }

    //获得相机当前矩阵
    public void SetLCameraViewMatrix()
    {
        ViewMatrix.UnitMatrix4X4();
        LMatrix4X4 a = ViewMatrix.MatrixMultiply(_PosMatrix);
        LMatrix4X4 b = a.MatrixMultiply(_AngelY);
        LMatrix4X4 c = b.MatrixMultiply(_AngelX);
        LMatrix4X4 d = c.MatrixMultiply(_AngelZ);
        ViewMatrix = d.MatrixTranspose();
    }

    public LVector4 EyeDirection(LVector4 v)
    {
        return _Pos - v;
    }
}
