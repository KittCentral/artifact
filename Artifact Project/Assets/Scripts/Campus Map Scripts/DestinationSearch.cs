using UnityEngine;
using System.Collections;

public class DestinationSearch : MonoBehaviour {
	private string[] destinations;
	// Use this for initialization
	void Start () {
		string [] destinations = {"914 Broadway","924 Broadway","Academic Advising Center: Students in Transition","Aden Hall",
			"Administrative and Research Center","Andrews Hall","Armory","Arnett Hall","Arts and Sciences Finance and Payroll Administration",
			"Arts and Sciences Office Building 1","Athens Court","Athens North Court","Athletics Practice Field","Baker Hall","Balch Fieldhouse",
			"Bear Creek Apartments at Williams Village","Benson Earth Sciences","Brackett Hall","Bruce Curtis Building (Museum Collections)",
			"Buckingham Hall","Business Field","Business, Leeds School of Business, BUS","CU Book Store","CU Heritage Center","Carlson Gymnasium",
			"Center for Asian Studies","Center for Astrophysics and Space Astronomy","Center for Community","Chancellor's Residence,PRES,President's Residence",
			"Cheyenne Arapaho Hall","Clare Small Arts and Sciences","Cockerell Hall","College Inn","Computing Center","Continuing Education",
			"Cooperative Institute for Research in Environmental Sciences","Coors Events/Conference Center","Cristol Chemistry and Biochemistry","Crosman Hall",
			"Dal Ward Athletic Center","Dalton Trumbo Fountain Court","Darley Commons","Darley Towers","Denison Arts and Sciences","Discovery Learning Center",
			"Duane Physics and Astrophysics","Eaton Humanities","Economics","Education","Ekeley Sciences","Engineering Center","Environmental Design",
			"Environmental Health and Safety Center ","Euclid Avenue AutoPark","Faculty-Staff Court","Family Housing Children's Center - Newton Court",
			"Farrand Field","Farrand Hall","Fiske Planetarium and Science Center","Fleming Building","Folsom Field","Franklin Field","Gamow Tower",
			"Gates Woodruff Women's Studies Cottage","Gold Biosciences Building","Grounds and Recycling","Guggenheim Geography","Hale Science",
			"Hallett Hall","Health Physics Laboratory","Hellems Arts and Sciences","Housing System Maintenance Center","Housing System Service Center",
			"Humanities","Imig Music","Institute for Behavioral Genetics ","Institute of Behavioral Science ","Institute of Behavioral Science No. 02",
			"Institute of Behavioral Science No. 04 ","Institute of Behavioral Science No. 06","Institute of Behavioral Science No. 07 ",
			"Institute of Behavioral Science No. 09","Institute of Behavioral Science No. 10","Integrated Teaching and Learning Laboratory",
			"International English Center","JILA","Jennie Smoly Caruthers Biotec Building","Ketchum Arts and Sciences","Kittredge Central",
			"Kittredge Field","Kittredge West Hall","Koelbel Building","Koenig Alumni Center","LASP Space Technology Research Center","Lesser House",
			"Libby Hall","MacAllister Building","Macky Auditorium","Marine Court","Mary Rippon Theatre","Mathematics Building","McKenna Languages",
			"Muenzinger Psychology","Museum Collections ","Museum of Natural History","Newton Court","Norlin Library","Norlin Quadrangle","Old Main",
			"Page Foundation Center","Police and Parking Services","Porter Biosciences","Potts Field","Power House","Prentup Field","Ramaley Biology",
			"Reed Hall","Regent Administrative Center","Regent Drive AutoPark","Research Laboratory No. 1, Litman","Research Laboratory No. 2, RL2",
			"Research Laboratory No. 4, Life Science","Research Laboratory No. 6, Marine Street Science Center","Research Park Greenhouse","Roser ATLAS Center",
			"Science Learning Laboratory ","Sewall Hall","Smiley Court","Smith Hall","Sommers-Bausch Observatory","Space Science Building",
			"Speech, Language, and Hearing Sciences","Stadium Ticket Building","Stearns Towers ","Student Recreation Center","Technology Learning Center",
			"Temporary Building No. 1","Temporary Building No. 82","Transportation Center and Annex","University Administrative Center","University Administrative Center Annex",
			"University Club","University Memorial Center","University Residence","University Theatre","Varsity Lake and Bridge","Visual Arts Complex",
			"Wardenburg Student Health Center","Willard Hall","Williams Village 2","Williams Village Field","Wolf Law","Woodbury Arts and Sciences "};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}