using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyDMessage : InfaceMessage
{
    public void Execute()
    {
        Test.s_lRenderSys.CCameraTransform(new LVector4(-Test.s_speed, 0, 0), new LVector4());
    }
}