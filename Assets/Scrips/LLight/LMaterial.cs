using UnityEngine;
using System.Collections;

//材质
public class LMaterial
{
    //漫反射颜色
    public Color _diffuse { set; get; }

    //镜面反射颜色
    public Color _specular { set; get; }

    //漫反射强度
    public float DiffusStrength { set; get; }

    //高光反射强度
    public float SpecularStrength { set; get; }

    //高光指数
    public float n_s { set; get; }

    public Texture2D t { set; get; }

    public Color[][] ColorArray;

    public LMaterial(Color diffuse, Color specular,
        float diffusStrength, float specularStrength, float ns)
    {
        _diffuse = diffuse;
        _specular = specular;
        DiffusStrength = diffusStrength;
        SpecularStrength = specularStrength;
        n_s = ns;
    }

    public void SetTexture(string Texture2DName)
    {
        Texture2D t = UnityEngine.Resources.Load<Texture2D>(Texture2DName);
   
        SetTexture(t);
    }

    public void SetTexture(Texture t)
    {
        if (t == null)
        {
            Debug.LogError("贴图读取错误");
            return;
        }

        this.t = t as Texture2D;

        ColorArray = new Color[this.t.width][];

        for (int i = 0; i < this.t.width; ++i)
        {
            ColorArray[i] = new Color[this.t.height];
            for (int j = 0; j < this.t.height; ++j)
            {
                ColorArray[i][j] = this.t.GetPixel(i, j);
            }
        }

    }

    public Color GetColor(int x, int y)
    {
        if (x < 0 || y < 0)
        {
            return ColorArray[0][0];
        }
        else if (x >= t.width && y >= t.height)
        {
            return ColorArray[t.width-1][t.height-1];
        }
        else if(x >= t.width && y < t.height)
        {
            return ColorArray[t.width - 1][y];         
        }
        else if(x < t.width && y >= t.height)
        {
            return ColorArray[x][t.height - 1];
        }
        else if(y < 0)
        {
            return ColorArray[x][0];
        }
        else if( x < 0)
        {
            return ColorArray[0][y];
        }

        return ColorArray[x][y];
    }
}
