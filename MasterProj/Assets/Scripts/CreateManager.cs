using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;

public class CreateManager : MonoBehaviour
{

    public TMP_InputField removeInput;
    public TMP_InputField CreateBehaviourInput;
    public Button BuildClassBtn;
    public Button AddInstanceBtn;
    public Button RemoveInstanceBtn;

    public static CreateManager manager;

    public GameObject instanceGo;
    int idx = 0;

    Dictionary<int, GameObject> instancesDic = new Dictionary<int, GameObject>();

    private void Awake()
    {
        manager = GetComponent<CreateManager>();
    }

    string removeInputTxt;
    string buildDesc;

    private void Start()
    {
        //BuildClassBtn.onClick.AddListener(BuildClassBtnHandler);
        AddInstanceBtn.onClick.AddListener(AddInstanceBtnHandler);
        RemoveInstanceBtn.onClick.AddListener(RemoveInstanceBtnHandler);
        removeInput.onValueChanged.AddListener(RemoveInputHandler);
        CreateBehaviourInput.onValueChanged.AddListener(CreateBehaviourInputHandler);

    }

    private void CreateBehaviourInputHandler(string arg0)
    {
        buildDesc = arg0;
    }

    private void RemoveInputHandler(string arg0)
    {
        removeInputTxt = arg0;
    }

    private void RemoveInstanceBtnHandler()
    {
        if (string.IsNullOrEmpty(removeInputTxt))
        {
            return;
        }
        int val = int.Parse(removeInputTxt);
        if (instancesDic.ContainsKey(val))
        {
            GameObject.Destroy(instancesDic[val]);
            instancesDic.Remove(val);
        }
    }

    private void AddInstanceBtnHandler()
    {
        GameObject go = GameObject.Instantiate(instanceGo);
        ++idx;
        //AI ai = go.GetComponent<AI>();
        AI ai = go.GetComponent<AI>();
        if (ai != null)
        {
            //ai.SetBaseInfo(idx, transform);
            instancesDic.Add(idx, go);
        }
    }
}
