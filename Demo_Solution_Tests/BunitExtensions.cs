using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;

namespace Demo_Solution_Tests
{
	public static class BunitExtensions
	{
		public static void Type(this IElement element, string text) 
		{
			var charArr = text.ToCharArray();
			foreach (var letter in charArr) 
			{
				element.KeyDown(letter);
				element.KeyUp(letter);
			}
		}

        public static IEnumerable<T> Add<T>(this IEnumerable<T> source, T item)
        {
            foreach (var element in source)
                yield return element;
            yield return item;
        }
    }
}
