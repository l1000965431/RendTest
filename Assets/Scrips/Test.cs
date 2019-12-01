using UnityEngine;
using System.Collections;
using System;

public class Test : MonoBehaviour
{
    public static int ScreenWidth = 800;
    public static int ScreenHight = 600;
    public static bool s_Debug = false;
    public static bool RenderUpdate = true;

    public static LRenderSys s_lRenderSys;

    public static LModel s_model;

    public static float s_speed;

    private KeyMessageSys _KeyMessageSys;

    void Awake()
    {
        //关闭垂直同步
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;

        Renderer r = this.GetComponent<Renderer>();
        s_lRenderSys = new LRenderSys(r);
        s_lRenderSys.SetLight(false);
        s_speed = 0.1f;

        _KeyMessageSys = new KeyMessageSys();

        s_model = LModelFactory.CreateMoel("MyModel");
        Vector3 pos = new Vector3(0, 0, 4);
        s_model.SetPos(new LVector4(0, -1, 4));
        s_model.SetRotation(-90.0f, 180.0f, 0.0f);
        s_lRenderSys.AddModelList(s_model);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        s_lRenderSys.Render();
    }

    private bool KeyControl(KeyCode key)
    {
        bool Key = false;
        if (s_Debug)
        {
            Key = Input.GetKeyDown(key);
        }
        else
        {
            Key = Input.GetKey(key);
        }

        return Key;
    }

    void OnGUI()
    {
        if (s_Debug)
        {
            if (Input.anyKeyDown)
            {
                Event e = Event.current;

                if (e.isKey)
                {
                    _KeyMessageSys.SendMessage(e.keyCode.ToString());
                }
            }
        }
        else
        {
            if (Input.anyKey)
            {
                Event e = Event.current;

                if (e.isKey)
                {
                    _KeyMessageSys.SendMessage(e.keyCode.ToString());
                }
            }
        }
    }

}
