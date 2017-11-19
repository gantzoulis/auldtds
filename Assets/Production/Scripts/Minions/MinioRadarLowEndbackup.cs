﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinionRadarLowEndbackup : MonoBehaviour
{
	#region Classes
	[System.Serializable]
	public class Room
	{
		public GameObject room;
		public float roomPoints;
	}

	[System.Serializable]
	public class UnitInRadar
	{
		public GameObject unit;
		public float unitDistance;
		public bool fullengaged;
	}
	#endregion

	#region Properties
	[Header ("Room")]
	[SerializeField]
	private List<Room> roomList;
	[SerializeField]
	private GameObject parentObject;
	private MinionAI minionAI;
	[SerializeField]
	private GameObject targetRoom;

	[Header("Unit")]
	private List<UnitInRadar> unitInRadarList;
	[SerializeField]
	private GameObject targetUnit;
	[SerializeField]
	private GameObject minion;
	#endregion

	#region System Functions
	// Use this for initialization

	void Start () 
	{
		roomList = new List<Room>();
		unitInRadarList = new List<UnitInRadar>();
		minionAI = parentObject.GetComponent<MinionAI>();
	}

	// Update is called once per frame
	void Update () 
	{
		//AssignTargetRoom();
		//AssignTargetUnit();
		ClearRTSUnitTable();
	}

	private void OnTriggerStay(Collider other)
	{
		if((other.tag == "RoomArea") && (other.GetComponent<RoomEntityIdentifier>().roomTreasureScore > 0))
		{
			Room roomClass = new Room();
			roomClass.room = other.gameObject;
			roomClass.roomPoints = other.GetComponent<RoomEntityIdentifier>().roomTotalScore;
			if(!roomList.Any(i=>i.room == other.gameObject))
			{
				roomList.Add(roomClass);
				roomList.Sort(SortByPoints);
				targetRoom = roomList[0].room;
			}
			AssignTargetRoom(targetRoom);
		}
		else if(roomList.All(i=>i.roomPoints <= 0))
		{
			targetRoom = null;
		}

		if((other.tag == "RTSUnit") && (other.GetComponent<UnitAttributes>().unitBaseAttributes.unitIsAlive == true))
		{
			if((!other.GetComponent<ObjectEngage>().fullEngaged))
			{
				UnitInRadar unitInRadarClass = new UnitInRadar();
				unitInRadarClass.unit = other.gameObject;
				unitInRadarClass.unitDistance = Vector3.Distance(other.transform.position,this.transform.position);
				unitInRadarClass.fullengaged = other.GetComponent<ObjectEngage>().fullEngaged;
				if(!unitInRadarList.Any(i=>i.unit == other.gameObject))
				{
					unitInRadarList.Add(unitInRadarClass);
					unitInRadarList.Sort(SortByUnitDistance);
					targetUnit = unitInRadarList[0].unit;
				}
				AssignTargetUnit(targetUnit);
			}
		}
		else if(unitInRadarList == null)
		{
			targetUnit = null;
		}
	}
	#endregion

	#region Select Room Functions
	static int SortByPoints(Room tr1, Room tr2)
	{
		return tr2.roomPoints.CompareTo(tr1.roomPoints);
	}

	private void AssignTargetRoom(GameObject targetRoom)
	{
		this.GetComponentInParent<MinionAI>().lockedRoomTarget = targetRoom;
		roomList.Clear();
	}
	#endregion

	#region Select Unit Functions
	static int SortByUnitDistance(UnitInRadar u1, UnitInRadar u2)
	{
		return u1.unitDistance.CompareTo(u2.unitDistance);
	}

	private void AssignTargetUnit(GameObject targetUnit)
	{
		if (!minionAI.lockedOnUnit)
		{
			this.GetComponentInParent<MinionAI>().unitTarget = targetUnit;
			//unitInRadarList.Clear();
		}
	}

	private void ClearRTSUnitTable()
	{
		for (int i =0; i < unitInRadarList.Count; i++)
		{
			if(unitInRadarList[i] == null)
			{
				unitInRadarList.Remove(unitInRadarList[i]);
			}

			if(unitInRadarList[i].fullengaged)
			{
				unitInRadarList.Remove(unitInRadarList[i]);
			}
		}
	}
	#endregion
}

