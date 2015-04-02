using NUnit.Framework; 
using System; 
using UnityTest; 
using UnityEngine;



public class InputControllerTest
{
	
	[Test]
	public void InputControllerWorks() 
	{
		bool touch; 
		float touch_count= Input.touchCount; 
		if (touch_count == 0)
			touch = false;
		else {
			touch= true; 
		}

		bool mouse= Input.GetMouseButtonDown(0); 
		bool space= Input.GetKeyDown(KeyCode.Space); 

		Assert.That (touch==false); 
		Assert.That (mouse==false); 
		Assert.That (space==false); 
		
		
	}
	
}