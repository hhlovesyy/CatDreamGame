using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinusEffectObject : CatGameBaseItem
{
    //这种是具备连续效果的物体，比如隔几秒后有xxx效果
    private void LogicFunc() //每隔一段时间的效果
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //从策划表当中读，每过多少秒触发xxx效果
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
