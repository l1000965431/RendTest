using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyCtrlMessage : InfaceMessage
{
    public void Execute()
    {
        int cur = (int)Test.s_lRenderSys.GetCurFillType();
        int num = (int)BaseLRender.FillType.FILLTYPE_NUM;
        cur = (cur + 1) % num;
        Test.s_lRenderSys.SetFillType((BaseLRender.FillType)cur);
        Test.s_lRenderSys.ModelClear();
        Test.s_lRenderSys.SetUpdate();
    }
}