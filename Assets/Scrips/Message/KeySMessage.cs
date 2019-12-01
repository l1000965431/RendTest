using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class KeySMessage : InfaceMessage
{
    public void Execute()
    {
        Test.s_lRenderSys.CCameraTransform(new LVector4(0, 0, -Test.s_speed), new LVector4());
    }
}