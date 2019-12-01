using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyJMessage : InfaceMessage
{
    public void Execute()
    {
        Test.s_model.SetRotation(0, Test.s_speed*10, 0);
        Test.s_lRenderSys.SetUpdate();
    }
}

