using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class KeyAlpha3Message : InfaceMessage
{
    public void Execute()
    {
        Test.s_lRenderSys.ListClear();
        Test.s_model = LModelFactory.CreateMoel("MyModel");
        Test.s_model.SetPos(new LVector4(0, -1, 4));
        Test.s_model.SetRotation(-90.0f, 180.0f, 0.0f);
        Test.s_lRenderSys.AddModelList(Test.s_model);
    }
}

