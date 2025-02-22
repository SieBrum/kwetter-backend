﻿using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    /// <summary>
    /// The default message handler. Emits the received message data as <see cref="byte[]"/>
    /// </summary>
    public interface IMessageHandler
    {
        Task HandleMessageAsync(string messageType, ReadOnlyMemory<byte> obj);
    }

    /// <summary>
    /// Typed variant of <see cref="IMessageHandler"/>. This serializes the <see cref=ReadOnlyMemory{byte}"/> of <see cref="IMessageHandler.HandleMessageAsync(string, byte[])"/> into <typeparamref name="TMessage"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageHandler<TMessage> : IMessageHandler
        where TMessage : class
    {
        /// <summary>
        /// Default interface implementation of <see cref="IMessageHandler.HandleMessageAsync(string, ReadOnlyMemory{byte})"/>
        /// </summary>
        Task IMessageHandler.HandleMessageAsync(string messageType, ReadOnlyMemory<byte> obj)
        {
            return HandleMessageAsync(messageType, JsonSerializer.Deserialize<TMessage>(obj.Span));
        }

        Task HandleMessageAsync(string messageType, TMessage message);
    }
}
