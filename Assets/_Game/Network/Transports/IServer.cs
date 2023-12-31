﻿using System;

namespace Network.Transports
{
    /// <summary>Defines methods, properties, and events which every transport's server must implement.</summary>
    public interface IServer : IPeer
    {
        /// <summary>Invoked when a connection is established at the transport level.</summary>
        event EventHandler<ConnectedEventArgs> Connected;

        /// <inheritdoc cref="Server.Port"/>
        ushort Port { get; }
        
        /// <summary>Starts the transport and begins listening for incoming connections.</summary>
        /// <param name="port">The local port on which to listen for connections.</param>
        void Start(ushort port);
        
        /// <summary>Closes an active connection.</summary>
        /// <param name="connection">The connection to close.</param>
        void Close(Connection connection);

        /// <summary>Closes all existing connections and stops listening for new connections.</summary>
        void Shutdown();
    }
}
