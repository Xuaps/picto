﻿using System.Collections.Generic;
using System.Linq;

namespace PictoUI.Messaging
{
    public interface IMessagePump
    {
        void Start();
        void Stop();
    }

    public static class MessagePumpExtensions
    {
        public static void StartAll<T>(this IEnumerable<T> pumps) where T : IMessagePump
        {
            foreach(var pump in pumps ?? Enumerable.Empty<T>())
            {
                pump.Start();
            }
        }

        public static void StopAll<T>(this IEnumerable<T> pumps) where T : IMessagePump
        {
            foreach (var pump in pumps ?? Enumerable.Empty<T>())
            {
                pump.Start();
            }
        }
    }
}
