namespace AMS.Helpers
{
    // Shared by every amount field in the app via the AmountUnitEntry control: lets a user type
    // "2.5" and pick "Billion" instead of typing "2500000000" by hand, to cut down on zero-counting
    // mistakes on large currency amounts. "Units (1)" is always first/default so that not touching
    // the dropdown never changes the number a user typed.
    public class AmountUnitOption
    {
        public string Label { get; set; }
        public double Multiplier { get; set; }
        public override string ToString() => Label;
    }

    public static class AmountUnits
    {
        public static readonly AmountUnitOption[] Options =
        {
            new AmountUnitOption { Label = "Units (1)", Multiplier = 1 },
            new AmountUnitOption { Label = "Thousand (1,000)", Multiplier = 1_000 },
            new AmountUnitOption { Label = "Lac (100,000)", Multiplier = 100_000 },
            new AmountUnitOption { Label = "Million (1,000,000)", Multiplier = 1_000_000 },
            new AmountUnitOption { Label = "Billion (1,000,000,000)", Multiplier = 1_000_000_000 },
        };
    }
}
