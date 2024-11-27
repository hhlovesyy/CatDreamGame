using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinusEffectObject : CatGameBaseItem
{
    public float effectInterval = 1f; //每隔多少秒触发一次效果

    public float effectValue = 1f; //效果值
    //这种是具备连续效果的物体，比如隔几秒后有xxx效果
    private void LogicFunc() //每隔一段时间的效果
    {
        
    }
}
