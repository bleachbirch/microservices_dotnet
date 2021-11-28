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
        var events = JsonConvert.DeserializeObject<IEnumerable<SpecialOffersEvent>>(await content.ReadAsStringAsync());
        if(events == null)
        {
            return;
        }

        foreach (var ev in events)
        {
            dynamic eventData = ev.Content;
            //Обрабатываем события 'ev' посредством eventData
            _start = Math.Max(_start, ev.SequenceNumber + 1);
        }
    }

    private async Task<HttpResponseMessage> ReadEvents()
    {
        using(var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri($"http://{_loyaltyProgramHost}");
            var response = await httpClient.GetAsync($"/events/?start={_start}&end={_start + _chunkSize}").ConfigureAwait(false);
            return response;
        }
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