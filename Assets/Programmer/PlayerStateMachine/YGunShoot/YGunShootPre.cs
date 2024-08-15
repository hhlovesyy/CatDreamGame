using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YGunShootPre : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera thirdAimCamera;
    public GameObject testCommonThirdPersonFollowCam;
    public Transform thirdPersonFollowPlace;
    public Transform thirdPersonCommonFollowPlace;
    public Transform aimTargetReticle;
    //public GameObject[] effectToSpawn;
    public Transform gunTrans;

    public GameObject player;
    //public GameObject muzzlePrefab;
    // Start is called before the first frame update
    void Start()
    {
        Cinemachine.CinemachineVirtualCamera thirdAimCamera = this.thirdAimCamera;
        GameObject testCommonThirdPersonFollowCam = this.testCommonThirdPersonFollowCam;
        
        //给testCharacterShoot赋值
        HTestCharacterShoot testCharacterShoot = player.AddComponent<HTestCharacterShoot>();
        testCharacterShoot.thirdAimCamera = thirdAimCamera;
        testCharacterShoot.testCommonThirdPersonFollowCam = testCommonThirdPersonFollowCam;
        testCharacterShoot.thirdPersonFollowPlace = thirdPersonFollowPlace;
        testCharacterShoot.thirdPersonCommonFollowPlace = thirdPersonCommonFollowPlace;
        testCharacterShoot.aimTargetReticle = aimTargetReticle;
        // testCharacterShoot.effectToSpawn = yGunShootPre.effectToSpawn[0];
        testCharacterShoot.gunTrans = gunTrans;
        testCharacterShoot.mainPlayerCamera = Camera.main;

        // testCharacterShoot.thirdAimCamera.gameObject.SetActive(false);
        thirdAimCamera.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
