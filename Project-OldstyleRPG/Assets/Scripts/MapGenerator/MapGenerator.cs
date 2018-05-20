using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Room
{
	public Vector2 TR_pos;
	public Vector2 BL_pos;
};

//classe: foglia dell BSP
public class Leaf
{
	//posizione in basso a sinistra dell'area
	public float min_posX, min_posY;
	//posizione in alto a destra dell'area
	public float max_posX, max_posY;

	//figlio a sinistra
	Leaf LeftChildren;
	//figlio a destra
	Leaf RightChildren;

	//il padre di questa foglia
	Leaf m_parent;

	Room m_room;

	bool isLastLeaf = false;

	public bool isHorizontalDiv;


	// la grandezza minima che puo avere un'area
	public int min_sizeDivision = 4;
	//spazziatura della funzione Random nella generazione della divisione X o Y
	public float RandomTreshold;
	//percentuale di spazziatura tra altezza e larghezza 
	public float PercentageTreshold;
	
	public GameObject roomBuilder;
	public GameObject corridorBuilder;
	
	//public static List<GameObject> dungeonPieces =  new List<GameObject>();
	public static List<RoomInfo> roomInfos = new List<RoomInfo>();
	public static List<CorridorInfo> corridorInfo = new List<CorridorInfo>();
	
	//public static List<Vector3> positions = new List<Vector3>();
	//public static List<Vector2> sizes = new List<Vector2>();
	
	//inizializzazione della classe
	public Leaf()
	{

	}
	
	public void getList(out List<RoomInfo> _roomInfo, out List<CorridorInfo> _corridorInfo){
		_roomInfo = roomInfos;
		_corridorInfo = corridorInfo;
	}
	
	//Funzione che suddivide l'albero
	public void Subdivide()
	{
		//controllo per vedere se la stanza sia piu piccola della grandezza minima consentita, e quindi vedere se dividerla nuovamente		
		if((this.max_posX - this.min_posX) >= min_sizeDivision && (this.max_posY - this.min_posY) >= min_sizeDivision)
		{
			//inizializzazione variabili di divisione
			int divX = 0;
			int divY = 0;

			//generazione della direzione nella quale verra divisa la stanza
			int direction = Random.Range(0, 2);

			//inizializzazione delle variabili figlie
			Leaf a = new Leaf();
			Leaf b = new Leaf();

			//check per vedere se la larghezza sia piu grande dell altezza
			//se e piu grande si divide in verticale
			//se piu piccolo si divide in horizontale
			if ((this.max_posX - this.min_posX) > (this.max_posY - this.min_posY) && (this.max_posX - this.min_posX) / (this.max_posY - this.min_posY)  >= PercentageTreshold)
            	direction = 0;
        	else if ((this.max_posY - this.min_posY) > (this.max_posX - this.min_posX) && (this.max_posY - this.min_posY) / (this.max_posX - this.min_posX) >= PercentageTreshold)
            	direction = 1;

			//genera la posiione x e y della divisione usando la spazziatura
			divX = (int)Random.Range(0 + RandomTreshold, (this.max_posX - this.min_posX) - RandomTreshold);
			divY = (int)Random.Range(0 + RandomTreshold, (this.max_posY - this.min_posY) - RandomTreshold);

				
			//che tipo di direzione e uscita dall random
			if(direction == 0)
			{

				//divisione verticale
				a.min_posX = min_posX;
				a.min_posY = min_posY;

				a.max_posX = min_posX + divX;
				a.max_posY = max_posY;

				b.min_posX = min_posX + divX;
				b.min_posY = min_posY;

				b.max_posX = max_posX;
				b.max_posY = max_posY;

				a.isHorizontalDiv = true;
				b.isHorizontalDiv = true;

			}
			else
			{
				//divisione orizontale
				a.min_posX = min_posX;
				a.min_posY = min_posY;

				a.max_posX = max_posX;
				a.max_posY = min_posY + divY;

				b.min_posX = min_posX;
				b.min_posY = min_posY + divY;

				b.max_posX = max_posX;
				b.max_posY = max_posY;

				a.isHorizontalDiv = false;
				b.isHorizontalDiv = false;
			}

			//passaggio delle variabili padre ai figli
			a.RandomTreshold = RandomTreshold;
			a.min_sizeDivision = min_sizeDivision;
			a.PercentageTreshold = PercentageTreshold;
			a.roomBuilder = roomBuilder;
			a.corridorBuilder = corridorBuilder;
			
			b.RandomTreshold = RandomTreshold;
			b.min_sizeDivision = min_sizeDivision;
			b.PercentageTreshold = PercentageTreshold;
			b.roomBuilder = roomBuilder;
			b.corridorBuilder = corridorBuilder;

			//aggiunta dei figli
			LeftChildren = a;
			RightChildren = b;


			LeftChildren.Subdivide();
			RightChildren.Subdivide();
		}

	}

	public void CheckRoom()
	{
		if(LeftChildren !=null || RightChildren != null)
		{
			if(LeftChildren !=null)
				LeftChildren.CheckRoom();
			if(RightChildren != null)
				RightChildren.CheckRoom();
		}
		else
		{
			GenRooms();
		}

	}

	public void GenRooms()
	{
		Vector2 min_pos = new Vector2(min_posX, min_posY);
		Vector2 max_pos = new Vector2(max_posX, max_posY);

		float first_BL_X = min_pos.x;
		float second_BL_X = min_pos.x + (max_pos.x - min_pos.x) / 3.0f;

		float first_BL_Y = min_pos.y;
		float second_BL_Y = min_pos.y + (max_pos.y - min_pos.y) / 3.0f;

		float first_TR_X = min_pos.x + ((max_pos.x - min_pos.x) / 3.0f) * 2.0f;
		float second_TR_X = max_pos.x;

		float first_TR_Y = min_pos.y + ((max_pos.y - min_pos.y) / 3.0f) * 2.0f;
		float second_TR_Y = max_pos.y;

		Vector2 room_pos_bottom_Left = new Vector2((int)Random.Range(first_BL_X, second_BL_X), (int)Random.Range(first_BL_Y, second_BL_Y));
		Vector2 room_pos_top_right = new Vector2((int)Random.Range(first_TR_X, second_TR_X ), (int)Random.Range(first_TR_Y , second_TR_Y));
		
		m_room.BL_pos = room_pos_bottom_Left;
		m_room.TR_pos = room_pos_top_right;
		
		//TODO: put room here
		
		//Gizmos.color = Color.blue;

		Vector3 position = new Vector3((m_room.BL_pos.x + m_room.TR_pos.x) / 2.0f,0,(m_room.BL_pos.y + m_room.TR_pos.y) / 2.0f);
		Vector3 size = new Vector3((m_room.TR_pos.x - m_room.BL_pos.x), 0 ,(m_room.TR_pos.y - m_room.BL_pos.y));
		
		Vector2 size2D = new Vector2(size.x, size.z);
		
		/*
		GameObject newRoom = groundTile;
		newRoom.transform.position = position;
		newRoom.GetComponentInChildren<SpriteRenderer>().size = size2D;
		 */
		
		//GameObject newPiece = new GameObject();
		//GenericRoomBuilder builder = newPiece.GetComponent<GenericRoomBuilder>();
		RoomInfo newRoomInfo = new RoomInfo(position, size2D, 4);
		roomInfos.Add(newRoomInfo);
		
		//positions.Add(position);
		//sizes.Add(size2D);
		
		//GameObject ground = Instantiate(groundTile, position, Quaternion.identity);
		//Gizmos.DrawCube(position, size);

		isLastLeaf = true;
	}

	public void connectRooms()
	{
		if(LeftChildren !=null || RightChildren != null)
		{

			if(LeftChildren !=null)
				LeftChildren.connectRooms();
			if(RightChildren != null)
				RightChildren.connectRooms();


			//Gizmos.color = Color.green;

			float Acenter_X = (LeftChildren.min_posX + LeftChildren.max_posX) / 2.0f;
			float Acenter_Y = (LeftChildren.min_posY + LeftChildren.max_posY) / 2.0f;

			float Bcenter_X = (RightChildren.min_posX + RightChildren.max_posX) / 2.0f;
			float Bcenter_Y = (RightChildren.min_posY + RightChildren.max_posY) / 2.0f;						

			Vector3 from = new Vector3(Acenter_X, 0, Acenter_Y);
			Vector3 to = new Vector3(Bcenter_X, 0, Bcenter_Y);
			//Gizmos.DrawLine(from, to);
			
			//Debug.Log("from: " + from + " to: " + to);
			
			Vector3 centerPos = (from + to) / 2;
			Vector2 size = new Vector2(to.x - from.x, to.z - from.z);
			if(size.x == 0)
				size.x = 1;
			if(size.y == 0)
				size.y = 1;
			
			
			
			//GameObject newPiece = corridorBuilder;
			//GenericCorridorBuilder builder = newPiece.GetComponent<GenericCorridorBuilder>();
			CorridorInfo newCorrInfo = new CorridorInfo(centerPos, size, 4);
			corridorInfo.Add(newCorrInfo);
			
			//positions.Add(centerPos);
			//sizes.Add(size);

			if(!RightChildren.isHorizontalDiv)
			{
			}
			else
			{

		/*Gizmos.color = Color.green;
		if(LeftChildren.isLastLeaf || RightChildren.isLastLeaf)
		{
			from = new Vector3((LeftChildren.m_room.BL_pos.x + LeftChildren.m_room.TR_pos.x) / 2.0f, 0, (LeftChildren.m_room.BL_pos.y +LeftChildren.m_room.TR_pos.y) / 2.0f);
				to = new Vector3((RightChildren.m_room.BL_pos.x +RightChildren.m_room.TR_pos.x) / 2.0f, 0, (RightChildren.m_room.BL_pos.y +RightChildren.m_room.TR_pos.y) / 2.0f);

			//Gizmos.DrawLine(from, to);
		}*/
				
			}


		}
	}

	public bool isLastNode()
	{
		if(LeftChildren == null && RightChildren ==null)
		{
			return true;
		}
		return false;
	}


	public void FirstDepth()
	{

		//disegno dell debug
		if(LeftChildren != null)
		{
			//Gizmos.DrawWireCube(new Vector3((LeftChildren.min_posX + LeftChildren.max_posX) / 2.0f,0,(LeftChildren.min_posY + LeftChildren.max_posY) / 2.0f) , new Vector3(LeftChildren.max_posX -  LeftChildren.min_posX,0,LeftChildren.max_posY -LeftChildren.min_posY));
			LeftChildren.FirstDepth();
		}
		if(RightChildren != null)
		{
			//Gizmos.DrawWireCube(new Vector3((RightChildren.min_posX + RightChildren.max_posX) / 2.0f,0,(RightChildren.min_posY + RightChildren.max_posY) / 2.0f ) , new Vector3(RightChildren.max_posX - RightChildren.min_posX,0,RightChildren.max_posY -RightChildren.min_posY));
			RightChildren.FirstDepth();
		}

	}

	
}

public class MapGenerator : MonoBehaviour 
{
	public int dungeonSizeX,  dungeonSizeY;
	public int m_min_sizeDivision;
	public float m_PercentageTreshold;
	public float m_randomTreshold;
	
	public GameObject roomBuilder;
	public GameObject corridorBuilder;
	
	Leaf m_root = new Leaf();
	
	
	public static List<RoomInfo> roomInfo;
	public static List<CorridorInfo> corridorInfo;
	
	//public static List<Vector3> positions;
	//public static List<Vector2> sizes;

	void Start()
	{
		BSPDungeon();
	}
	
	void OnDrawGizmos()
	{
		
	}
	
	void BSPDungeon(){
		
		SetupRoot();
		m_root.Subdivide();

		//Gizmos.color = Color.red;
		m_root.FirstDepth();
		m_root.CheckRoom();
		m_root.connectRooms();
		
		m_root.getList(out roomInfo, out corridorInfo);
		for(int i = 0; i < roomInfo.Count; i++){
			//print(roomInfo[i].size);
			
			GameObject newRoom = Instantiate(roomBuilder, roomInfo[i].position, Quaternion.identity);
			newRoom.GetComponent<GenericRoomBuilder>().info = roomInfo[i];
		}
		for(int i = 0; i < corridorInfo.Count; i++){
			GameObject newCorridor = Instantiate(corridorBuilder, corridorInfo[i].position, Quaternion.identity);
			newCorridor.GetComponent<GenericCorridorBuilder>().info = corridorInfo[i];
			//GenericRoomBuilder roomGenerator = newRoom.GetComponent<GenericRoomBuilder>();
			//roomGenerator.info = new RoomInfo(positions[i], sizes[i], 4);
			//newRoom.GetComponentInChildren<SpriteRenderer>().size = sizes[i];
		}
	}
	
	static void Test(){
		
	}
	
	void SetupRoot(){
		
		m_root.min_posX = 0;		
		m_root.min_posY = 0;		
		m_root.min_sizeDivision = m_min_sizeDivision;
		m_root.PercentageTreshold = m_PercentageTreshold;
		m_root.max_posX = dungeonSizeX;
		m_root.max_posY = dungeonSizeY;
		m_root.RandomTreshold = m_randomTreshold;
		
		m_root.roomBuilder = this.roomBuilder;
		m_root.corridorBuilder = this.corridorBuilder;
	}
}
