using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//TODO:觸發事件後要讓事件消失
public class RoomManager : MonoBehaviour
{
    public Transform cameraTransform;
    [Tooltip("房間物件")]
    public GameObject room;
    public TriggerPoint triggerPoint;
    public int createRoomNum;
    public int createRoomRange;
    public Vector3 initinalRoom;
    public int currentIndex = 0;
    [Tooltip("遊戲結束的時候要生門出來")]
    public GameObject door;

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
        rooms = new LinkedList<Room>();
    }

    public void Start()
    {
        InitinalRoom();
    }
    
    public void TriggerPointEvent(GameObject gameObject)
    {
        if (gameObject.name == "CreateRoomCheckPoint")
        {
            CreateRoom();
        }
        else if (gameObject.name == "EventCheckPoint")
        {
            isSolvingPuzzle = true;
            OnPuzzleGame.Invoke();
            Destroy(gameObject);
        }
        else if(gameObject.name == "TestMouse")
        {
            //TODO:直接 GameOver 還是什麼?
        }
        else if(gameObject.name == "Door")
        {
            //TODO:播放勝利的動畫
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
        if (cameraTransform.rotation.y == 0)
        {
            if(PuzzleManager.Instance.IsSuccess)
                CreateForwordRoom();
            else
                CreateBackRoom();
        }
        else
        {
            if(PuzzleManager.Instance.IsSuccess)
                CreateBackRoom();
            else
                CreateForwordRoom();
        }
    }
}
