using NUnit.Framework; 
using System; 
using UnityTest; 


public class InputControllerTests 
{
	
	[Test]
	public void InputControllerWorks() 
	{
		
		bool test_input = false; 
		int touch_count = 1; 
		
		
		if (test_input==false && touch_count > 0 ){
			test_input=true; 
		}
		
		touch_count = 0; 
		
		Assert.AreEqual(test_input, true); 
		
		if (test_input==true && touch_count==0 ) {
			test_input= false; 
		}
		
		Assert.That (test_input, false); 
		
		
	}
	
}