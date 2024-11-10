using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.ModelLayer.TransactionDtos
{
    public class TransactionReportFilterDto
{
    private DateTime? _dateFrom;
    private DateTime? _dateTo;

    public DateTime? DateFrom
    {
        get => _dateFrom;
        set => _dateFrom = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
    }

    public DateTime? DateTo
    {
        get => _dateTo;
        set => _dateTo = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
    }

    public bool IsSuccess { get; set; }
}

}