/****************************************************
    文件：ChangeSkinSys.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/12/25 18:2:20
	功能：换装系统
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ChangeSkinSys : SystemRoot 
{
    public UnityEngine.Material bowNew;
    public UnityEngine.Material bowOld;

    public SkinnedMeshRenderer weaponMesh;
    public GameObject flashFX;
    private static ChangeSkinSys _instance;

    
    public static ChangeSkinSys Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Start()
    {
        _instance = this;
        //avatarHips = hips.GetComponentsInChildren<Transform>();
    }
    public void ChangeWeaponSkinToNew()
    {
        weaponMesh.material = bowNew;
        flashFX.SetActive(true);
    }
    public void ChangeWeaponSkinToOld()
    {
        weaponMesh.material = bowOld;
        flashFX.SetActive(false);
    }
}