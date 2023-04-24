var lines = File.ReadAllLines("events.csv");
var events = lines.Select(l =>
{
    var dates = l.Split(',');
    return
    (
        StartDate: DateTime.Parse(dates[0]),
        EndDate: DateTime.TryParse(dates[1], out var endDate) ? endDate : DateTime.MaxValue
    );
});
var startDate = events.OrderBy(e => e.StartDate).First().StartDate;
Enumerable.Range(0, (DateTime.Today - startDate).Days + 1)
    .Select(d =>
    {
        var date = startDate.AddDays(d);
        var day = new DateTime(date.Year, date.Month, date.Day);
        return
        (
            Day: day,
            Events: events.Count(e => e.StartDate <= day && e.EndDate >= day.AddDays(1))
        );
    })
    .Where(eventDay => eventDay.Events > 0)
    .Select(eventDay => $"{eventDay.Day:yyyy-MM-dd}: {eventDay.Events}")
    .ToList()
    .ForEach(Console.WriteLine);
