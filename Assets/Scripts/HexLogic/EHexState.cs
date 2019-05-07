/*
    Copyright (c) 2019, Szymon Jakóbczyk
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//represents states that one field can be in
namespace Assets.Scripts.HexLogic
{
    public enum EHexState
    {
        Ocean,
        Sea,
        Flatland,
        Rainforest,
        Tundra,
        Ice,
        Mountains,
        Desert
    }
}
