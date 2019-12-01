using UnityEngine;
using System.Collections;

/**
 向量
**/
public struct LVector4 {

    public float x;
    public float y;
    public float z;
    public float w;

    public override string ToString()//重写ToSting方法
    {
        string strResult = "(" + this.x + "," + this.y+ "," + this.z + ","+this.w+")";
        return strResult;
    }

    public LVector4(LVector4 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
        this.w = v.w;
    }

    public LVector4(float x,float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = 1.0f;
    }

    public LVector4(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
        this.w = 1.0f;
    }

    public static LVector4[] Vector3ArrayToLVector4Array(Vector3[] v)
    {
        LVector4[] lv = new LVector4[v.Length];
        for(int i = 0; i < lv.Length; ++i)
        {
            lv[i] = new LVector4(v[i]);
        }

        return lv;
    }

    // 向量加法
    public LVector4 Addition(float x, float y, float z)
    {
        return new LVector4(this.x + x, this.y + y, this.z + z);
    }

    public LVector4 Addition(LVector4 v)
    {
        return Addition(v.x, v.y, v.z);
    }

    public void AdditionSelf(float x,float y, float z)
    {
        this.x += x;
        this.y += y;
        this.z += z;
    }

    public void AdditionSelf(LVector4 v)
    {
        AdditionSelf(v.x, v.y, v.z);
    }

    //向量点乘
    public float Dot(float x, float y ,float z)
    {
        return this.x * x + this.y * y + this.z * z;
    }

    public float SqrMagnitude()
    {
        return (this.x * this.x + this.y * this.y + this.z * this.z);
    }

    public float Dot(LVector4 v)
    {
        return Dot(v.x,v.y,v.z);
    }

    public LVector4 Normalize()
    {
        return this/Mathf.Sqrt(this.SqrMagnitude());
    }

    public LVector4 Multiplication(float f)
    {
        return new LVector4(this.x * f, this.y * f, this.z * f);
    }

    public LVector4 Cross(float x, float y, float z)
    {
        return new LVector4(this.y * z - this.z * y, this.z * x - this.x * z, this.x * y - this.y * x);
    }
    
    public LVector4 Cross(LVector4 v)
    {
        return Cross(v.x, v.y, v.z);
    }

    public static LVector4 operator + (LVector4 v1, LVector4 v2)
    {
        return v1.Addition(v2);
    }

    public static LVector4 operator -(LVector4 v1, LVector4 v2)
    {
        return v1.Addition(-v2.x,-v2.y,-v2.z);
    }

    public static LVector4 operator *(LVector4 v1, float d)
    {
        return v1.Multiplication(d);
    }

    public static LVector4 operator /(LVector4 v1, float d)
    {
        return v1.Multiplication(1/d);
    }

    public void Clear()
    {
        x = 0.0f;
        y = 0.0f;
        z = 0.0f;
        w = 1.0f;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
