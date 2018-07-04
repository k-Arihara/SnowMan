﻿using UnityEngine;
using System.Collections.Generic;

public class VRControllerScript : MonoBehaviour
{
  private int tool;
  private Dictionary<string, bool> flag = new Dictionary<string, bool>();
  private GameObject system, canvas, gripObj, bucket, icepick, scoop, ctrlModel;
  private SphereCollider controllerCollider;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  private void Start()
  {
    system = GameObject.Find("System");
    canvas = GameObject.Find("Canvas");
    bucket = GameObject.Find("bucket");
    icepick = GameObject.Find("icepick");
    scoop = GameObject.Find("scoop");
    ctrlModel = GameObject.Find("Model");
    controllerCollider = gameObject.GetComponent<SphereCollider>();

    flag.Add("isOpenMenu", false);
    flag.Add("bucket", false);
    flag.Add("scoop", false);
    flag.Add("icepick", false);
    flag.Add("ctrlModel", true);
    flag.Add("scooped", false);

    tool = 0;

    bucket.SetActive(flag["bucket"]);
    scoop.SetActive(flag["scoop"]);
    icepick.SetActive(flag["icepick"]);
    ctrlModel.SetActive(flag["ctrlModel"]);
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);
    touchPosition = device.GetAxis();

    //トリガーを握っている
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
      if (flag["isOpenMenu"])
      {
        system.GetComponent<SystemScript>().CreateSnowBall(new Vector3(Random.Range(1.0f, -1.0f), 0.06f, Random.Range(1.0f, -1.0f)));
      }
    }

    //トリガーを離した
    if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    {
      ReleaseObject();
    }

    //タッチパッドをクリック
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
    {
      if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
      {
        if (touchPosition.y > 0)
        {
          //タッチパッド上をクリックした場合の処理
          tool = 0;
          ChangeTool(tool);
        }
        else
        {
          //下をクリック
          flag["isOpenMenu"] = !flag["isOpenMenu"];
          canvas.SetActive(flag["isOpenMenu"]);
          tool = 2;
          ChangeTool(tool);
        }
      }
      else
      {
        if (touchPosition.x > 0)
        {
          //タッチパッド右をクリックした場合の処理
          tool = 1;
          ChangeTool(tool);
        }
        else
        {
          //左をクリック 
          tool = 3;
          ChangeTool(tool);
        }
      }
    }
  }

  void OnTriggerEnter(Collider collisionObj)
  {

  }

  void OnTriggerStay(Collider collisionObj)
  {
    if (collisionObj.gameObject.name == "SnowBall")
    {
      if (device != null && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
      {
        if (gripObj != collisionObj.gameObject)
        {
          GrabObject(collisionObj.gameObject);
        }
      }
    }

    if (collisionObj.gameObject.name == "Ground")
    {
      if (!flag["scooped"])
      {
        flag["scooped"] = true;
      }
    }
  }

  void OnTriggerExit(Collider collisionObj)
  {
    if (collisionObj.gameObject.name == "Ground" && flag["scooped"] && (device != null && device.GetPress(SteamVR_Controller.ButtonMask.Trigger)))
    {
      ScoopSnow();
    }
    flag["scooped"] = false;
  }

  void GrabObject(GameObject obj)
  {
    this.gripObj = obj;

    FixedJoint fj = gameObject.GetComponent<FixedJoint>();
    fj.connectedBody = obj.GetComponent<Rigidbody>();
  }

  void ReleaseObject()
  {
    FixedJoint fj = gameObject.GetComponent<FixedJoint>();
    if (fj != null)
    {
      fj.connectedBody = null;
    }
    this.gripObj = null;
  }

  void RotateSnow(Collider collisionObj)
  {
    var relativePos = collisionObj.gameObject.transform.position - controllerCollider.transform.position;
    Debug.Log(relativePos);

    var rigidbody = collisionObj.gameObject.GetComponent<Rigidbody>();

    rigidbody.AddForce(relativePos.x * 50, 0, relativePos.z * 50);
  }

  //使用する道具を変更
  void ChangeTool(int n)
  {
    switch (n)
    {
      case 0:
        flag["bucket"] = true;
        flag["scoop"] = false;
        flag["icepick"] = false;
        flag["ctrlModel"] = false;
        break;

      case 1:
        flag["bucket"] = false;
        flag["scoop"] = true;
        flag["icepick"] = false;
        flag["ctrlModel"] = false;
        break;

      case 3:
        flag["bucket"] = false;
        flag["scoop"] = false;
        flag["icepick"] = true;
        flag["ctrlModel"] = false;
        break;

      default:
        flag["bucket"] = false;
        flag["scoop"] = false;
        flag["icepick"] = false;
        flag["ctrlModel"] = true;
        break;
    }

    bucket.SetActive(flag["bucket"]);
    scoop.SetActive(flag["scoop"]);
    icepick.SetActive(flag["icepick"]);
    ctrlModel.SetActive(flag["ctrlModel"]);
  }

  void ScoopSnow()
  {
    Debug.Log("Appear SnowBall");

    var currentPosition = gameObject.transform.position;

    for (int i = -2; i <= 3; i++)
    {
      for (int j = -2; j <= 3; j++)
      {
        for (int k = -2; k <= 3; k++)
        {
          system.GetComponent<SystemScript>().CreateSnowBall(new Vector3(currentPosition.x + i * 0.08f, currentPosition.y + k * 0.08f, currentPosition.z + j * 0.08f));
        }
      }
    }
  }
}