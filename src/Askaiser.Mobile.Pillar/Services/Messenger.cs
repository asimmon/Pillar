using System;
using Askaiser.Mobile.Pillar.Interfaces;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Services
{
    public sealed class Messenger : IMessenger
    {
        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args)
            where TSender : class
        {
            MessagingCenter.Send(sender, message, args);
        }

        public void Send<TSender>(TSender sender, string message)
            where TSender : class
        {
            MessagingCenter.Send(sender, message);
        }

        public void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback, TSender source = default(TSender))
            where TSender : class
        {
            MessagingCenter.Subscribe(subscriber, message, callback, source);
        }

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = default(TSender))
            where TSender : class
        {
            MessagingCenter.Subscribe(subscriber, message, callback, source);
        }

        public void Unsubscribe<TSender, TArgs>(object subscriber, string message)
            where TSender : class
        {
            MessagingCenter.Unsubscribe<TSender, TArgs>(subscriber, message);
        }

        public void Unsubscribe<TSender>(object subscriber, string message)
            where TSender : class
        {
            MessagingCenter.Unsubscribe<TSender>(subscriber, message);
        }
    }
}