using System;
using System.Collections.Generic;
using vibe.Messages;

namespace vibe.Services
{
    public class Messenger
    {
        private Dictionary<Type, HashSet<Delegate>> subscribers = [];

        public void Subscribe<TMessage>(Action<TMessage> onReceived)
        {
            if (!this.subscribers.TryGetValue(typeof(TMessage), out var messageSubscribers))
            {
                messageSubscribers = [];
                subscribers.Add(typeof(TMessage), messageSubscribers);
            }

            messageSubscribers.Add(onReceived);
        }

        public void Unsubscribe<TMessage>(Action<TMessage> unsubscribed)
        {
            if (this.subscribers.TryGetValue(typeof(TMessage), out var messageSubscribers))
            {
                messageSubscribers.Remove(unsubscribed);
            }
        }

        public void Send<TMessage>(TMessage message)
        {
            if (this.subscribers.TryGetValue(typeof(TMessage), out var messageSubscribers))
            {
                foreach (var subscriber in messageSubscribers)
                {
                    subscriber.DynamicInvoke(message);
                }
            }
        }
    }
}
