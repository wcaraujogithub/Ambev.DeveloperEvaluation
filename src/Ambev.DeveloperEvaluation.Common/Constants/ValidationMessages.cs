namespace Ambev.DeveloperEvaluation.Common.Constants
{
    public class ValidationMessages
    {
        public static class Sale
        {
            public const string ProductItensRequired = "There must be at least one product for sale.";
            public const string UnitPricesRequired = "Unit price must be greater than zero.";
        }

        public static class General
        {
            public const string FieldIsRequired = "This field is required.";
            public const string InvalidFormat = "Invalid format.";
            public const string MinimumLength = "Must be at least 3 characters long.";
            public const string MaximumLength = "Cannot be longer than 50 characters.";
            public const string InclusiveBetween = "The quantity must be between 1 and 20.";

        }
    }
}
