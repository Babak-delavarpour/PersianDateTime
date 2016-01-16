using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public struct PersianDateTime : IComparable, IFormattable, IConvertible, ISerializable, IComparable<PersianDateTime>, IEquatable<PersianDateTime>
    {
        #region Constants

        static readonly PersianCultureInfo Culture = new PersianCultureInfo();
        static readonly DateTime MinSupportedDateTime = Calendar.MinSupportedDateTime;
        static readonly DateTime MaxSupportedDateTime = Calendar.MaxSupportedDateTime;

        static Calendar Calendar { get { return Culture.Calendar; } }


        /// <summary>
        /// Represents the largest possible value of PersianDateTime. This field is read-only.
        /// </summary>
        public static readonly PersianDateTime MaxValue = MinSupportedDateTime;


        /// <summary>
        /// Represents the smallest possible value of PersianDateTime. This field is read-only.
        /// </summary>
        public static readonly PersianDateTime MinValue = MaxSupportedDateTime;

        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to a specified DateTime.
        /// </summary>
        /// <param name="date">A DateTime instance.</param>
        public PersianDateTime(DateTime date)
            : this(date < MinSupportedDateTime ? MinSupportedDateTime.Ticks : date.Ticks)
        { }


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to a specified number of ticks.
        /// </summary>
        /// <param name="ticks">A date and time expressed in the number of 100-nanosecond intervals that have elapsed since Farvardin 1, 0001 at 00:00:00.000 in the Persian calendar.</param>
        public PersianDateTime(long ticks)
            : this(ticks, DateTimeKind.Unspecified)
        { }


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to a specified number
        /// of ticks and to Coordinated Universal Time (UTC) or local time.
        /// </summary>
        /// <param name="ticks">A date and time expressed in the number of 100-nanosecond intervals that have elapsed since Farvardin 1, 0001 at 00:00:00.000 in the Persian calendar.</param>
        /// <param name="kind">One of the enumeration values that indicates whether ticks specifies a local time, Coordinated Universal Time (UTC), or neither.</param>
        public PersianDateTime(long ticks, DateTimeKind kind)
        {
            if (ticks < MinSupportedDateTime.Ticks || ticks > MaxSupportedDateTime.Ticks)
                throw new ArgumentOutOfRangeException("ticks");

            Contract.EndContractBlock();

            _date = new DateTime(ticks, kind);
        }


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to the specified year, month, and day.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        public PersianDateTime(int year, int month, int day)
            : this(year, month, day, 0, 0, 0, 0, DateTimeKind.Unspecified)
        { }


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to the specified
        /// year, month, day, hour, minute, and second.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        public PersianDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, 0, DateTimeKind.Unspecified)
        { }


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to the specified
        /// year, month, day, hour, minute, and second.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
            : this(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified)
        { }


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to the specified
        /// year, month, day, hour, minute, and second.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="kind">
        /// One of the System.DateTimeKind values that indicates whether year, month,
        /// day, hour, minute, second, and millisecond specify a local time, Coordinated
        /// Universal Time (UTC), or neither.
        /// </param>
        public PersianDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
            : this(year, month, day, hour, minute, second, 0, kind)
        { }


        /// <summary>
        /// Initializes a new instance of the PersianDateTime structure to the specified
        /// year, month, day, hour, minute, and second.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="kind">
        /// One of the System.DateTimeKind values that indicates whether year, month,
        /// day, hour, minute, second, and millisecond specify a local time, Coordinated
        /// Universal Time (UTC), or neither.
        /// </param>
        public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
        {
            _date = new DateTime(year, month, day, hour, minute, second, millisecond, Calendar, kind);
        }


        #endregion

        #region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime _date;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime CurrentDate
        {
            get
            {
                if (_date < MinSupportedDateTime)
                    _date = MinSupportedDateTime;

                return _date;
            }
        }


        /// <summary>
        /// Gets a PersianDateTime object that is set to the current date and time on
        /// this computer, expressed as the local time.
        /// </summary>
        /// <returns>
        /// A PersianDateTime whose value is the current local date and time.
        /// </returns>
        public static PersianDateTime Now
        {
            get { return DateTime.Now; }
        }


        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// <returns>
        /// A PersianDateTime set to today's date, with the time component set to 00:00:00.
        /// </returns>
        public static PersianDateTime Today
        {
            get { return DateTime.Today; }
        }


        /// <summary>
        /// Gets a PersianDateTime object that is set to the current date and time on
        /// this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns>
        /// A PersianDateTime whose value is the current UTC date and time.
        /// </returns>
        public static PersianDateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }


        /// <summary>
        /// Gets the date component of this instance.
        /// </summary>
        /// <returns>
        /// A new PersianDateTime with the same date as this instance, and the time value
        /// set to 12:00:00 midnight (00:00:00).
        /// </returns>
        public PersianDateTime Date
        {
            get { return CurrentDate.Date; }
        }


        /// <summary>
        /// Gets the day of the month represented by this instance.
        /// </summary>
        /// <returns>
        /// The day component, expressed as a value between 1 and 31.
        /// </returns>
        public int Day
        {
            get { return Calendar.GetDayOfMonth(CurrentDate); }
        }


        /// Gets the day of the week represented by this instance.
        /// </summary>
        /// <returns>
        /// The day of the week, expressed as a string value.
        /// </returns>
        public DayOfWeek DayOfWeek
        {
            get { return Calendar.GetDayOfWeek(CurrentDate); }
        }


        /// <summary>
        /// Gets the day of the week represented by this instance.
        /// </summary>
        /// <returns>
        /// The day of the week.
        /// </returns>
        public string PersianDayOfWeek
        {
            get { return Culture.DateTimeFormat.DayNames[(int)this.DayOfWeek]; }
        }


        /// <summary>
        /// Gets the day of the year represented by this instance.
        /// </summary>
        /// <returns>
        /// The day of the year, expressed as a value between 1 and 366.
        /// </returns>
        public int DayOfYear
        {
            get { return Calendar.GetDayOfYear(CurrentDate); }
        }


        /// <summary>
        /// Gets the hour component of the date represented by this instance.
        /// </summary>
        /// <returns>
        /// The hour component, expressed as a value between 0 and 23.
        /// </returns>
        public int Hour
        {
            get { return CurrentDate.Hour; }
        }


        /// <summary>
        /// Gets a value that indicates whether the time represented by this instance is
        /// based on local time, Coordinated Universal Time (UTC), or neither.
        /// </summary>
        public DateTimeKind Kind
        {
            get { return CurrentDate.Kind; }
        }


        /// <summary>
        /// Gets the milliseconds component of the date represented by this instance.
        /// </summary>
        /// <returns>
        /// The milliseconds component, expressed as a value between 0 and 999.
        /// </returns>
        public int Millisecond
        {
            get { return CurrentDate.Millisecond; }
        }


        /// <summary>
        /// Gets the day of the month represented by this instance.
        /// </summary>
        /// <returns>
        /// The day component, expressed as a value between 1 and 31.
        /// </returns>
        public int Minute
        {
            get { return CurrentDate.Minute; }
        }


        /// <summary>
        /// Gets the month component of the date represented by this instance.
        /// </summary>
        /// <returns>
        /// The month component, expressed as a value between 1 and 12.
        /// </returns>
        public int Month
        {
            get { return Calendar.GetMonth(CurrentDate); }
        }


        /// <summary>
        /// Gets the month of the year represented by this instance.
        /// </summary>
        /// <returns>
        /// The month of the year, expressed as a string value.
        /// </returns>
        public string MonthOfYear
        {
            get { return Culture.DateTimeFormat.MonthNames[this.Month - 1]; }
        }


        /// <summary>
        /// Gets the seconds component of the date represented by this instance.
        /// </summary>
        /// <returns>
        /// The seconds, between 0 and 59.
        /// </returns>
        public int Second
        {
            get { return CurrentDate.Second; }
        }


        /// <summary>
        /// Gets the number of ticks that represent the date and time of this instance.
        /// </summary>
        /// <returns>
        /// The number of ticks that represent the date and time of this instance. The
        /// value is between PersianDateTime.MinValue.Ticks and PersianDateTime.MaxValue.Ticks.
        /// </returns>
        public long Ticks
        {
            get { return CurrentDate.Ticks; }
        }


        /// <summary>
        /// Gets the time of day for this instance.
        /// </summary>
        /// <returns>
        /// A System.TimeSpan that represents the fraction of the day that has elapsed
        /// since midnight.
        /// </returns>
        public TimeSpan TimeOfDay
        {
            get { return CurrentDate.TimeOfDay; }
        }


        /// <summary>
        /// Gets the year component of the date represented by this instance.
        /// </summary>
        /// <returns>
        /// The year, between 1 and 9999.
        /// </returns>
        public int Year
        {
            get { return Calendar.GetYear(CurrentDate); }
        }


        #endregion

        #region Operators

        public static implicit operator DateTime(PersianDateTime date)
        {
            return date.CurrentDate;
        }


        public static implicit operator PersianDateTime(DateTime date)
        {
            return new PersianDateTime(date);
        }


        /// <summary>
        /// Subtracts a specified date and time from another specified date and time
        /// and returns a time interval.
        /// </summary>
        /// <param name="d1">A PersianDateTime (the minuend).</param>
        /// <param name="d2">A PersianDateTime (the subtrahend).</param>
        /// <returns>
        /// A System.TimeSpan that is the time interval between d1 and d2; that is, d1 minus d2.
        /// </returns>
        public static TimeSpan operator -(PersianDateTime d1, PersianDateTime d2)
        {
            return d1.CurrentDate - d2.CurrentDate;
        }


        /// <summary>
        /// Subtracts a specified time interval from a specified date and time and returns
        /// </summary>
        /// <param name="d">A PersianDateTime.</param>
        /// <param name="t">A PersianDateTime.</param>
        /// <returns>
        /// A PersianDateTime whose value is the value of d minus the value of t.
        /// </returns>
        public static PersianDateTime operator -(PersianDateTime d, TimeSpan t)
        {
            return d.CurrentDate - t;
        }


        /// <summary>
        /// Adds a specified time interval to a specified date and time, yielding a new
        /// date and time.
        /// </summary>
        /// <param name="d">A PersianDateTime.</param>
        /// <param name="t">A PersianDateTime.</param>
        /// <returns>A PersianDateTime that is the sum of the values of d and t.</returns>
        public static PersianDateTime operator +(PersianDateTime d, TimeSpan t)
        {
            return d.CurrentDate + t;
        }


        /// <summary>
        /// Determines whether one specified PersianDateTime is less than another specified
        /// PersianDateTime.
        /// </summary>
        /// <param name="d1">A PersianDateTime.</param>
        /// <param name="d2">A PersianDateTime.</param>
        /// <returns>true if t1 is less than t2; otherwise, false.</returns>
        public static bool operator <(PersianDateTime d1, PersianDateTime d2)
        {
            return d1.CurrentDate < d2.CurrentDate;
        }


        /// <summary>
        /// Determines whether one specified PersianDateTime is less than or equal to
        /// another specified PersianDateTime.
        /// </summary>
        /// <param name="d1">A PersianDateTime.</param>
        /// <param name="d2">A PersianDateTime.</param>
        /// <returns>
        /// true if t1 is less than or equal to t2; otherwise, false.
        /// </returns>
        public static bool operator <=(PersianDateTime d1, PersianDateTime d2)
        {
            return d1.CurrentDate <= d2.CurrentDate;
        }


        /// <summary>
        /// Determines whether one specified PersianDateTime is greater than another
        /// specified PersianDateTime.
        /// </summary>
        /// <param name="d1">A PersianDateTime.</param>
        /// <param name="d2">A PersianDateTime.</param>
        /// <returns>
        /// true if t1 is greater than t2; otherwise, false.
        /// </returns>
        public static bool operator >(PersianDateTime d1, PersianDateTime d2)
        {
            return d1.CurrentDate > d2.CurrentDate;
        }


        /// <summary>
        /// Determines whether one specified PersianDateTime is greater than or equal
        /// to another specified PersianDateTime.
        /// </summary>
        /// <param name="d1">A PersianDateTime.</param>
        /// <param name="d2">A PersianDateTime.</param>
        /// <returns>
        /// true if t1 is greater than or equal to t2; otherwise, false.
        /// </returns>
        public static bool operator >=(PersianDateTime d1, PersianDateTime d2)
        {
            return d1.CurrentDate >= d2.CurrentDate;
        }


        #endregion

        #region Static Methods


        /// <summary>
        /// Converts the PersianDateTime object to the christian counterpart
        /// </summary>
        /// <param name="date">A PersianDateTime object</param>
        /// <returns>
        /// A DateTime object, which is reperesents the christian counterpart of PersianDateTime object
        /// </returns>
        public static DateTime ToDateTime(PersianDateTime date)
        {
            return date.CurrentDate;
        }


        /// <summary>
        /// Converts the DateTime object to the persian counterpart
        /// </summary>
        /// <param name="date">A DateTime object</param>
        /// <returns>
        /// A PersianDateTime object, which is reperesents the persian counterpart of DateTime object
        /// </returns>
        public static PersianDateTime ToPersianDateTime(DateTime date)
        {
            return new PersianDateTime(date);
        }


        /// <summary>
        /// Compares two instances of PersianDateTime and returns an integer that indicates
        /// whether the first instance is earlier than, the same as, or later than the
        /// second instance.
        /// </summary>
        /// <param name="d1">The first PersianDateTime.</param>
        /// <param name="d2">The second PersianDateTime.</param>
        /// <returns>
        /// A signed number indicating the relative values of t1 and t2.  Value Type
        /// Condition Less than zero t1 is earlier than t2. Zero t1 is the same as t2.
        /// Greater than zero t1 is later than t2.
        /// </returns>
        public static int Compare(PersianDateTime d1, PersianDateTime d2)
        {
            return DateTime.Compare(d1.CurrentDate, d2.CurrentDate);
        }


        /// <summary>
        /// Returns the number of days in the specified month and year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month (a number ranging from 1 to 12).</param>
        /// <returns>
        /// The number of days in month for the specified year.  For example, if month
        /// equals 2 for February, the return value is 28 or 29 depending upon whether
        /// year is a leap year.
        /// </returns>
        public static int DaysInMonth(int year, int month)
        {
            return Calendar.GetDaysInMonth(year, month);
        }


        /// <summary>
        /// Returns the number of days in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>
        /// The number of days in the specified year.  
        /// </returns>
        public static int DaysInYear(int year)
        {
            return IsLeapYear(year) ? 366 : 365;
        }


        /// <summary>
        /// Returns a value indicating whether two instances of PersianDateTime are equal.
        /// </summary>
        /// <param name="d1">The first PersianDateTime instance.</param>
        /// <param name="d2">The second PersianDateTime instance.</param>
        /// <returns>true if the two PersianDateTime values are equal; otherwise, false.</returns>
        public static bool Equals(PersianDateTime d1, PersianDateTime d2)
        {
            return DateTime.Equals(d1.CurrentDate, d2.CurrentDate);
        }


        /// <summary>
        /// Deserializes a 64-bit binary value and recreates an original serialized PersianDateTime
        /// object.
        /// </summary>
        /// <param name="dateData">
        /// A 64-bit signed integer that encodes the PersianDateTime.Kind property in
        /// a 2-bit field and the PersianDateTime.Ticks property in a 62-bit field.
        /// </param>
        /// <returns>
        /// A PersianDateTime object that is equivalent to the PersianDateTime object
        /// that was serialized by the PersianDateTime.ToBinary() method.
        /// </returns>
        public static PersianDateTime FromBinary(long dateData)
        {
            return DateTime.FromBinary(dateData);
        }


        /// <summary>
        /// Converts the specified Windows file time to an equivalent local time.
        /// </summary>
        /// <param name="fileTime">A Windows file time expressed in ticks.</param>
        /// <returns>
        /// A PersianDateTime object that represents a local time equivalent to the date
        /// and time represented by the fileTime parameter.
        /// </returns>
        public static PersianDateTime FromFileTime(long fileTime)
        {
            return DateTime.FromFileTime(fileTime);
        }


        /// <summary>
        /// Converts the specified Windows file time to an equivalent UTC time.
        /// </summary>
        /// <param name="fileTime">A Windows file time expressed in ticks.</param>
        /// <returns>
        /// A PersianDateTime object that represents a UTC time equivalent to the date
        /// and time represented by the fileTime parameter.
        /// </returns>
        public static PersianDateTime FromFileTimeUtc(long fileTime)
        {
            return DateTime.FromFileTimeUtc(fileTime);
        }


        /// <summary>
        /// Returns a PersianDateTime equivalent to the specified OLE Automation Date.
        /// </summary>
        /// <param name="d">An OLE Automation Date value.</param>
        /// <returns>
        /// A PersianDateTime that represents the same date and time as d.
        /// </returns>
        public static PersianDateTime FromOADate(double d)
        {
            return DateTime.FromOADate(d);
        }


        /// <summary>
        /// Returns an indication whether the specified year is a leap year.
        /// </summary>
        /// <param name="year">A 4-digit year.</param>
        /// <returns>
        /// true if year is a leap year; otherwise, false.
        /// </returns>
        public static bool IsLeapYear(int year)
        {
            return Calendar.IsLeapYear(year);
        }


        /// <summary>
        /// Retunrs a PersianDateTime equivalent to the specified String Date.
        /// </summary>
        /// <param name="s">
        /// a date string with or without delimiter eg. 13760302 or 1376/03/02
        /// </param>
        /// <returns>
        /// A PersianDateTime that represents the same date and time as date.
        /// </returns>
        public static PersianDateTime Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            Contract.EndContractBlock();

            return DateTime.Parse(FormatInputDateString(s), Culture);
        }


        /// <summary>
        /// Converts the specified string representation of a date and time to its PersianDateTime
        /// equivalent using the specified format.
        /// The format of the string representation must match the specified format exactly.
        /// </summary>
        /// <param name="s">A string that contains a date and time to convert.</param>
        /// <param name="format">A format specifier that defines the required format of s. For more information, see the Remarks section.</param>
        /// <returns>An object that is equivalent to the date and time contained in s</returns>
        public static PersianDateTime ParseExact(string s, string format)
        {
            return DateTime.ParseExact(FormatInputDateString(s), format, Culture);
        }


        /// <summary>
        /// Converts the specified string representation of a date and time to its PersianDateTime
        /// equivalent using the specified array of formats, and style. The format of the string 
        /// representation must match at least one of the specified formats exactly.
        /// </summary>
        /// <param name="s">A string that contains a date and time to convert.</param>
        /// <param name="formats">An array of allowable formats of s</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <returns>An object that is equivalent to the date and time contained in s, as specified by formats and style.</returns>
        public static PersianDateTime ParseExact(string s, string[] formats, DateTimeStyles style)
        {
            return DateTime.ParseExact(FormatInputDateString(s), formats, Culture, style);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">
        /// a date string with or without delimiter eg. 13760302 101030 or 1376/03/02 10:10:30
        /// </param>
        /// <param name="result">
        /// PersianDateTime object
        /// </param>
        /// <returns>
        /// true if s was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse(string s, out PersianDateTime result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch
            {
                result = default(DateTime);
                return false;
            }
        }


        /// <summary>
        /// Converts the specified string representation of a date and time to its PersianDateTime
        /// equivalent using the specified array of formats, and style. The format of the string 
        /// representation must match at least one of the specified formats exactly. 
        /// The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string that contains a date and time to convert.</param>
        /// <param name="formats">An array of allowable formats of s. See the Remarks section for more information.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <param name="result">
        /// When this method returns, contains the System.DateTime value equivalent to the
        /// date and time contained in s, if the conversion succeeded, or PersianDateTime.MinValue
        /// if the conversion failed. The conversion fails if s or formats is null, s or an element 
        /// of formats is an empty string, or the format of s is not exactly as specified by at least 
        /// one of the format patterns in formats. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the s parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParseExact(string s, string[] formats, DateTimeStyles style, out PersianDateTime result)
        {
            try
            {
                result = ParseExact(s, formats, style);
                return true;
            }
            catch
            {
                result = default(DateTime);
                return false;
            }
        }


        /// <summary>
        /// Converts the specified string representation of a date and time to its PersianDateTime
        /// equivalent using the specified format. 
        /// The format of the string representation must match the specified format exactly. 
        /// The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="format">The required format of s. See the Remarks section for more information.</param>
        /// <param name="result">
        /// When this method returns, contains the System.DateTime value equivalent to the
        /// date and time contained in s, if the conversion succeeded, or PersianDateTime.MinValue
        /// if the conversion failed. The conversion fails if either the s or format parameter
        /// is null, is an empty string, or does not contain a date and time that correspond
        /// to the pattern specified in format. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParseExact(string s, string format, out PersianDateTime result)
        {
            try
            {
                result = ParseExact(s, format);
                return true;
            }
            catch
            {
                result = default(DateTime);
                return false;
            }
        }

        static string FormatInputDateString(string date)
        {
            var days = Culture.DateTimeFormat.DayNames;
            return date
                .Replace("یک شنبه", days[0])
                .Replace("دو شنبه", days[1])
                .Replace("سه‌شنبه", days[2])
                .Replace("چهار شنبه", days[3])
                .Replace("پنجشنبه", days[4]);
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Adds the value of the specified System.TimeSpan to the value of this instance.
        /// </summary>
        /// <param name="value">A System.TimeSpan object that represents a positive or negative time interval.</param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        public PersianDateTime Add(TimeSpan value)
        {
            return Calendar.AddMilliseconds(CurrentDate, value.TotalMilliseconds);
        }


        /// <summary>
        /// Adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="value">A number of whole and fractional days. The value parameter can be negative or positive.</param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the number of days represented by value.
        /// </returns>
        public PersianDateTime AddDays(int value)
        {
            return Calendar.AddDays(CurrentDate, value);
        }


        /// <summary>
        /// Adds the specified number of hours to the value of this instance.
        /// </summary>
        /// <param name="value">A number of whole and fractional hours. The value parameter can be negative or positive.</param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the number of hours represented by value.
        /// </returns>
        public PersianDateTime AddHours(int value)
        {
            return Calendar.AddHours(CurrentDate, value);
        }


        /// <summary>
        /// Adds the specified number of milliseconds to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of whole and fractional milliseconds. The value parameter can be
        /// negative or positive. Note that this value is rounded to the nearest integer.
        /// </param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the number of milliseconds represented by value.
        /// </returns>
        public PersianDateTime AddMilliseconds(double value)
        {
            return Calendar.AddMilliseconds(CurrentDate, value);
        }


        /// <summary>
        /// Adds the specified number of minutes to the value of this instance.
        /// </summary>
        /// <param name="value">A number of whole and fractional minutes. The value parameter can be negative or positive.</param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the number of minutes represented by value.
        /// </returns>
        public PersianDateTime AddMinutes(int value)
        {
            return Calendar.AddMinutes(CurrentDate, value);
        }


        /// <summary>
        /// Adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="value">A number of months. The months parameter can be negative or positive.</param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and months.
        /// </returns>
        public PersianDateTime AddMonths(int value)
        {
            return Calendar.AddMonths(CurrentDate, value);
        }


        /// <summary>
        /// Adds the specified number of seconds to the value of this instance.
        /// </summary>
        /// <param name="value">A number of whole and fractional seconds. The value parameter can be negative or positive.</param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the number of seconds represented by value.
        /// </returns>
        public PersianDateTime AddSeconds(int value)
        {
            return Calendar.AddSeconds(CurrentDate, value);
        }


        /// <summary>
        /// Adds the specified number of ticks to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of 100-nanosecond ticks. The value parameter can be positive or negative.
        /// </param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the time represented by value.
        /// </returns>
        public PersianDateTime AddTicks(long value)
        {
            return CurrentDate.AddTicks(value);
        }


        /// <summary>
        /// Adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
        /// <returns>
        /// A PersianDateTime whose value is the sum of the date and time represented
        /// by this instance and the number of years represented by value.
        /// </returns>
        public PersianDateTime AddYears(int value)
        {
            return Calendar.AddYears(CurrentDate, value);
        }


        /// <summary>
        /// Compares the value of this instance to a specified PersianDateTime value
        /// and returns an integer that indicates whether this instance is earlier than,
        /// the same as, or later than the specified PersianDateTime value.
        /// </summary>
        /// <param name="value">A PersianDateTime object to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the value
        /// parameter.  Value Description Less than zero This instance is earlier than
        /// value. Zero This instance is the same as value. Greater than zero This instance
        /// is later than value.
        /// </returns>
        public int CompareTo(PersianDateTime value)
        {
            return CurrentDate.CompareTo(value.CurrentDate);
        }


        /// <summary>
        /// Compares the value of this instance to a specified object that contains a
        /// specified PersianDateTime value, and returns an integer that indicates whether
        /// this instance is earlier than, the same as, or later than the specified PersianDateTime
        /// value.
        /// </summary>
        /// <param name="value">A boxed PersianDateTime object to compare, or null.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description Less than zero This instance is earlier than value. Zero
        /// This instance is the same as value. Greater than zero This instance is later
        /// than value, or value is null.
        /// </returns>
        public int CompareTo(object value)
        {
            if (value is PersianDateTime)
                return CurrentDate.CompareTo(((PersianDateTime)value).CurrentDate);
            else
                throw new ArgumentException("value is not a PersianDateTime.");
        }


        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified
        /// PersianDateTime instance.
        /// </summary>
        /// <param name="value">A PersianDateTime instance to compare to this instance.</param>
        /// <returns>
        /// true if the value parameter equals the value of this instance; otherwise, false.
        /// </returns>
        public bool Equals(PersianDateTime value)
        {
            return CurrentDate.Equals(value.CurrentDate);
        }


        /// <summary>
        /// Converts the value of this instance to all the string representations supported
        /// by the standard PersianDateTime format specifiers.
        /// </summary>
        /// <returns>
        /// A string array where each element is the representation of the value of this
        /// instance formatted with one of the standard PersianDateTime formatting specifiers.
        /// </returns>
        public string[] GetDateTimeFormats()
        {
            return CurrentDate.GetDateTimeFormats();
        }


        /// <summary>
        /// Converts the value of this instance to all the string representations supported
        /// by the specified standard PersianDateTime format specifier.
        /// </summary>
        /// <param name="format">A DateTime format string.</param>
        /// <returns>
        /// A string array where each element is the representation of the value of this
        /// instance formatted with the format standard PersianDateTime formatting specifier.
        /// </returns>
        public string[] GetDateTimeFormats(char format)
        {
            return CurrentDate.GetDateTimeFormats(format);
        }


        /// <summary>
        /// Converts the value of this instance to all the string representations supported
        /// by the standard PersianDateTime format specifiers and the specified culture-specific
        /// formatting information.
        /// </summary>
        /// <param name="provider">
        /// An System.IFormatProvider that supplies culture-specific formatting information
        /// about this instance.
        /// </param>
        /// <returns>
        /// A string array where each element is the representation of the value of this
        /// instance formatted with one of the standard PersianDateTime formatting specifiers.
        /// </returns>
        public string[] GetDateTimeFormats(IFormatProvider provider)
        {
            return CurrentDate.GetDateTimeFormats(provider);
        }


        /// <summary>
        /// Returns the System.TypeCode for value type PersianDateTime.
        /// </summary>
        /// <returns>
        /// The enumerated constant, System.TypeCode.DateTime.
        /// </returns>
        public TypeCode GetTypeCode()
        {
            return CurrentDate.GetTypeCode();
        }


        /// <summary>
        /// Indicates whether this instance of PersianDateTime is within the Daylight
        /// Saving Time range for the current time zone.
        /// </summary>
        /// <returns>
        /// true if PersianDateTime.Kind is System.DateTimeKind.Local or System.DateTimeKind.Unspecified
        /// and the value of this instance of PersianDateTime is within the Daylight
        /// Saving Time range for the current time zone. false if PersianDateTime.Kind
        /// is System.DateTimeKind.Utc.
        /// </returns>
        public bool IsDaylightSavingTime()
        {
            if (Month <= 6)
                return true;

            return false;
        }


        /// <summary>
        /// Subtracts the specified date and time from this instance.
        /// </summary>
        /// <param name="value">An instance of PersianDateTime.</param>
        /// <returns>
        /// A System.TimeSpan interval equal to the date and time represented by this
        /// instance minus the date and time represented by value.
        /// </returns>
        public TimeSpan Subtract(PersianDateTime value)
        {
            return CurrentDate.Subtract(value.CurrentDate);
        }


        /// <summary>
        /// Subtracts the specified duration from this instance.
        /// </summary>
        /// <param name="value">An instance of System.TimeSpan.</param>
        /// <returns>
        /// A PersianDateTime equal to the date and time represented by this instance
        /// minus the time interval represented by value.
        /// </returns>
        public PersianDateTime Subtract(TimeSpan value)
        {
            return CurrentDate.Subtract(value);
        }


        /// <summary>
        /// Serializes the current PersianDateTime object to a 64-bit binary value that
        /// subsequently can be used to recreate the PersianDateTime object.
        /// </summary>
        /// <returns>
        /// A 64-bit signed integer that encodes the System.DateTime.Kind and System.DateTime.Ticks properties.
        /// </returns>
        public long ToBinary()
        {
            return CurrentDate.ToBinary();
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to a Windows file time.
        /// </summary>
        /// <returns>
        /// The value of the current PersianDateTime object expressed as a Windows file time.
        /// </returns>
        public long ToFileTime()
        {
            return CurrentDate.ToFileTime();
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to a Windows file
        /// time.
        /// </summary>
        /// <returns>
        /// The value of the current PersianDateTime object expressed as a Windows file time.
        /// </returns>
        public long ToFileTimeUtc()
        {
            return CurrentDate.ToFileTimeUtc();
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to local time.
        /// </summary>
        /// <returns>
        ///    A PersianDateTime object whose PersianDateTime.Kind property is System.DateTimeKind.Local,
        ///    and whose value is the local time equivalent to the value of the current
        ///    PersianDateTime object, or PersianDateTime.MaxValue if the converted value
        ///    is too large to be represented by a PersianDateTime object, or PersianDateTime.MinValue
        ///    if the converted value is too small to be represented as a PersianDateTime
        ///    object.
        /// </returns>
        public PersianDateTime ToLocalTime()
        {
            return CurrentDate.ToLocalTime();
        }


        /// <summary>
        /// Converts the value of this instance to the equivalent OLE Automation date.
        /// </summary>
        /// <returns>
        /// A double-precision floating-point number that contains an OLE Automation
        /// date equivalent to the value of this instance.
        /// </returns>
        public double ToOADate()
        {
            return CurrentDate.ToOADate();
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to Coordinated Universal
        /// Time (UTC).
        /// </summary>
        /// <returns>
        /// A PersianDateTime object whose PersianDateTime.Kind property is System.DateTimeKind.Utc,
        /// and whose value is the UTC equivalent to the value of the current PersianDateTime
        /// object, or PersianDateTime.MaxValue if the converted value is too large to
        /// be represented by a PersianDateTime object, or PersianDateTime.MinValue if
        /// the converted value is too small to be represented by a PersianDateTime object.
        /// </returns>
        public PersianDateTime ToUniversalTime()
        {
            return CurrentDate.ToUniversalTime();
        }


        /// <summary>
        /// Converts current instance to System.DateTime
        /// </summary>
        /// <returns>The System.DateTime equivalent of current instance</returns>
        public DateTime ToDateTime()
        {
            return this.CurrentDate;
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to its equivalent
        /// string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the value of the current PersianDateTime object.
        /// </returns>
        public override string ToString()
        {
            return ToString("F");
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to its equivalent
        /// string representation using the specified format.
        /// </summary>
        /// <param name="Format">A DateTime format string.</param>
        /// <returns>
        /// A string representation of value of the current PersianDateTime object as
        /// specified by format.
        /// </returns>
        public string ToString(string Format)
        {
            switch (Format)
            {
                case "d":
                    return this.Year + "/" + this.Month.ToString("00") + "/" + this.Day.ToString("00");

                case "D":
                    return this.PersianDayOfWeek + "، " + this.Day + " " + this.MonthOfYear + "، " + this.Year;

                case "t":
                    return CurrentDate.ToString("HH:mm");

                case "T":
                    return CurrentDate.ToString("HH:mm:ss");

                case "dt":
                case "td":
                    return ToString("d") + " - " + ToString("t");

                case "DT":
                case "TD":
                    return ToString("D") + " - " + ToString("T");

                case "dT":
                case "Td":
                    return ToString("d") + " - " + ToString("T");

                case "Dt":
                case "tD":
                    return ToString("D") + " - " + ToString("t");

                case "f":
                    return ToString("Dt");

                case "F":
                    return ToString("DT");

                case "y":
                case "Y":
                    return this.MonthOfYear + "، " + this.Year;

                case "N":
                    return this.Year + this.Month.ToString("00") + this.Day.ToString("00");
            }

            return CurrentDate.ToString(Format, Culture);
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to its equivalent string
        /// representation using the specified culture-specific format information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of value of the current PersianDateTime object as specified by provider.</returns>
        public string ToString(IFormatProvider provider)
        {
            return CurrentDate.ToString(provider);
        }


        /// <summary>
        /// Converts the value of the current PersianDateTime object to its equivalent string
        /// representation using the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A standard or custom date and time format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of value of the current System.DateTime object as specified by format and provider.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return CurrentDate.ToString(format, provider);
        }

        #endregion

        #region Overridden Methods

        public override int GetHashCode()
        {
            return CurrentDate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return CurrentDate.Equals(obj);
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToBoolean(provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToChar(provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToSByte(provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToByte(provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToInt16(provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToUInt16(provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToInt32(provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToUInt32(provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToInt64(provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToUInt64(provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToSingle(provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToDouble(provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToDecimal(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return CurrentDate;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)CurrentDate).ToType(conversionType, provider);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)CurrentDate).GetObjectData(info, context);
        }

        #endregion
    }


    public class PersianCultureInfo : CultureInfo
    {
        #region Constructor

        public PersianCultureInfo()
            : base("fa-IR", false)
        {
            SetCalendars();
        }

        #endregion

        #region Class Members

        DateTimeFormatInfo _dtfi;
        readonly PersianCalendar _calendar = new PersianCalendar();

        public override Calendar Calendar
        {
            get { return _calendar; }
        }

        public override DateTimeFormatInfo DateTimeFormat
        {
            get
            {
                if (_dtfi == null)
                {
                    _dtfi = base.DateTimeFormat;

                    _dtfi.DayNames = new string[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه", "شنبه" };
                    _dtfi.ShortestDayNames = new string[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                    _dtfi.AbbreviatedDayNames = _dtfi.DayNames;

                    _dtfi.MonthNames = new string[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
                    _dtfi.MonthGenitiveNames = _dtfi.MonthNames;
                    _dtfi.AbbreviatedMonthNames = _dtfi.MonthNames;
                    _dtfi.AbbreviatedMonthGenitiveNames = _dtfi.MonthNames;

                    _dtfi.FirstDayOfWeek = DayOfWeek.Saturday;
                }

                return _dtfi;
            }

            set
            {
            }
        }

        /// <summary>
        /// Updates CultureInfo calendars
        /// Windows does not support PersianCalendar with 'fa-IR' culture
        /// using reflection we change array of calendars the culture supports with the first index being default Persian calendar
        /// </summary>
        void SetCalendars()
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var cultureDataField = typeof(CultureInfo).GetField("m_cultureData", flags);
            var cultureData = cultureDataField.GetValue(this);
            var waCalendarsField = cultureData.GetType().GetField("waCalendars", flags);

            /// CalendarId.PERSIAN = 22, CalendarId.GREGORIAN = 1, CalendarId.GREGORIAN_ARABIC = 10, CalendarId.HIJRI = 6
            waCalendarsField.SetValue(cultureData, new int[] { 22, 1, 10, 6 });
        }

        #endregion
    }
}
