using UnityEngine;
using System.Collections;

/**
 矩阵
**/
public struct LMatrix4X4
{
    public float m00;
    public float m01;
    public float m02;
    public float m03;
    public float m10;
    public float m11;
    public float m12;
    public float m13;
    public float m20;
    public float m21;
    public float m22;
    public float m23;
    public float m30;
    public float m31;
    public float m32;
    public float m33;


    public float this[int row, int column]
    {
        get
        {
            if (row <= 0 || column <= 0)
            {
                return 0.0f;
            }


            return getRowCloumn(row, column);

        }
        set
        {
            if (row <= 0 || column <= 0)
            {
                return;
            }

            setRowCloumn(row, column, value);
        }
    }

    public override string ToString()//重写ToSting方法
    {
        string strResult = "";
        for (int i = 1; i <= 4; ++i)
        {
            strResult += System.Environment.NewLine + "[";
            for (int j = 1; j <= 4; ++j)
            {
                if (j == 4)
                {
                    strResult += this[i, j];
                }
                else
                {
                    strResult += this[i, j] + ",";
                }
            }
            strResult += "]" + System.Environment.NewLine;
        }

        return strResult;
    }

    //矩阵单位化
    public void UnitMatrix4X4()
    {
        this.Clear();
        this[1, 1] = 1;
        this[2, 2] = 1;
        this[3, 3] = 1;
        this[4, 4] = 1;
    }

    private void setRowCloumn(int row, int cloumn, float value)
    {
        switch (row)
        {
            case 1:
                {
                    switch (cloumn)
                    {
                        case 1:
                            m00 = value;
                            break;
                        case 2:
                            m01 = value;
                            break;
                        case 3:
                            m02 = value;
                            break;
                        case 4:
                            m03 = value;
                            break;
                    }
                }
                break;
            case 2:
                {
                    switch (cloumn)
                    {
                        case 1:
                            m10 = value;
                            break;
                        case 2:
                            m11 = value;
                            break;
                        case 3:
                            m12 = value;
                            break;
                        case 4:
                            m13 = value;
                            break;
                    }
                }
                break;
            case 3:
                {
                    switch (cloumn)
                    {
                        case 1:
                            m20 = value;
                            break;
                        case 2:
                            m21 = value;
                            break;
                        case 3:
                            m22 = value;
                            break;
                        case 4:
                            m23 = value;
                            break;
                    }
                }
                break;
            case 4:
                {
                    switch (cloumn)
                    {
                        case 1:
                            m30 = value;
                            break;
                        case 2:
                            m31 = value;
                            break;
                        case 3:
                            m32 = value;
                            break;
                        case 4:
                            m33 = value;
                            break;
                    }
                }
                break;
        }
    }

    private float getRowCloumn(int row, int cloumn)
    {
        switch (row)
        {
            case 1:
                {
                    switch (cloumn)
                    {
                        case 1:
                            return m00;
                        case 2:
                            return m01;
                        case 3:
                            return m02;
                        case 4:
                            return m03;
                    }
                }
                break;
            case 2:
                {
                    switch (cloumn)
                    {
                        case 1:
                            return m10;
                        case 2:
                            return m11;
                        case 3:
                            return m12;
                        case 4:
                            return m13;
                    }
                }
                break;
            case 3:
                {
                    switch (cloumn)
                    {
                        case 1:
                            return m20;
                        case 2:
                            return m21;
                        case 3:
                            return m22;
                        case 4:
                            return m23;
                    }
                }
                break;
            case 4:
                {
                    switch (cloumn)
                    {
                        case 1:
                            return m30;
                        case 2:
                            return m31;
                        case 3:
                            return m32;
                        case 4:
                            return m33;
                    }
                }
                break;
        }

        return 0;
    }

    public void Clear()
    {
        m00 = 0.0f;
        m01 = 0.0f;
        m02 = 0.0f;
        m03 = 0.0f;
        m10 = 0.0f;
        m11 = 0.0f;
        m12 = 0.0f;
        m13 = 0.0f;
        m20 = 0.0f;
        m21 = 0.0f;
        m22 = 0.0f;
        m23 = 0.0f;
        m30 = 0.0f;
        m31 = 0.0f;
        m32 = 0.0f;
        m33 = 0.0f;
    }

    public LMatrix4X4 MatrixMultiply(LMatrix4X4 M)
    {
        LMatrix4X4 re = new LMatrix4X4();
        for (int i = 1; i <= 4; ++i)
        {
            for (int j = 1; j <= 4; ++j)
            {
                for (int k = 1; k <= 4; k++)
                {
                    re[i, j] += this[i, k] * M[k, j];
                }
            }
        }
        return re;
    }

    public LMatrix4X4 MatrixMultiplySelf(LMatrix4X4 M)
    {
        LMatrix4X4 re = new LMatrix4X4();
        for (int i = 1; i <= 4; ++i)
        {
            for (int j = 1; j <= 4; ++j)
            {
                for (int k = 1; k <= 4; k++)
                {
                    re[i, j] += this[i, k] * M[k, j];
                }
            }
        }
        return re;
    }

    public LVector4 MatrixMultiply(LVector4 v)
    {
        LVector4 MultiplyRe = new LVector4();

        int i = 1;
        int j = 1;
        MultiplyRe.x = this[i, j] * v.x + this[i, j + 1] * v.y + this[i, j + 2] * v.z + this[i, j + 3] * v.w;
        j = 1;
        MultiplyRe.y = this[i + 1, j] * v.x + this[i + 1, j + 1] * v.y + this[i + 1, j + 2] * v.z + this[i + 1, j + 3] * v.w;
        j = 1;
        MultiplyRe.z = this[i + 2, j] * v.x + this[i + 2, j + 1] * v.y + this[i + 2, j + 2] * v.z + this[i + 2, j + 3] * v.w;
        j = 1;
        MultiplyRe.w = this[i + 3, j] * v.x + this[i + 3, j + 1] * v.y + this[i + 3, j + 2] * v.z + this[i + 3, j + 3] * v.w;
        return MultiplyRe;
    }

    public LMatrix4X4 MatrixAdd(LMatrix4X4 M)
    {
        LMatrix4X4 re = new LMatrix4X4();
        for (int i = 0; i <= 4; ++i)
        {
            for (int j = 0; j <= 4; ++j)
            {
                re[i, j] = this[i, j] + M[i, j];
            }
        }
        return re;
    }

    public LMatrix4X4 MatrixSubtration(LMatrix4X4 M)
    {
        LMatrix4X4 re = new LMatrix4X4();

        for (int i = 0; i <= 4; ++i)
        {
            for (int j = 0; j <= 4; ++j)
            {
                re[i, j] = this[i, j] - M[i, j];
            }
        }

        return re;
    }

    //矩阵平移
    public void GetTranslationMatrix(float x, float y, float z)
    {
        this.UnitMatrix4X4();
        this[4, 1] = x;
        this[4, 2] = y;
        this[4, 3] = z;
    }

    public void GetTranslationMatrix(LVector4 TranslationVec)
    {
        GetTranslationMatrix(TranslationVec.x, TranslationVec.y, TranslationVec.z);
    }


    //矩阵缩放
    public void SetSocal(LVector4 ScoalVec)
    {
        this.UnitMatrix4X4();
        this[1, 1] = ScoalVec.x;
        this[2, 2] = ScoalVec.y;
        this[3, 3] = ScoalVec.z;
    }


    //矩阵旋转
    public void SetRotaionX(float alpha)
    {
        float Rad = Mathf.Deg2Rad * alpha;
        this.UnitMatrix4X4();
        this[2, 2] = Mathf.Cos(Rad);
        this[2, 3] = Mathf.Sin(Rad);
        this[3, 2] = -Mathf.Sin(Rad);
        this[3, 3] = Mathf.Cos(Rad);
    }

    public void SetRotaionY(float alpha)
    {
        float Rad = Mathf.Deg2Rad * alpha;

        this.UnitMatrix4X4();
        this[1, 1] = Mathf.Cos(Rad);
        this[1, 3] = -Mathf.Sin(Rad);
        this[3, 1] = Mathf.Sin(Rad);
        this[3, 3] = Mathf.Cos(Rad);
    }

    public void SetRotaionZ(float alpha)
    {
        float Rad = Mathf.Deg2Rad * alpha;

        this.UnitMatrix4X4();
        this[1, 1] = Mathf.Cos(Rad);
        this[1, 2] = Mathf.Sin(Rad);
        this[2, 1] = -Mathf.Sin(Rad);
        this[2, 2] = Mathf.Cos(Rad);
    }

    //转置
    public LMatrix4X4 MatrixTranspose()
    {
        LMatrix4X4 m = new LMatrix4X4();
        for (int i = 1; i <= 4; ++i)
        {
            for (int j = 1; j <= 4; ++j)
            {
                m[i, j] = this[j, i];
            }
        }

        return m;
    }
}
