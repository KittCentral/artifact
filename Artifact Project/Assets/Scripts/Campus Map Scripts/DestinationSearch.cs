using UnityEngine;
using System.Collections;

public class DestinationSearch : MonoBehaviour {
	private string[] destinations;
	private Vector3[] destinationlocations;
	private Building[] buildings = new Building[153];
	// Use this for initialization
	void Start () {
		string [] destinations = {
			"Academic Advising Center: Students in Transition",//done
			"Aden Hall",//
			"Administrative and Research Center",//
			"Andrews Hall",//
			"Armory",//
			"Arnett Hall",//
			"Arts and Sciences Finance and Payroll Administration",//
			"Arts and Sciences Office Building 1",//
			"Athens Court",//
			"Athens North Court",//
			"Athletics Practice Field",//
			"Baker Hall",//
			"Balch Fieldhouse",
			"Bear Creek Apartments at Williams Village",//
			"Benson Earth Sciences",//
			"Brackett Hall",//
			"Bruce Curtis Building (Museum Collections)",//
			"Buckingham Hall",//
			"Business Field",//
			"Business, Leeds School of Business, BUS",//
			"CU Book Store",//
			"CU Heritage Center",//
			"Carlson Gymnasium",//
			"Center for Asian Studies",//
			"Center for Astrophysics and Space Astronomy",
			"Center for Community",
			"Chancellor's Residence,PRES,President's Residence",
			"Cheyenne Arapaho Hall",
			"Clare Small Arts and Sciences",
			"Cockerell Hall",
			"College Inn",
			"Computing Center",
			"Continuing Education",
			"Cooperative Institute for Research in Environmental Sciences",
			"Coors Events/Conference Center",
			"Cristol Chemistry and Biochemistry",
			"Crosman Hall",
			"Dal Ward Athletic Center",
			"Dalton Trumbo Fountain Court",
			"Darley Commons",
			"Darley Towers",
			"Denison Arts and Sciences",
			"Discovery Learning Center",
			"Duane Physics and Astrophysics",
			"Eaton Humanities",
			"Economics",
			"Education",
			"Ekeley Sciences",
			"Engineering Center",
			"Environmental Design",
			"Environmental Health and Safety Center ",
			"Euclid Avenue AutoPark","Faculty-Staff Court",
			"Family Housing Children's Center - Newton Court",
			"Farrand Field",
			"Farrand Hall",
			"Fiske Planetarium and Science Center",
			"Fleming Building",
			"Folsom Field",
			"Franklin Field",
			"Gamow Tower",
			"Gates Woodruff Women's Studies Cottage",
			"Gold Biosciences Building",
			"Grounds and Recycling",
			"Guggenheim Geography",
			"Hale Science",
			"Hallett Hall",
			"Health Physics Laboratory",
			"Hellems Arts and Sciences",
			"Housing System Maintenance Center",
			"Housing System Service Center",
			"Humanities",
			"Imig Music",
			"Institute for Behavioral Genetics ",
			"Institute of Behavioral Science ",
			"Institute of Behavioral Science No. 02",
			"Institute of Behavioral Science No. 04 ",
			"Institute of Behavioral Science No. 06",
			"Institute of Behavioral Science No. 07 ",
			"Institute of Behavioral Science No. 09",
			"Institute of Behavioral Science No. 10",
			"Integrated Teaching and Learning Laboratory",
			"International English Center",
			"JILA",
			"Jennie Smoly Caruthers Biotec Building",
			"Ketchum Arts and Sciences",
			"Kittredge Central",
			"Kittredge Field",
			"Kittredge West Hall",
			"Koelbel Building",
			"Koenig Alumni Center",
			"LASP Space Technology Research Center",
			"Lesser House",
			"Libby Hall",
			"MacAllister Building",
			"Macky Auditorium",
			"Marine Court",
			"Mary Rippon Theatre",
			"Mathematics Building",
			"McKenna Languages",
			"Muenzinger Psychology",
			"Museum Collections ",
			"Museum of Natural History",
			"Newton Court",
			"Norlin Library",
			"Norlin Quadrangle",
			"Old Main",
			"Page Foundation Center",
			"Police and Parking Services",
			"Porter Biosciences",
			"Potts Field",
			"Power House",
			"Prentup Field",
			"Ramaley Biology",
			"Reed Hall",
			"Regent Administrative Center",
			"Regent Drive AutoPark",
			"Research Laboratory No. 1, Litman",
			"Research Laboratory No. 2, RL2",
			"Research Laboratory No. 4, Life Science",
			"Research Laboratory No. 6, Marine Street Science Center",
			"Research Park Greenhouse",
			"Roser ATLAS Center",
			"Science Learning Laboratory ",
			"Sewall Hall",
			"Smiley Court",
			"Smith Hall",
			"Sommers-Bausch Observatory",
			"Space Science Building",
			"Speech, Language, and Hearing Sciences",
			"Stadium Ticket Building",
			"Stearns Towers ",
			"Student Recreation Center",
			"Technology Learning Center",
			"Temporary Building No. 1",
			"Temporary Building No. 82",
			"Transportation Center and Annex",
			"University Administration Center",
			"University Administrative Center Annex",
			"University Club",
			"University Memorial Center",
			"University Residence",
			"University Theatre",
			"Varsity Lake and Bridge",
			"Visual Arts Complex",
			"Wardenburg Student Health Center",
			"Willard Hall",
			"Williams Village 2",
			"Williams Village Field",
			"Wolf Law",
			"Woodbury Arts and Sciences"};
		
		Vector3 [] destinationlocations = {
			new Vector3(73.54697f,165.1207f,-18),new Vector3(92.36178f,154.1034f,-18),new Vector3(117.4822f,169.7337f,-18),new Vector3(98.42948f,145.1078f,-18),new Vector3(76.49773f,164.2636f,-18),new Vector3(100.3821f,145.8219f,-18),new Vector3(71.1441f,164.1775f,-18),new Vector3(77.15618f,163.9836f,-18),new Vector3(86.26775f,167.4393f,-18),new Vector3(82.16479f,168.9899f,-18),
			new Vector3(90.91123f,167.7584f,-18),new Vector3(87.81487f,154.2338f,-18),new Vector3(87.67052f,160.5443f,-18),new Vector3(120.7029f,130.3964f,-18),new Vector3(91.49084f,156.9573f,-18),new Vector3(93.16761f,154.9325f,-18),new Vector3(76.02614f,155.8282f,-18),new Vector3(100.9336f,143.5225f,-18),new Vector3(98.11758f,151.5041f,-18),new Vector3(95.98318f,151.5082f,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18),
			new Vector3(0,0,-18),new Vector3(0,0,-18),new Vector3(0,0,-18)};
		int i = 0;
		foreach(string x in destinations)
		{
			buildings[i] = new Building();
			buildings [i].Name = x;
			i++;
		}
		i = 0;
		foreach (Vector3 y in destinationlocations) {
			buildings [i].Location = y;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public struct Building
{

	string name;
	Vector3 location;
	string description;

	public string Name 
	{
		get 
		{
			return name;
		}
		set 
		{
			name = value;
		}
	}
	public Vector3 Location 
	{
		get 
		{
			return location;
		}
		set 
		{
			location = value;
		}
	}
	public string Description 
	{
		get 
		{
			return description;
		}
		set 
		{
			description = value;
		}
	}


}