using NetCore.Donation.WebClient;
using Radzen;
using Radzen.Blazor;

namespace NetCore.Donation.Admin;

public static class RadzenODataQuery
{
    public static ODataListRequest From<TItem>(LoadDataArgs args, RadzenDataFilter<TItem>? filter, string select)
    {
        var odataFilter = args.Filter;
        if (filter?.Filters?.Any() == true)
        {
            var fromFilter = filter.ToODataFilterString();
            if (!string.IsNullOrWhiteSpace(fromFilter))
            {
                odataFilter = string.IsNullOrWhiteSpace(odataFilter)
                    ? fromFilter
                    : $"({fromFilter}) and ({odataFilter})";
            }
        }

        return new ODataListRequest
        {
            Select = select,
            Filter = odataFilter,
            Top = args.Top,
            Skip = args.Skip,
            OrderBy = args.OrderBy,
            Count = true,
        };
    }

    public static string ShortId(Guid id) => id.ToString("N")[..8];

    public static string ShortId(Guid? id) => id is { } value ? ShortId(value) : "—";
}
