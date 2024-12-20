namespace UserPanel.Models.Password
{

    public class PasswordProtectionForm
    {
        public bool IsUpperCase { get; set; }

        public bool IsSpecialChar { get; set; }

        public bool IsNoRepeat { get; set; }

        public bool IsDigit { get; set; }

        public bool IsLenRange { get; set; }

        public int MinValueLen { get; set; }

        public int MaxValueLen { get; set;}

        public bool IsPassTime { get; set; }


        public int PassTimeValue { get; set; }

    }
}
