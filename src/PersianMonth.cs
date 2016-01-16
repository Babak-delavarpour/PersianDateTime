using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;


namespace Exirsoft
{
    /// <summary>
    /// Represents a year/month in Persian calendar.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Month = {ToString()}, StartDate = {StartDate.ToString(\"yyyy/MM/dd\")}, EndDate = {EndDate.ToString(\"yyyy/MM/dd\")}")]
    public struct PersianMonth : IComparable, IFormattable, IConvertible, ICloneable, ISerializable, IComparable<PersianMonth>, IEquatable<PersianMonth>
    {
        #region Properties

        const string VALUE_FIELD = "_value";
        const string STANDARD_FORMAT = "yyyymm";

        const int MAX_YEAR = 9378;
        const int MIN_YEAR = 1;

        const int MAX_MONTH = 12;
        const int MIN_MONTH = 1;

        const int MAX_VALUE = 937810;
        const int MIN_VALUE = MIN_YEAR * 100 + MIN_MONTH;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static readonly PersianCalendar CALENDAR = new PersianCalendar();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static readonly string[] MONTHS = new string[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly int _value;


        /// <summary>
        /// Represents month names in a Persian calendar.
        /// </summary>
        public static ReadOnlyCollection<string> MonthNames = Array.AsReadOnly(MONTHS);


        /// <summary>
        /// Represents the largest possible value of PersianMonth.
        /// </summary>
        public static readonly PersianMonth MaxValue = new PersianMonth(MAX_VALUE, false);


        /// <summary>
        /// Represents the smallest possible value of PersianMonth.
        /// </summary>
        public static readonly PersianMonth MinValue = new PersianMonth(MIN_VALUE, false);


        /// <summary>
        /// Gets the year-month value of the PersianMonth instance as an integer.
        /// </summary>
        public int Value
        {
            get
            {
                if (_value == 0)
                    return MIN_VALUE;

                return _value;
            }
        }


        /// <summary>
        /// Gets the name of the month in Persian calendar.
        /// </summary>
        public string Name
        {
            get
            {
                return MONTHS[Month - 1];
            }
        }


        /// <summary>
        /// Gets the starting date of the PersianMonth instance.
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return new DateTime(Year, Month, 1, CALENDAR);
            }
        }


        /// <summary>
        /// Gets the end date of the PersianMonth instance.
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return new DateTime(Year, Month, Days, CALENDAR);
            }
        }


        /// <summary>
        /// Gets number of days in the PersianMonth instance.
        /// </summary>
        public int Days
        {
            get
            {
                return CALENDAR.GetDaysInMonth(Year, Month);
            }
        }


        /// <summary>
        /// Gets the year component of the PersianMonth instance.
        /// </summary>
        public int Year
        {
            get
            {
                return Value / 100;
            }
        }


        /// <summary>
        /// Gets the month component of the PersianMonth instance.
        /// </summary>
        public int Month
        {
            get
            {
                return Value % 100;
            }
        }


        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the PersianMonth structure to the specified year, month in Persian calendar.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        public PersianMonth(int year, int month)
        {
            Contract.Requires(year < MIN_YEAR && year > MAX_YEAR && month < MIN_MONTH && month > MAX_MONTH);

            if (year < MIN_YEAR || year > MAX_YEAR)
                throw new ArgumentOutOfRangeException("year");

            if (month < MIN_MONTH || month > MAX_MONTH)
                throw new ArgumentOutOfRangeException("month");

            Contract.EndContractBlock();

            _value = GetYearMonthValue(year, month);
        }


        /// <summary>
        /// Initializes a new instance of the PersianMonth structure to a specified year-month value in Persian calendar.
        /// </summary>
        /// <param name="val">An integer representing a year-month value in Persian calendar</param>
        public PersianMonth(int val)
            : this(val, true)
        { }


        /// <summary>
        /// Initializes a new instance of the PersianMonth structure to the specified date in Persian calendar.
        /// </summary>
        /// <param name="date">DateTime instance</param>
        public PersianMonth(DateTime date)
        {
            Contract.Requires(date > CALENDAR.MinSupportedDateTime && date < CALENDAR.MaxSupportedDateTime);

            if (date > CALENDAR.MaxSupportedDateTime || date < CALENDAR.MinSupportedDateTime)
                throw new ArgumentOutOfRangeException("date");

            Contract.EndContractBlock();

            _value = GetYearMonthValue(CALENDAR.GetYear(date), CALENDAR.GetMonth(date));
        }


        private PersianMonth(int val, bool validate)
        {
            if (validate)
            {
                Contract.Requires(val > MIN_VALUE && val < MAX_VALUE);

                if (val < MIN_VALUE || val > MAX_VALUE)
                    throw new ArgumentOutOfRangeException("val");

                int year = val / 100;
                int month = val % 100;

                if (year < MIN_YEAR || year > MAX_YEAR || month < MIN_MONTH || month > MAX_MONTH)
                    throw new ArgumentOutOfRangeException("val");

                Contract.EndContractBlock();

                _value = GetYearMonthValue(year, month);
            }
            else
            {
                _value = val;
            }
        }


        private PersianMonth(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            Contract.EndContractBlock();

            bool found = false;
            int value = 0;


            // Get the data
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case VALUE_FIELD:
                        value = Convert.ToInt32(enumerator.Value, CultureInfo.InvariantCulture);
                        found = true;
                        break;

                    default:
                        break;
                }
            }
            if (found)
            {
                _value = value;
            }
            else
            {
                throw new SerializationException("Invalid serialization data.");
            }
        }


        #endregion

        #region Class Methods

        /// <summary>
        /// Creates a new PersianMonth that is a copy of the current instance.
        /// </summary>
        /// <returns>A new PersianMonth that is a copy of this instance.</returns>
        public PersianMonth Clone()
        {
            return new PersianMonth(_value, false);
        }


        /// <summary>
        /// Converts the value of the current PersianMonth object to its equivalent string representation using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom year and month format string.</param>
        /// <returns>A string representation of value of the current PersianMonth object as specified by format.</returns>
        public string ToString(string format)
        {
            Contract.Requires(format != null && format != "");
            StringBuilder result = new StringBuilder();

            char ch;
            int value,
                tokenLen,
                index = 0,
                len = format.Length;

            format = format ?? STANDARD_FORMAT;

            while (index < len)
            {
                ch = format[index];
                tokenLen = index + 1;

                while (tokenLen < len && format[tokenLen] == ch)
                    tokenLen++;

                tokenLen -= index;

                switch (ch)
                {
                    case 'm':
                    case 'M':

                        // tokenLen == 1 : Month as digits with no leading zero.
                        // tokenLen == 2 : Month as digits with leading zero for single-digit months.
                        // tokenLen == 3 : Month as a three-letter abbreviation.
                        // tokenLen >= 4 : Month as its full name.

                        value = Month;

                        if (tokenLen <= 2)
                        {
                            result.Append(value.ToString("D" + tokenLen, CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            result.Append(MONTHS[value - 1]);
                        }
                        break;

                    case 'y':
                    case 'Y':

                        // y: Always print (year % 100). No leading zero.
                        // yy: Always print (year % 100) with leading zero.
                        // yyy/yyyy/yyyyy/... : Print year value.  No leading zero.

                        value = Year;
                        if (tokenLen <= 2)
                        {
                            result.Append((value % 100).ToString("D" + tokenLen, CultureInfo.InvariantCulture));
                        }
                        else {
                            result.Append(value.ToString("D" + tokenLen, CultureInfo.InvariantCulture));
                        }
                        break;

                    default:
                        result.Append(ch);
                        tokenLen = 1;
                        break;
                }

                index += tokenLen;
            }

            return result.ToString();
        }


        /// <summary>
        /// Returns a new PersianMonth that adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="value">A number of months. The months parameter can be negative or positive.</param>
        /// <returns>An object whose value is the sum of the months represented by this instance.</returns>
        public PersianMonth AddMonths(int value)
        {
            return new PersianMonth(GetYearMonthValue(Year, Month + value), false);
        }


        /// <summary>
        /// Returns a new PersianMonth that adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="value">A number of years. The years parameter can be negative or positive.</param>
        /// <returns>An object whose value is the sum of the years represented by this instance.</returns>
        public PersianMonth AddYears(int value)
        {
            return new PersianMonth(GetYearMonthValue(Year + value, Month), false);
        }


        /// <summary>
        /// Returns a new PersianMonth that adds one month to the value of this instance.
        /// </summary>
        public PersianMonth GetNextMonth()
        {
            return AddMonths(1);
        }


        /// <summary>
        /// Returns a new PersianMonth that reduces one month from the value of this instance.
        /// </summary>
        public PersianMonth GetPreviousMonth()
        {
            return AddMonths(-1);
        }


        /// <summary>
        /// Returns a new PersianMonth that adds one year to the value of this instance.
        /// </summary>
        public PersianMonth GetNextYear()
        {
            return AddMonths(12);
        }


        /// <summary>
        /// Returns a new PersianMonth that reduces one year from the value of this instance.
        /// </summary>
        public PersianMonth GetPreviousYear()
        {
            return AddMonths(-12);
        }


        /// <summary>
        /// Returns a new PersianMonth that represents the first month of the year of this instance.
        /// </summary>
        public PersianMonth GetYearFirstMonth()
        {
            return new PersianMonth(Year, 1);
        }


        /// <summary>
        /// Returns a new PersianMonth that represents the last month of the year of this instance.
        /// </summary>
        public PersianMonth GetYearLastMonth()
        {
            return new PersianMonth(Year, 12);
        }


        /// <summary>
        /// Determines whether current month is a leap month.
        /// </summary>
        /// <returns>True if current month is a leap month.</returns>
        public bool IsLeapMonth()
        {
            return Month == 12 && CALENDAR.IsLeapYear(Year);
        }


        /// <summary>
        /// Gets the name of the month in Persian calendar.
        /// </summary>
        /// <param name="month">Month value between 1 and 12</param>
        /// <returns>The name of the month in Persian calendar.</returns>
        public static string GetMonthName(int month)
        {
            Contract.Requires(month < MIN_MONTH || month > MAX_MONTH);

            if (month < MIN_MONTH || month > MAX_MONTH)
                throw new ArgumentOutOfRangeException("month");

            Contract.EndContractBlock();
            return MONTHS[month - 1];
        }


        /// <summary>
        /// Gets a PersianMonth object that is set to the current date on this computer, expressed as the local time.
        /// </summary>
        public static PersianMonth Now
        {
            get
            {
                return new PersianMonth(DateTime.Now);
            }
        }


        /// <summary>
        /// Converts the string representation of a persian year and month to its PersianMonth equivalent.
        /// </summary>
        /// <param name="val">A string that contains a year and month to convert.</param>
        /// <returns>An object that is equivalent to the year and month contained in val.</returns>
        public static PersianMonth Parse(string val)
        {
            Contract.Requires(val != null);

            PersianMonth month;
            if (TryParse(val, out month))
            {
                return month;
            }

            throw new FormatException("Cannot parse the string specified!");
        }


        /// <summary>
        /// Converts the specified string representation of a persian year and month to its PersianMonth equivalent 
        /// and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="val">A string containing a year and month to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the PersianMonth value equivalent to the year and month 
        /// contained in value, if the conversion succeeded, or PersianMonth.MinValue if the conversion 
        /// failed. The conversion fails if the value parameter is null, is an empty string (""), or 
        /// does not contain a valid string representation of a year and month.
        /// </param>
        /// <returns>true if the val parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string val, out PersianMonth result)
        {
            Contract.Requires(val != null);

            val = val.Trim();
            result = MinValue;

            if (val == null || String.IsNullOrEmpty(val))
                return false;

            int month = 0;
            if (Int32.TryParse(val, out month))
            {
                result = new PersianMonth(month, true);
                return true;
            }

            int len = val.Length, year, index;
            if (len >= 3 && len <= 7)
            {
                index = len - 2;
                if (Int32.TryParse(val.Substring(index, 2), out month))
                {
                    char ch = val[index - 1];
                    if (ch == '/' || ch == '\\' || ch == ' ')
                        index--;

                    if (Int32.TryParse(val.Substring(0, index), out year))
                    {
                        result = new PersianMonth(year, month);
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Returns a value indicating whether two PersianMonth instances have the same date and time value.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>true if the two values are equal; otherwise, false.</returns>
        public static bool Equals(PersianMonth m1, PersianMonth m2)
        {
            if (Object.ReferenceEquals(m2, m1))
                return true;

            return m1._value == m2._value;
        }


        /// <summary>
        /// Compares two instances of PersianMonth and returns an integer that indicates whether the first 
        /// instance is earlier than, the same as, or later than the second instance.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of m1 and m2.
        /// </returns>
        public static int Compare(PersianMonth m1, PersianMonth m2)
        {
            if (m1._value > m2._value)
                return 1;

            if (m1._value < m2._value)
                return -1;

            return 0;
        }


        /// <summary>
        /// Generates a sequence of PersianMonth months within a specified range.
        /// </summary>
        /// <param name="month">The value of the first month in the sequence.</param>
        /// <param name="count">The number of sequential months to generate.</param>
        /// <returns>A sequence of PersianMonth months</returns>
        public static IEnumerable<PersianMonth> Range(PersianMonth month, short count)
        {
            return Enumerable.Range(0, count).Select(t => month.AddMonths(t));
        }


        /// <summary>
        /// Generates a sequence of PersianMonth months within a specified range.
        /// </summary>
        /// <param name="start">The value of the first month in the sequence.</param>
        /// <param name="end">The value of the end month in the sequence.</param>
        /// <returns>A sequence of PersianMonth months</returns>
        public static IEnumerable<PersianMonth> Range(PersianMonth start, PersianMonth end)
        {
            while (start <= end)
            {
                yield return start;
                start++;
            }
        }


        private static int GetYearMonthValue(int year, int month)
        {
            year = year + (int)Math.Floor((month - 1) / 12D);
            month = (month - 1) % 12 + 1;

            if (month <= 0)
                month += 12;

            int value = year * 100 + month;

            if (value > MAX_VALUE || value < MIN_VALUE)
                throw new ArgumentOutOfRangeException();

            return value;
        }


        #endregion

        #region Operators


        /// <summary>
        /// Gets the year-month value of the PersianMonth instance as an integer.
        /// </summary>
        /// <param name="val">PersianMonth instance</param>
        public static implicit operator int(PersianMonth val)
        {
            return val.Value;
        }


        /// <summary>
        /// Gets the PersianMonth representation of the specified year-month value in Persian calendar.
        /// </summary>
        /// <param name="val">An integer representing a year-month value in Persian calendar</param>
        public static implicit operator PersianMonth(int val)
        {
            Contract.Requires(val > MIN_VALUE && val < MAX_VALUE);
            return new PersianMonth(val, true);
        }


        /// <summary>
        /// Gets the starting date of the PersianMonth instance.
        /// </summary>
        /// <param name="val">PersianMonth instance</param>
        public static implicit operator DateTime(PersianMonth val)
        {
            return val.StartDate;
        }


        /// <summary>
        /// Gets the PersianMonth representation of the specified DateTime.
        /// </summary>
        /// <param name="val">DateTime instance</param>
        public static implicit operator PersianMonth(DateTime val)
        {
            Contract.Requires(val > CALENDAR.MinSupportedDateTime && val < CALENDAR.MaxSupportedDateTime);
            return new PersianMonth(val);
        }


        /// <summary>
        /// Determines whether two specified instances of PersianMonth are equal.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>true if m1 and m2 represent the same value; otherwise, false.</returns>
        public static bool operator ==(PersianMonth m1, PersianMonth m2)
        {
            return Equals(m1, m2);
        }


        /// <summary>
        /// Determines whether two specified instances of PersianMonth are not equal.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>true if m1 and m2 do not represent the same value; otherwise, false.</returns>
        public static bool operator !=(PersianMonth m1, PersianMonth m2)
        {
            return !Equals(m1, m2);
        }


        /// <summary>
        /// Determines whether one specified PersianMonth is later than another specified PersianMonth.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>true if m1 is later than m2; otherwise, false.</returns>
        public static bool operator >(PersianMonth m1, PersianMonth m2)
        {
            return m1._value > m2._value;
        }


        /// <summary>
        /// Determines whether one specified PersianMonth represents a date and time that is the same as or later than another specified PersianMonth.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>true if m1 is the same as or later than m2; otherwise, false.</returns>
        public static bool operator >=(PersianMonth m1, PersianMonth m2)
        {
            return m1._value >= m2._value;
        }


        /// <summary>
        /// Determines whether one specified PersianMonth is earlier than another specified PersianMonth.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>true if m1 is earlier than m2; otherwise, false.</returns>
        public static bool operator <(PersianMonth m1, PersianMonth m2)
        {
            return m1._value < m2._value;
        }


        /// <summary>
        /// Determines whether one specified PersianMonth represents a date and time that is the same as or earlier than another specified PersianMonth.
        /// </summary>
        /// <param name="m1">The first object to compare.</param>
        /// <param name="m2">The second object to compare.</param>
        /// <returns>true if m1 is the same as or later than m2; otherwise, false.</returns>
        public static bool operator <=(PersianMonth m1, PersianMonth m2)
        {
            return m1._value <= m2._value;
        }


        /// <summary>
        /// Returns a new PersianMonth that adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="pmonth">PersianMonth instance</param>
        /// <param name="month">A number of months. The months parameter can be negative or positive.</param>
        /// <returns>An object whose value is the sum of the months represented by this instance.</returns>
        public static PersianMonth operator +(PersianMonth pmonth, int month)
        {
            return pmonth.AddMonths(month);
        }


        /// <summary>
        /// Returns a new PersianMonth that adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="month">A number of months. The months parameter can be negative or positive.</param>
        /// <param name="pmonth">PersianMonth instance</param>
        /// <returns>An object whose value is the sum of the months represented by this instance.</returns>
        public static PersianMonth operator +(int month, PersianMonth pmonth)
        {
            return pmonth.AddMonths(month);
        }


        /// <summary>
        /// Returns a new PersianMonth that reduces the specified number of months of the value of this instance.
        /// </summary>
        /// <param name="pmonth">PersianMonth instance</param>
        /// <param name="month">A number of months. The months parameter can be negative or positive.</param>
        /// <returns>An object whose value is the deduction of the months represented by this instance.</returns>
        public static PersianMonth operator -(PersianMonth pmonth, int month)
        {
            return pmonth.AddMonths(-month);
        }


        /// <summary>
        /// Returns a new PersianMonth that reduces the specified number of months of the value of this instance.
        /// </summary>
        /// <param name="month">A number of months. The months parameter can be negative or positive.</param>
        /// <param name="pmonth">PersianMonth instance</param>
        /// <returns>An object whose value is the deduction of the months represented by this instance.</returns>
        public static PersianMonth operator -(int month, PersianMonth pmonth)
        {
            return pmonth.AddMonths(-month);
        }


        /// <summary>
        /// Subtracts a specified PersianMonth from another specified PersianMonth and returns months difference.
        /// </summary>
        /// <param name="p1">The PersianMonth value to subtract from.</param>
        /// <param name="p2">The PersianMonth value to subtract.</param>
        /// <returns>The months difference between p1 and p2; that is, p1 minus p2.</returns>
        public static int operator -(PersianMonth p1, PersianMonth p2)
        {
            return p1.Value - p2.Value;
        }


        /// <summary>
        /// Returns a new PersianMonth that adds one month to the value of this instance.
        /// </summary>
        /// <param name="month">PersianMonth instance</param>
        /// <returns>An object whose value is one month greater than this instance.</returns>
        public static PersianMonth operator ++(PersianMonth month)
        {
            return month.AddMonths(1);
        }


        /// <summary>
        /// Returns a new PersianMonth that reduces one month of the value of this instance.
        /// </summary>
        /// <param name="month">PersianMonth instance</param>
        /// <returns>An object whose value is one month less than this instance.</returns>
        public static PersianMonth operator --(PersianMonth month)
        {
            return month.AddMonths(-1);
        }


        #endregion

        #region Implementations

        #region Overridden Methods

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Value;
        }


        /// <summary>
        /// Converts the value of the current PersianMonth object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the value of the current PersianMonth object.</returns>
        public override string ToString()
        {
            return ToString("yyyy/mm");
        }


        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>true if value is an instance of PersianMonth and equals the value of this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PersianMonth))
                return false;

            return Equals(this, (PersianMonth)obj);
        }


        #endregion

        #region IComparable

        int IComparable.CompareTo(object obj)
        {
            if (obj is PersianMonth)
                return Compare(this, (PersianMonth)obj);

            return 1;
        }


        /// <summary>
        /// Compares the value of this instance to a specified PersianMonth value and
        /// returns an integer that indicates whether this instance is earlier than, the
        /// same as, or later than the specified PersianMonth value.
        /// </summary>
        /// <param name="value">The object to compare to the current instance.</param>
        /// <returns>A signed number indicating the relative values of this instance and the value parameter.</returns>
        public int CompareTo(PersianMonth value)
        {
            return Compare(this, value);
        }

        #endregion

        #region IFormattable

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return ToString(format);
        }


        #endregion

        #region IConvertible

        /// <summary>
        /// Returns the System.TypeCode for value type PersianMonth.
        /// </summary>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }


        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }


        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }


        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }


        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }


        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)_value;
        }


        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)_value;
        }


        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return _value;
        }


        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)_value;
        }


        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return _value;
        }


        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)_value;
        }


        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return _value;
        }


        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return _value;
        }


        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return _value;
        }


        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return StartDate;
        }


        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString(STANDARD_FORMAT);
        }


        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this, conversionType, provider);
        }

        #endregion

        #region ICloneable

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region ISerializable

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Contract.Requires(info != null);

            if (info == null)
                throw new ArgumentNullException("info");

            Contract.EndContractBlock();

            info.AddValue(VALUE_FIELD, _value);
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Returns a value indicating whether the value of this instance is equal to the value of the specified PersianMonth instance.
        /// </summary>
        /// <param name="value">The object to compare to this instance.</param>
        /// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
        public bool Equals(PersianMonth value)
        {
            return Equals(this, value);
        }

        #endregion 

        #endregion
    }
}
