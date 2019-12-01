using UnityEngine;
using System.Collections;

//模型
public class LModel
{
    //模型定点本地坐标数组
    LVector[] _modelVector;

    //模型定点渲染坐标数组
    LVector[] _modelRenderVector;

    //平面索引
    LPlaneIndex _LPlaneIndex;
    //模型材质
    LMaterial _material;

    //模型位置
    LVector4 _mPos;
    //模型旋转
    LVector4 _Angel;

    //模型旋转矩阵
    LMatrix4X4 _AngelX;
    LMatrix4X4 _AngelY;
    LMatrix4X4 _AngelZ;
    //旋转矩阵结果
    LMatrix4X4 _r;

    int _VectorNum;

    int _PlaneNum;

    public static LModel CreateLModel(LVector[] v, int[] index, int VectorNum, int PlaneNum, string TexturePath, bool IsNor = false)
    {
        Texture2D t = UnityEngine.Resources.Load<Texture2D>(TexturePath);

        return CreateLModel(v, index, VectorNum, PlaneNum, t, IsNor);
    }

    public static LModel CreateLModel(LVector[] v, int[] index, int VectorNum, int PlaneNum, Texture TexturePath, bool IsNor = false)
    {
        return new LModel(v,index,VectorNum,PlaneNum,TexturePath,IsNor);
    }

    private LModel(LVector[] v, int[] index, int VectorNum, int PlaneNum, Texture TexturePath, bool IsNor = false)
    {
        _VectorNum = VectorNum;
        _PlaneNum = PlaneNum;
        _modelVector = v;
        _modelRenderVector = new LVector[VectorNum];
        _mPos = new LVector4();
        _Angel = new LVector4();
        _AngelX = new LMatrix4X4();
        _AngelY = new LMatrix4X4();
        _AngelZ = new LMatrix4X4();
        _r = new LMatrix4X4();

        _AngelX.UnitMatrix4X4();
        _AngelY.UnitMatrix4X4();
        _AngelZ.UnitMatrix4X4();
        _r.UnitMatrix4X4();

        _LPlaneIndex = new LPlaneIndex(PlaneNum, VectorNum, ref _modelVector, index);

        //计算定点法向量
        if (IsNor)
        {
            VectorNor();
        }

        //_modelRenderVector = (LVector[])_modelVector.Clone();

        for (int i = 0; i < VectorNum; ++i)
        {
            _modelRenderVector[i] = new LVector();
            _modelRenderVector[i].u = _modelVector[i].u;
            _modelRenderVector[i].v = _modelVector[i].v;
            _modelRenderVector[i].color = _modelVector[i].color;
            _modelRenderVector[i].Nor = _modelVector[i].Nor;
        }

        _material = new LMaterial(Color.white, Color.white, 1.0f, 5.0f, 5.0f);
        _material.SetTexture(TexturePath);
    }

    public void SetMaterial(LMaterial materia)
    {
        _material = null;
        _material = materia;
    }

    public LMaterial GetMaterial()
    {
        return _material;
    }

    public void SetVector(int index, LVector v)
    {
        _modelRenderVector[index] = null;
        _modelRenderVector[index] = v;
    }

    public void SetVector(LVector[] v)
    {
        _modelRenderVector = v;
    }

    public LVector[] GetVector()
    {
        return _modelRenderVector;
    }

    public LVector[] GetModelVector()
    {
        return _modelVector;
    }

    public LVector GetVector(int index)
    {
        return _modelRenderVector[index];
    }

    public int GetVectorNum()
    {
        return _VectorNum;
    }

    public void SetPos(LVector4 v)
    {
        _mPos = v;
    }

    public void SetRotation(float alphax, float alphay, float alphaz)
    {
        _Angel.x += alphax;
        _Angel.y += alphay;
        _Angel.z += alphaz;

        _AngelX.UnitMatrix4X4();
        _AngelX.SetRotaionX(alphax);

        _AngelY.UnitMatrix4X4();
        _AngelY.SetRotaionY(alphay);

        _AngelZ.UnitMatrix4X4();
        _AngelZ.SetRotaionZ(alphaz);


        LMatrix4X4 a = _AngelX.MatrixMultiply(_AngelY);
        LMatrix4X4 b = a.MatrixMultiply(_AngelZ);
        _r.UnitMatrix4X4();
        _r = b.MatrixTranspose();

        for (int i = 0; i < _VectorNum; ++i)
        {
            _modelVector[i].Pos = _r.MatrixMultiply(_modelVector[i].Pos);
            _modelVector[i].Nor = _r.MatrixMultiply(_modelVector[i].Nor);
        }
    }

    public LPlaneIndex getPlaneIndex()
    {
        return _LPlaneIndex;
    }

    //将本地定点数组变换到世界坐标系中
    public void ToWorld()
    {
        for (int i = 0; i < _VectorNum; ++i)
        {
            _modelRenderVector[i].Clear();

            //位置坐标变换
            _modelRenderVector[i].Pos = _modelVector[i].Pos.Addition(_mPos);

            //法线变换
            _modelRenderVector[i].Nor = _modelVector[i].Nor.Addition(_mPos);
            //_modelRenderVector[i].Nor = _r.MatrixMultiply(_modelRenderVector[i].Nor);
        }
    }

    public void ToScreen()
    {
        float Width = Test.ScreenWidth / 2;
        float Hight = Test.ScreenHight / 2;

        for (int i = 0; i < _VectorNum; ++i)
        {
            _modelRenderVector[i].Pos.x *= Width;
            _modelRenderVector[i].Pos.y *= Hight;

            _modelRenderVector[i].Pos.x += Width;
            _modelRenderVector[i].Pos.y += Hight;
        }
    }

    public void BackCullFace(ref LCamera c)
    {
        for (int i = 0; i < _PlaneNum; ++i)
        {           
            _LPlaneIndex.BackCullFace(i, ref _modelRenderVector);
        }
    }

    //计算定点法向量
    private void VectorNor()
    {
        //缓存每个定点法向量的和
        LVector4[] Nor = new LVector4[_VectorNum];
        for (int i = 0; i < _VectorNum; ++i)
        {
            Nor[i] = new LVector4();
        }

        //缓存每个定点法向量的相加次数
        int[] AddNum = new int[_VectorNum];

        //根据索引计算法向量
        for (int i = 0; i < _PlaneNum; i += 3)
        {
            int index1 = _LPlaneIndex.getPointIndex(i)[0];
            int index2 = _LPlaneIndex.getPointIndex(i)[1];
            int index3 = _LPlaneIndex.getPointIndex(i)[2];

            LVector4 a = _modelVector[index1].Pos - _modelVector[index2].Pos;
            LVector4 b = _modelVector[index1].Pos - _modelVector[index3].Pos;
            LVector4 n = a.Cross(b);

            Nor[index1] += n;
            Nor[index2] += n;
            Nor[index3] += n;

            AddNum[index1] += 1;
            AddNum[index2] += 1;
            AddNum[index3] += 1;
        }


        for (int i = 0; i < _VectorNum; ++i)
        {
            _modelVector[i].Nor = (Nor[i].Normalize()) / AddNum[i];
        }

    }

    public void Clear()
    {
        for (int i = 0; i < _VectorNum; ++i)
        {
            _modelRenderVector[i].u = _modelVector[i].u;
            _modelRenderVector[i].v = _modelVector[i].v;
            _modelRenderVector[i].color = _modelVector[i].color;
            _modelRenderVector[i].Nor = _modelVector[i].Nor;
        }
    }
}
