using System;

namespace MongoDb.Include
{
    /// <summary>
    /// Represents an error for mismatching foreign key
    /// </summary>
    public class ForeignKeyMismatchException : Exception
    {
        public ForeignKeyMismatchException(string message)
            : base(message)
        {
        }
    }
}
