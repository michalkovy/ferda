using System;
using Ferda.Modules;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Provides basic method for instanciate exceptions
    /// joint with attribute.
    /// </summary>
    public class Exceptions
    {
        /// <summary>
        /// Gets the categories in attribute disjunctivity error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns></returns>
        public static BadParamsError AttributeCategoriesDisjunctivityError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "",
                restrictionTypeEnum.AttributeCategoriesDisjunctivityError
                );
        }
    }

    /// <summary>
    /// This exception extends <see cref="T:System.InvalidOperationException"/>.
    /// </summary>
    public class IntervalsNotAllowedException : InvalidOperationException
    {
    }

    /// <summary>
    /// Disjunctivity is broken i.e. some objects are in collision.
    /// </summary>
    public class DisjunctivityCollisionException : ArgumentException
    {
        private string[] _objectsInCollision;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.DisjunctivityCollisionException"/> class.
        /// </summary>
        /// <param name="objectsInCollision">The objects in collision.</param>
        public DisjunctivityCollisionException(string[] objectsInCollision)
        {
            _objectsInCollision = objectsInCollision;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.DisjunctivityCollisionException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public DisjunctivityCollisionException(Exception innerException)
            : base("", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.DisjunctivityCollisionException"/> class.
        /// </summary>
        public DisjunctivityCollisionException()
        {
        }
    }

    /// <summary>
    /// Order can not be determined i.e. neither x &lt; y nor x &gt; y.
    /// </summary>
    public class NotComparableCollisionException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.NotComparableCollisionException"/> class.
        /// </summary>
        public NotComparableCollisionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.NotComparableCollisionException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public NotComparableCollisionException(Exception innerException)
            : base("", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.NotComparableCollisionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotComparableCollisionException(string message)
            : base(message)
        {
        }
    }
}