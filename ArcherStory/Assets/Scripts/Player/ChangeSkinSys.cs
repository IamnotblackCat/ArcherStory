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

    //public SkinnedMeshRenderer smrClose;
    //public GameObject hips;
    //public Transform[] avatarHips;
    //public UnityEngine.Material closeNew;
    //public Mesh mesh;
    //public bool isNew = false;

    //private void Update()
    //{
    //    if (isNew)
    //    {
    //        ChangeMesh();
    //    }
    //}
    //private void ChangeMesh()
    //{
        //list<transform> bones = new list<transform>();
        //foreach (var trans in smrclose.bones)
        //{
        //    foreach (var bone in avatarhips)
        //    {
        //        if (bone.name == trans.name)
        //        {
        //            bones.add(bone);
        //            break;
        //        }
        //    }
        //}
        //smrclose.bones = bones.toarray();
        //smrclose.material = closenew;
        //smrclose.sharedmesh = mesh;
    //    smrClose.materials[1] = closeNew;
    //}
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