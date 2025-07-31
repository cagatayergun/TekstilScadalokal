// Services/LiveEventAggregator.cs
using System;
using System.Collections.Generic;
using TekstilScada.Models;

namespace TekstilScada.Services
{
    // Olay verisini taşımak için basit bir sınıf
    public class LiveEvent
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; } // Kaynak (Makine Adı, Sistem vb.)
        public string Message { get; set; }
        public EventType Type { get; set; }

        public override string ToString()
        {
            return $"{Timestamp:HH:mm:ss} | {Source} | {Message}";
        }
    }

    public enum EventType
    {
        Alarm,
        Process,
        SystemInfo,
        SystemWarning,
        SystemSuccess
    }

    // Olayları toplayan ve yayınlayan merkezi servis
    public class LiveEventAggregator
    {
        // Singleton deseni: Programda bu sınıftan sadece bir tane olacak
        private static readonly LiveEventAggregator _instance = new LiveEventAggregator();
        public static LiveEventAggregator Instance => _instance;

        // Yeni bir olay geldiğinde tetiklenecek olan event
        public event Action<LiveEvent> OnEventPublished;

        private LiveEventAggregator() { }

        public void Publish(LiveEvent liveEvent)
        {
            // Olayı tüm dinleyicilere yayınla
            OnEventPublished?.Invoke(liveEvent);
        }

        // Yardımcı metotlar
        public void PublishAlarm(string machineName, string message)
        {
            Publish(new LiveEvent { Timestamp = DateTime.Now, Source = machineName, Message = message, Type = EventType.Alarm });
        }
        public void PublishProcessEvent(string machineName, string message)
        {
            Publish(new LiveEvent { Timestamp = DateTime.Now, Source = machineName, Message = message, Type = EventType.Process });
        }
        public void PublishSystemInfo(string message)
        {
            Publish(new LiveEvent { Timestamp = DateTime.Now, Source = "Sistem", Message = message, Type = EventType.SystemInfo });
        }
    }
}
