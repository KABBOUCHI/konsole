using UnityEngine;

namespace KABBOUCHI
{
    public class KonsoleInvalidCommandException : UnityException
    {
        public KonsoleInvalidCommandException(string message = "Command is not valid") : base(message)
        {

        }
    }
}