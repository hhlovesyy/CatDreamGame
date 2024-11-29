using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum TriggerShowLevelPartType
{
    InShowOutHide, //进入当前trigger的时候显示，离开的时候隐藏public List<Collider> triggers; 
}
public class TriggerToShowLevelPart : MonoBehaviour
{
    //这个类用于控制每个trigger和他们显隐的情况
    public List<GameObject> triggerGameObjects;
    [SerializeField] public TriggerShowLevelPartType triggerType;
    //用字典存储所有的GO
    private List<GameObject> triggerGameObjectHashSet = new List<GameObject>();
    private Vector3 triggerCenter;
    private Vector3 triggerSize;
    private void Start()
    {
        //todo:很费，到时候看看怎么优化，暂时先不管了，这是为了解决摔到一楼的物体会跟随二楼一起隐藏掉的问题
        //遍历triggerGameObjects下面所有的子物体，存储在triggerGameObjectHashSet中，包括所有子节点的子节点
        foreach (var triggerGameObject in triggerGameObjects)
        {
            triggerGameObjectHashSet.Add(triggerGameObject);
            foreach (Transform child in triggerGameObject.transform)
            {
                triggerGameObjectHashSet.Add(child.gameObject);
            }
        }
        // 获取当前碰撞箱的中心位置（局部坐标）
        triggerCenter = GetComponent<BoxCollider>().center;
        // 获取当前碰撞箱的宽高深（局部大小）
        triggerSize = GetComponent<BoxCollider>().size;
    }
    
    private bool IsTransformInTrigger(Vector3 transformPosition)
    {
        // 将局部中心和局部大小转换为世界坐标
        Vector3 worldCenter = transform.TransformPoint(triggerCenter);
        Vector3 halfSize = triggerSize / 2;

        // 使用世界坐标判断transformPosition是否在trigger中
        if (transformPosition.x > worldCenter.x - halfSize.x && transformPosition.x < worldCenter.x + halfSize.x &&
            transformPosition.y > worldCenter.y - halfSize.y && transformPosition.y < worldCenter.y + halfSize.y &&
            transformPosition.z > worldCenter.z - halfSize.z && transformPosition.z < worldCenter.z + halfSize.z)
        {
            return true;
        }
        return false;
    }

    private void JudgeHide(bool shouldHide)
    {
        foreach (var triggerGameObject in triggerGameObjectHashSet)
        {
            //dirty trick
            bool isInTrigger = IsTransformInTrigger(triggerGameObject.transform.position);
            if (isInTrigger)
            {
                triggerGameObject.gameObject.SetActive(!shouldHide);
            }
            //如果不在trigger中，就不用管了
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (triggerType == TriggerShowLevelPartType.InShowOutHide)
            {
                JudgeHide(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (triggerType == TriggerShowLevelPartType.InShowOutHide)
            {
                JudgeHide(true);
            }
        }
    }
}
