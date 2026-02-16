using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.API.Data.ValueConverters;

public class DateTimeToChar8Converter : ValueConverter<DateTime, string>
{
    public DateTimeToChar8Converter() : base(
        dateTime => dateTime.ToString("yyyyMMdd",CultureInfo.InvariantCulture),
        stringVal => DateTime.ParseExact(stringVal, "yyyyMMdd", CultureInfo.InvariantCulture))
    {
    }

}