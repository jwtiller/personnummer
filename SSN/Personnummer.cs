﻿using System;
using System.Text.RegularExpressions;

namespace SSN
{
    public record Personnummer
    {
        public int? Day { get; private set; }
        public int? Month { get; private set; }
        public int? Year { get; private set; }
        public int? IndividualNumber { get; private set; }
        public int? CheckDigit { get; private set; }
        public Gender Gender { get; private set; }
        public Personnummer(string ssn)
        {
            Parse(ssn);
        }

        public bool IsValid()
        {
            if (!Regex.IsMatch(ToString(), "[0-9]{11}", RegexOptions.Singleline))
                return false;
            if (!IsCheckDigitValid())
                return false;
            return true;
        }

        public bool IsCheckDigitValid()
        {
            var c1 = 11 - ((3 * GetDigit(0) + 7 * GetDigit(1) + 6 * GetDigit(2) + 1 * GetDigit(3) + 8 * GetDigit(4) + 9 * GetDigit(5) + 4 * GetDigit(6) + 5 * GetDigit(7) + 2 * GetDigit(8)) % 11);
            var c2 = 11 - ((5 * GetDigit(0) + 4 * GetDigit(1) + 3 * GetDigit(2) + 2 * GetDigit(3) + 7 * GetDigit(4) + 6 * GetDigit(5) + 5 * GetDigit(6) + 4 * GetDigit(7) + 3 * GetDigit(8) + 2 * c1) % 11);

            if (c1 == 11) // special rule
                c1 = 0;
            if (c2 == 11)
                c2 = 0; 

            int.TryParse($"{c1}{c2}", out int expected);
            return CheckDigit == expected;
        }

        private int GetDigit(int index)
        {
            return Convert.ToInt32(ToString().Substring(index, 1));
        }

        private void Parse(string ssn)
        {
            if (Regex.IsMatch(ssn, "[0-9]{11}",RegexOptions.Singleline))
            {
                if (int.TryParse(ssn.Substring(0, 2), out var day))
                    Day = day;
                if (int.TryParse(ssn.Substring(2, 2), out var month))
                    Month = month;
                if (int.TryParse(ssn.Substring(4, 2), out var year))
                    Year = year;
                if (int.TryParse(ssn.Substring(6, 3), out var individualNumber))
                    IndividualNumber = individualNumber;
                if (int.TryParse(ssn.Substring(9, 2), out var checkDigit))
                    CheckDigit = checkDigit;

                if (int.TryParse(ssn.Substring(9, 1), out var genderDigit))
                    Gender = genderDigit % 2 == 0 ? Gender.Male : Gender.Female;

            }
        }

        public override string ToString() => $"{Day?.ToString("D2")}{Month?.ToString("D2")}{Year?.ToString("D2")}{IndividualNumber?.ToString("D3")}{CheckDigit?.ToString("D2")}";
    }
}
