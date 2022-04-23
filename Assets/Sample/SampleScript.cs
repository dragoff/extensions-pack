using System;
using System.Collections.Generic;
using ExtensionsPack;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class SampleScript : MonoBehaviour
{

	private List<string> stringsList;
	private Color color;
	void Start()
	{
		float rangeOneStart = 500, rangeOneEnd = 1000;
		float rangeTwoStart = 0.5f, rangeTwoEnd = 1.0f;

// Choose an arbitrary number within range one
		float numberInRangeOne = 750;

		var remapped =numberInRangeOne.LinearRemap(rangeOneStart, rangeOneEnd, rangeTwoStart, rangeTwoEnd);
		Debug.Log(remapped);

		void Foo()
		{
			
		}
	}
}