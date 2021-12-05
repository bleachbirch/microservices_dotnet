using Newtonsoft.Json;
using System.Net;
using T = System.Timers;

public class EventSubcriber
{
    private readonly string _loyaltyProgramHost;
    private long _start = 0;
    private int _chunkSize = 100;
    private readonly T.Timer _timer;

    public EventSubcriber(string loyaltyProgramHost)
    {
        _loyaltyProgramHost = loyaltyProgramHost;
        _timer = new T.Timer(10 * 1000);
        _timer.AutoReset = false;
        _timer.Elapsed += (_, __) => SubscriptionCycleCallback().Wait();
    }

    private async Task SubscriptionCycleCallback()
    {
        var response = await ReadEvents().ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            await HandleEventsAsync(response.Content);
        }
        _timer.Start();
    }

    private async Task HandleEventsAsync(HttpContent content)
    {
        var lastSucceededEvent = 0L;
        var events = JsonConvert.DeserializeObject<IEnumerable<SpecialOffersEvent>>(await content.ReadAsStringAsync());
        if(events == null)
        {
            return;
        }

        foreach (var ev in events)
        {
            dynamic eventData = ev.Content;
            if (ShouldSendNotification(eventData))
            {
                var notificationSucceeded = await SendNotification(eventData).ConfigureAwait(false);
                if (!notificationSucceeded)
                {
                    return;
                }
            }
            lastSucceededEvent = ev.SequenceNumber + 1;
        }
        await WriteStartNumber(lastSucceededEvent).ConfigureAwait(false);
    }

    private async Task WriteStartNumber(long lastSucceededEvent)
    {
        //записываем начальный номер в базу данных
    }

    private async Task<bool> SendNotification(dynamic eventData)
    {
        //Отправление с помощью httpClient
        //Если все ок, возвращает true
        return true;
    }

    private bool ShouldSendNotification(dynamic eventData)
    {
        //определение на основе бизнес-правил
        return true;
    }

    private async Task<HttpResponseMessage> ReadEvents()
    {
        var startNumber = await ReadStartNumber().ConfigureAwait(false);
        using(var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri($"http://{_loyaltyProgramHost}");
            var resource = $"/events/?start={startNumber}&end={_start + _chunkSize}";
            var response = await httpClient.GetAsync(resource).ConfigureAwait(false);
            return response;
        }
    }

    private async Task<long> ReadStartNumber()
    {
        //Читаем начальный номер из базы
        return 0L;
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }
}