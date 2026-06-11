namespace CybsClass.WebApi.Service.Services.Utilities
{
    public static class DecimalTruncate
    {
        public static decimal Truncate(decimal? value, int decimalPlaces)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");
            }

            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces), "Decimal places must be non-negative.");

            try
            {
                decimal nonNullableValue = value.Value; // <-- get the actual decimal
                decimal multiplier = (decimal)Math.Pow(10, decimalPlaces);
                return Math.Truncate(nonNullableValue * multiplier) / multiplier;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while truncating the decimal value.", ex);
            }
        }

    }
}
