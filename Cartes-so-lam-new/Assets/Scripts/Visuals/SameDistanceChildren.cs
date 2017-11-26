using UnityEngine;
using System.Collections;

// place first and last elements in children array manually
// others will be placed automatically with equal distances between first and last elements
public class SameDistanceChildren : MonoBehaviour
{
	public Transform[] Children;

	// Use this for initialization
	private void Awake()
	{
		var firstElementPos = Children[0].position;
		var lastElementPos = Children[Children.Length - 1].position;

		// dividing by Children.Length - 1 because for example: between 10 points that are 9 segments
		var XDist = (lastElementPos.x - firstElementPos.x) / (float)(Children.Length - 1);
		var YDist = (lastElementPos.y - firstElementPos.y) / (float)(Children.Length - 1);
		var ZDist = (lastElementPos.z - firstElementPos.z) / (float)(Children.Length - 1);

		var Dist = new Vector3(XDist, YDist, ZDist);

		for (var i = 1; i < Children.Length; i++)
		{
			Children[i].transform.position = Children[i - 1].transform.position + Dist;
		}
	}


}
