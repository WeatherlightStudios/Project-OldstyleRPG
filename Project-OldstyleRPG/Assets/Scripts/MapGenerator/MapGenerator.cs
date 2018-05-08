using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe foglia dell BSP
public class Room
{
	//posizione in basso a sinistra dell'area
	public float min_posX, min_posY;
	//posizione in alto a destra dell'area
	public float max_posX, max_posY;

	//figlio a sinistra
	Room LeftChildren;
	//figlio a destra
	Room RightChildren;

	//il padre di questa foglia
	Room m_parent;

	// la grandezza minima che puo avere un'area
	public int min_sizeDivision = 4;
	//spazziatura della funzione Random nella generazione della divisione X o Y
	public float RandomTreshold;
	//percentuale di spazziatura tra altezza e larghezza 
	public float PercentageTreshold;

	//inizializzazione della classe
	public Room()
	{

	}

	//Funzione che suddivide l'albero
	public void Subdivide()
	{
		//controllo per vedere se la stanza sia piu piccola della grandezza minima consentita, e quindi vedere se dividerla nuovamente		
		if((this.max_posX - this.min_posX) >= min_sizeDivision && (this.max_posY - this.min_posY) >= min_sizeDivision)
		{
			//inizializzazione variabili di divisione
			float divX = 0;
			float divY = 0;

			//generazione della direzione nella quale verra divisa la stanza
			float direction = Random.Range(0, 2);

			//inizializzazione delle variabili figlie
			Room a = new Room();
			Room b = new Room();

			//check per vedere se la larghezza sia piu grande dell altezza
			//se e piu grande si divide in verticale
			//se piu piccolo si divide in horizontale
			if ((this.max_posX - this.min_posX) > (this.max_posY - this.min_posY) && (this.max_posX - this.min_posX) / (this.max_posY - this.min_posY)  >= 1.5)
            	direction = 0;
        	else if ((this.max_posY - this.min_posY) > (this.max_posX - this.min_posX) && (this.max_posY - this.min_posY) / (this.max_posX - this.min_posX) >= 1.5)
            	direction = 1;

			//genera la posiione x e y della divisione usando la spazziatura
			divX = Random.Range(0 + RandomTreshold, (this.max_posX - this.min_posX) - RandomTreshold);
			divY = (Random.Range(0 + RandomTreshold, this.max_posY - this.min_posY) - RandomTreshold);

				
			//che tipo di direzzione e uscita dall random
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

			}
			else
			{
				//divisione horizontale
				a.min_posX = min_posX;
				a.min_posY = min_posY;

				a.max_posX = max_posX;
				a.max_posY = min_posY +divY;

				b.min_posX = min_posX;
				b.min_posY = min_posY + divY;

				b.max_posX = max_posX;
				b.max_posY = max_posY;
			}

			//passaggio delle variabili padre ai figli
			a.RandomTreshold = RandomTreshold;
			a.min_sizeDivision = min_sizeDivision;
			a.PercentageTreshold = PercentageTreshold;
			
			b.RandomTreshold = RandomTreshold;
			b.min_sizeDivision = min_sizeDivision;
			b.PercentageTreshold = PercentageTreshold;

			//aggiunta dei figli
			LeftChildren = a;
			RightChildren = b;

			if(LeftChildren != null)
				LeftChildren.Subdivide();
			if(RightChildren != null)
				RightChildren.Subdivide();
		}

	}

	public void FirstDepth()
	{

		//disegno dell debug
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
	public int m_min_sizeDivision;
	public float m_PercentageTreshold;
	public float m_randomTreshold;

	Room m_root = new Room();
	

	void Start()
	{
	}
	
	void OnDrawGizmos()
	{
		m_root.min_posX = 0;		
		m_root.min_posY = 0;		
		m_root.min_sizeDivision = m_min_sizeDivision;
		m_root.PercentageTreshold = m_PercentageTreshold;
		m_root.max_posX = dungeonSizeX;
		m_root.max_posY = dungeonSizeY;
		m_root.RandomTreshold = m_randomTreshold;
		m_root.Subdivide();

		Gizmos.color = Color.red;
		m_root.FirstDepth();


	}
}
