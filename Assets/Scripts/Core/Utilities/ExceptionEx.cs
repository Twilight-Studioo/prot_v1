#region

using System;

#endregion

namespace Core.Utilities
{
    public class AlreadyAddedException : Exception
    {
        public AlreadyAddedException(string message) : base(message)
        {
        }
    }
}