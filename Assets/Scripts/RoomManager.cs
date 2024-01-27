using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Tooltip("房間物件")]
    public GameObject room;
    public TriggerPoint triggerPoint;
    public int moveRange;

    private Queue<Room> rooms;

    public void Awake()
    {
        triggerPoint.OnTrigger += TriggerPointEvent;
    }

    public void Start()
    {
        InitinalRoom();
    }


    public void TriggerPointEvent(string name)
    {
        if (name == "CreateRoomCheckPoint")
        {
            CreateRoom();
            DestroyRoom();
        }
        else if (name == "EventCheckPoint")
        {
            //TODO:回頭接 GameManager 的遊戲事件觸發
            PuzzleGameEvent();
        }
    }

    public void InitinalRoom()
    {
        
    }
    
    //生成房間，更改事件觸發的距離
    public void CreateRoom()
    {
        //
    }

    public void DestroyRoom()
    {
        
    }

    public void PuzzleGameEvent()
    {
        
    }
}
