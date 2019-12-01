using UnityEngine;
using System.Collections;

//全局光照
public class LLight
{
    //光照位置
    public LVector4 Pos;

    //光照旋转角度
    public LVector4 Angel;

    LLightType _curLLightType;

    float _AmbientLight; //环境光强度

    public enum LLightType
    {
        LIGHTTYPE_DIRECTION,       //方向光
        LIGHTTYPE_PARALLEL         //平行光
    }

    public LLight(LVector4 v, LVector4 angel)
    {
        Pos = v;
        Angel = angel;
        _curLLightType = LLightType.LIGHTTYPE_PARALLEL;

        _AmbientLight = 0.5f;
    }

    public void SetAmbientLight(float AmbientLight)
    {
        _AmbientLight = AmbientLight;
    }

    //光入射方向
    public LVector4 LightDirection(LVector4 v)
    {
        if (_curLLightType == LLightType.LIGHTTYPE_PARALLEL)
        {
            return Pos - v;
        }
        else if (_curLLightType == LLightType.LIGHTTYPE_DIRECTION)
        {
            return Angel - v;
        }

        return new LVector4();
    }

    //计算反射方向
    public LVector4 ReflectionDirection(LVector4 nor, LVector4 lightDirection)
    {
        nor = nor.Normalize();
        LVector4 d = nor * Mathf.Max(2 * lightDirection.Dot(nor), 0) - lightDirection;
        return d.Normalize();
    }

    //漫反射
    public Color DiffuseColor(ref LVector v, LVector4 lightDirection, LMaterial lMaterial)
    {
        LVector4 v1 = v.Nor.Normalize();
        LVector4 v2 = lightDirection.Normalize();
        Color c = Color.white;

        float diffN = lMaterial.DiffusStrength * Mathf.Max(v1.Dot(v2), 0);

        c.r = lMaterial._diffuse.r * diffN;
        c.g = lMaterial._diffuse.g * diffN;
        c.b = lMaterial._diffuse.b * diffN;

        return c;
    }

    public Color SpecularColor(ref LVector v, LVector4 rDirection, LVector4 eyerDirection, LMaterial lMaterial)
    {
        LVector4 v1 = ReflectionDirection(v.Nor, rDirection.Normalize());
        LVector4 v2 = eyerDirection.Normalize();
        Color c = Color.white;
        float Specular = Mathf.Pow(Mathf.Max(v1.Dot(v2), 0), lMaterial.n_s);
        c.r = lMaterial.SpecularStrength * Specular; ;
        c.g = lMaterial.SpecularStrength * Specular;
        c.b = lMaterial.SpecularStrength * Specular;

        return c;
    }

    public void LLightColor(ref LVector v, ref LVector renderv, LVector4 lightDirection, LVector4 eyerDirection, LMaterial lMaterial)
    {
        Color Dc = DiffuseColor(ref v, lightDirection, lMaterial);
        Color Sc = SpecularColor(ref v, lightDirection, eyerDirection, lMaterial);
        renderv.color.r = v.color.r * (Dc.r + Sc.r + _AmbientLight);
        renderv.color.g = v.color.g * (Dc.g + Sc.g + _AmbientLight);
        renderv.color.b = v.color.b * (Dc.b + Sc.b + _AmbientLight);
    }
}
