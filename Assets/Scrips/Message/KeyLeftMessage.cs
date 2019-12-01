using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class KeyLeftMessage : InfaceMessage
{
    public void Execute()
    {
        Test.s_lRenderSys.CCameraTransform(new LVector4(), new LVector4(0, Test.s_speed * 5, 0));
    }
}