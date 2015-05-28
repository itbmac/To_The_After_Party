using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

	public static IEnumerable<T> AsRandom<T>(this IEnumerable<T> sequence)
	{
		T[] retArray = sequence.ToArray();
		
		for (int i = 0; i < retArray.Length - 1; i += 1)
		{
			int swapIndex = Random.Range(i, retArray.Length);
			
			if (swapIndex != i) {
				// don't waste time swapping an object to its current position
				
				T temp = retArray[i];
				retArray[i] = retArray[swapIndex];
				retArray[swapIndex] = temp;
			}
		}
		
		return retArray;
	}
}
