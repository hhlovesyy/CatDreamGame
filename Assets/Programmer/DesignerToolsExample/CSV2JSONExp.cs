using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV2JSONExp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string enemyID = "70000001";
        var enemy = SD_RogueEnemyCSVFile.Class_Dic[enemyID];
        Debug.Log(enemy.EnemyBulletAttribute);
        Debug.Log(enemy.addressableLink);
        Debug.Log(enemy._RogueEnemyStartHealth()); //这个是int类型的，也就是做完类型转换的，一般逻辑用这个
        Debug.Log(enemy.RogueEnemyStartHealth);  //这个是string类型的
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
