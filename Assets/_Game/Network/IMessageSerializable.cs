namespace Network
{
    /// <summary>Represents a type that can be added to and retrieved from 
    /// messages using the <see cref="Message.AddSerializable{T}(T)"/> and 
    /// <see cref="Message.GetSerializable{T}"/> methods.</summary>
    public interface IMessageSerializable
    {
        /// <summary>Adds the type to the message.</summary>
        /// <param name="message">The message to add the type to.</param>
        void Serialize(Message message);

        /// <summary>Retrieves the type from the message.</summary>
        /// <param name="message">The message to retrieve the type from.</param>
        void Deserialize(Message message);
    }
}
