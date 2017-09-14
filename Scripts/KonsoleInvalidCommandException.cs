using UnityEngine;

namespace KABBOUCHI
{
	public class KonsoleInvalidkommandException : UnityException
	{
		public KonsoleInvalidkommandException(string message = "Command is not valid") : base(message)
		{

		}
	}
}