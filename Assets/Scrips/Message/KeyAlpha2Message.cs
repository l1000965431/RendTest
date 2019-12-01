using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyAlpha2Message : InfaceMessage
{
    public void Execute()
    {
        Test.s_lRenderSys.ListClear();
        Test.s_model = LModelFactory.CreateMoel("MyCube");
        Test.s_model.SetPos(new LVector4(0, 0, 4));
        Test.s_model.SetRotation(0.0f, 45.0f, 45.0f);
        Test.s_lRenderSys.AddModelList(Test.s_model);
        Test.s_lRenderSys.SetFillType(BaseLRender.FillType.FILLTYPE_FILLMODE);
    }
}

