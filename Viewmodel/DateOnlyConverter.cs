// File: Helpers/DateOnlyConverter.cs
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

public class DateOnlyConverter : ITypeConverter
{
    private readonly string _format;

    public DateOnlyConverter(string format)
    {
        _format = format;
    }

    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateOnly.TryParseExact(text, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }

        throw new FormatException($"Cannot parse '{text}' to DateOnly.");
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return ((DateOnly)value).ToString(_format, CultureInfo.InvariantCulture);
    }
}
namespace school_vacinaton_portal_backend.Viewmodel
{
    public class DateOnlyConverter
    {
    }
}
