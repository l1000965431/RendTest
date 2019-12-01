using System.Collections.Generic;
using UnityEngine;

//渲染流程控制
public class LRenderSys
{
    //测试用 渲染模型的时候换成模型列表
    List<LModel> _m;

    //UV和颜色渲染
    BaseLRender _LRender;

    //线框渲染
    BaseLRender _WireframeRender;

    //相机
    LCamera _lCamera;

    //阳光
    LLight _Sun;

    bool _IsUpdate;

    //是否开启光照
    public bool Islight;

    BaseLRender.FillType Filltype;

    private Texture2D _windowhandle;

    ZBuffer _zbuffer;

    public LRenderSys(Renderer r)
    {
        _windowhandle = new Texture2D(Test.ScreenWidth, Test.ScreenHight, TextureFormat.ARGB32, false);
        r.material.mainTexture = _windowhandle;

        _zbuffer = new ZBuffer(Test.ScreenWidth, Test.ScreenHight, _windowhandle);

        _LRender = new BCLRender(_zbuffer);
        _WireframeRender = new LRender(_zbuffer);

        _m = new List<LModel>();

        _lCamera = new LCamera(1, 1000, 90, Test.ScreenWidth, Test.ScreenHight);
        _lCamera.SetCameraPos(new LVector4(0, 0, 0));
        _lCamera.SetLCameraViewMatrix();

        _Sun = new LLight(new LVector4(100, 30, 100), new LVector4(0, 0, 0));

        _IsUpdate = true;

        Islight = false;

        Filltype = BaseLRender.FillType.FILLTYPE_UV;
    }

    public void SetLight(bool Light)
    {
        Islight = Light;
    }

    public bool GetLight()
    {
        return Islight;
    }

    public void SetFillType(BaseLRender.FillType filltype)
    {
        Filltype = filltype;
    }

    public BaseLRender.FillType GetCurFillType()
    {
        return Filltype;
    }

    public void SetUpdate()
    {
        _IsUpdate = true;
    }

    public bool IsUpdate()
    {
        return _IsUpdate;
    }

    public void AddModelList(LModel model)
    {
        _m.Add(model);

    }

    public void CameraWalk(LVector4 v)
    {
        _lCamera.SetCameraPos(v);
    }

    public void CCameraRoation(float alphax, float alphay, float alphaz)
    {
        _lCamera.LCameraRoation(alphax, alphay, alphaz);
    }

    public void CCameraTransform(LVector4 pos, LVector4 alpha)
    {
        _lCamera.SetCameraPos(pos);
        _lCamera.LCameraRoation(alpha.x, alpha.y, alpha.z);
        _lCamera.SetLCameraViewMatrix();

        this.SetUpdate();
    }

    public void Render()
    {
        if (_IsUpdate)
        {
            Clear();

            foreach (LModel model in _m)
            {
                //世界坐标变换
                WorldTranform(model);

                //光照计算
                CalutLight(model);

                //相机矩阵换
                CameraTranform(model);

                //屏幕坐标转换
                ScreenTranform(model);

                //背面剔除
                BackCullFace(model);

                //_WireframeRender.SetUVTexture(model.GetMaterial());
                //_WireframeRender.SetFillTyp(Filltype);
                //_WireframeRender.LRenderMode(model.GetVector(), model.getPlaneIndex());

                if (Filltype == BaseLRender.FillType.FILLTYPE_WIREFRAME)
                {
                    _WireframeRender.SetUVTexture(model.GetMaterial());
                    _WireframeRender.SetFillTyp(Filltype);
                    _WireframeRender.LRenderMode(model.GetVector(), model.getPlaneIndex());
                }
                else
                {
                    _LRender.SetUVTexture(model.GetMaterial());
                    _LRender.SetFillTyp(Filltype);
                    _LRender.LRenderMode(model.GetVector(), model.getPlaneIndex());
                }

            }

            Update();

            if (!Test.RenderUpdate)
            {
                _IsUpdate = false;
            }

        }
    }

    //相机变换
    private void CameraTranform(LModel m)
    {
        int VectorNum = m.GetVectorNum();
        LVector[] v = m.GetVector();

        float z;
        for (int i = 0; i < VectorNum; ++i)
        {
            v[i].Pos = _lCamera.ViewMatrix.MatrixMultiply(v[i].Pos);

            //保留Z 不做透视变换
            z = v[i].Pos.z;

            v[i].Pos = _lCamera.ProjectionMatrix.MatrixMultiply(v[i].Pos);
            v[i].Pos.z = z;

            if (!CuttingSsurface(ref v[i].Pos))
            {
                v[i].SetEnabel(false);
                continue;
            }

            v[i].SetEnabel(true);

            if (v[i].Pos.w != 0.0f)
            {
                v[i].Pos.x = v[i].Pos.x / (-v[i].Pos.w);
                v[i].Pos.y = v[i].Pos.y / (-v[i].Pos.w);
            }

            v[i].Pos.z = 1.0f / z;
            v[i].Pos.w = -v[i].Pos.w;

            v[i].u = m.GetModelVector()[i].u;
            v[i].v = m.GetModelVector()[i].v;
        }

        m.SetVector(v);
    }

    //世界坐标变换
    private void WorldTranform(LModel m)
    {
        m.ToWorld();
    }

    private void ScreenTranform(LModel m)
    {
        m.ToScreen();
    }

    private void BackCullFace(LModel m)
    {
        m.BackCullFace(ref _lCamera);
    }

    //裁剪空间剪裁
    private bool CuttingSsurface(ref LVector4 v)
    {
        float wmin = v.w;
        float wmax = -v.w;

        if (v.x < wmin || v.x > wmax)
        {
            return false;
        }

        if (v.y < wmin || v.y > wmax)
        {
            return false;
        }

        if (v.z < wmin || v.z > wmax)
        {
            return false;
        }

        return true;

    }

    private void CalutLight(LModel m)
    {
        for (int i = 0; i < m.GetVectorNum(); ++i)
        {
            if (!Islight)
            {
                m.GetVector()[i].color = m.GetModelVector()[i].color;
            }
            else
            {
                LVector4 lightDirection = _Sun.LightDirection(m.GetVector()[i].Pos);
                LVector4 eyeDirection = _lCamera.EyeDirection(m.GetVector()[i].Pos);
                _Sun.LLightColor(ref m.GetModelVector()[i], ref m.GetVector()[i], lightDirection, eyeDirection, m.GetMaterial());
            }
        }
    }

    public void ModelClear()
    {
        foreach (LModel model in _m)
        {
            model.Clear();
        }
    }

    public void ListClear()
    {
        _m.Clear();
    }


    private void Clear()
    {
        _windowhandle.Resize(Test.ScreenWidth, Test.ScreenHight, TextureFormat.ARGB32, false);
        _zbuffer.ClearZBuffer();
    }

    private void Update()
    {
        _windowhandle.Apply(false);
    }

}