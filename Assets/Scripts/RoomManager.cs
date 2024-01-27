using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
    [Tooltip("房間物件")]
    public GameObject room;
    public TriggerPoint triggerPoint;
    public int createRoomNum;
    public int createRoomRange;
    public Vector3 initinalRoom;
    public int currentIndex = 0;
    public bool isSucceed = true;

    private LinkedList<Room> rooms;
    public static bool isSolvingPuzzle = false;
    public static Action OnPuzzleGame;
    
    private static RoomManager instance;
    
    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RoomManager();
            }
            return instance;
        }
    }

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
        }
        else if (name == "EventCheckPoint")
        {
            isSolvingPuzzle = true;
            OnPuzzleGame.Invoke();
        }
    }

    public void InitinalRoom()
    {
        //TODO:設定 Puzzle 的位置
        //SetPuzzleGameCheckPosition();
        GameObject tempRoom = Instantiate(room);
        tempRoom.transform.parent = transform;
        tempRoom.transform.position = new Vector3(0, 0, 0);
        rooms.AddFirst(tempRoom.GetComponent<Room>());
        for (int i = 0; i < createRoomNum; i++)
        {
            tempRoom = Instantiate(room);
            tempRoom.transform.parent = transform;
            tempRoom.transform.position = new Vector3(0, 0, (i + 1)*createRoomRange);
            rooms.AddFirst(tempRoom.GetComponent<Room>());
        }
        for (int i = 0; i > -createRoomNum; i--)
        {
            tempRoom = Instantiate(room);
            tempRoom.transform.parent = transform;
            tempRoom.transform.position = new Vector3(0, 0, (i - 1)*createRoomRange);
            rooms.AddLast(tempRoom.GetComponent<Room>());
        }
    }
    
    //生成房間，更改事件觸發的距離
    public void CreateForwordRoom()
    {
        GameObject tempRoom = Instantiate(room);
        tempRoom.transform.parent = transform;
        tempRoom.transform.position = new Vector3(0, 0, (currentIndex + 2)*createRoomRange);
        Room destoryRoom = rooms.Last.Value;
        rooms.AddFirst(tempRoom.GetComponent<Room>());
        rooms.RemoveLast();
        Destroy(destoryRoom);
        currentIndex++;
    }
    
    public void CreateBackRoom()
    {
        GameObject tempRoom = Instantiate(room);
        tempRoom.transform.parent = transform;
        tempRoom.transform.position = new Vector3(0, 0, (currentIndex + 2)*createRoomRange);
        Room destoryRoom = rooms.First.Value;
        rooms.AddLast(tempRoom.GetComponent<Room>());
        rooms.RemoveFirst();
        Destroy(destoryRoom);
        currentIndex--;
    }

    public void CreateRoom()
    {
        if(isSucceed)
            CreateForwordRoom();
        else
            CreateBackRoom();
    }
    
    public void SetPuzzleGameCheckPosition()
    {
        //設定 Puzzle 的隨機位置
    }
}
