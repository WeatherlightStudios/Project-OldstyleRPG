using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public float min_posX, min_posY;
	public float max_posX, max_posY;

	Room LeftChildren;
	Room RightChildren;

	Room m_parent;

	public int min_sizeDivision = 4;

	public Room()
	{

	}

	public void Subdivide()
	{
			
		if((this.max_posX - this.min_posX) >= min_sizeDivision && (this.max_posY - this.min_posY) >= min_sizeDivision)
		{
			float divX = 0;
			float direction = Random.Range(0, 100);
			Room a = new Room();
			Room b = new Room();
			divX = Random.Range(this.min_posX, this.max_posX);
			
				float divY = 0;
				divY = (Random.Range(this.min_posY, this.max_posY));


			if(direction % 2 == 0)
			{


				a.min_posX = min_posX;
				a.min_posY = min_posY;

				a.max_posX = min_posX + divX;
				a.max_posY = max_posY;

				b.min_posX = min_posX + divX;
				b.min_posY = min_posY;

				b.max_posX = max_posX;
				b.max_posY = max_posY;

			}
			else
			{
				
				a.min_posX = min_posX;
				a.min_posY = min_posY;

				a.max_posX = max_posX;
				a.max_posY = min_posY +divY;

				b.min_posX = min_posX;
				b.min_posY = min_posY + divY;

				b.max_posX = max_posX;
				b.max_posY = max_posY;
			}

			LeftChildren = a;
			RightChildren = b;
			if(LeftChildren == null);
				LeftChildren.Subdivide();
			if(RightChildren == null);
				RightChildren.Subdivide();
		}

	}

	public void FirstDepth()
	{
		if(LeftChildren != null)
		{

			Gizmos.DrawWireCube(new Vector3((LeftChildren.min_posX + LeftChildren.max_posX) / 2.0f,0,(LeftChildren.min_posY + LeftChildren.max_posY) / 2.0f) , new Vector3(LeftChildren.max_posX -  LeftChildren.min_posX,0,LeftChildren.max_posY -LeftChildren.min_posY));
			LeftChildren.FirstDepth();
		}
		if(RightChildren != null)
		{
			Gizmos.DrawWireCube(new Vector3((RightChildren.min_posX + RightChildren.max_posX) / 2.0f,0,(RightChildren.min_posY + RightChildren.max_posY) / 2.0f ) , new Vector3(RightChildren.max_posX - RightChildren.min_posX,0,RightChildren.max_posY -RightChildren.min_posY));
			RightChildren.FirstDepth();
		}

	}

	
}

public class MapGenerator : MonoBehaviour 
{
	public int dungeonSizeX,  dungeonSizeY;

	Room m_root = new Room();
	

	void Start()
	{
	}
	
	void OnDrawGizmos()
	{
		m_root.Subdivide();
		m_root.max_posX = dungeonSizeX;
		m_root.max_posY = dungeonSizeY;

		m_root.min_posX = 0;		
		m_root.min_posY = 0;		

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(new Vector3(m_root.min_posX,0,m_root.min_posY) , new Vector3(m_root.max_posX,0,m_root.max_posY));
		Gizmos.color = Color.red;
		m_root.FirstDepth();


	}
}
