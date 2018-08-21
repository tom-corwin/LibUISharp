﻿using System;
using System.Runtime.InteropServices;
using LibUISharp.Internal;
using LibUISharp.SafeHandles;

namespace LibUISharp
{
    /// <summary>
    /// Implements the basic functonality required by a date-time picker.
    /// </summary>
    [NativeType("uiDateTimePicker")]
    public abstract class DateTimePickerBase : Control
    {
        private DateTime? dateTime = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePickerBase"/> class.
        /// </summary>
        protected DateTimePickerBase() { }

        /// <summary>
        /// Occurs when the <see cref="DateTime"/> property is changed.
        /// </summary>
        public event Action DateTimeChanged;

        /// <summary>
        /// Gets or sets the selected date and time.
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                if (IsInvalid) throw new UIComponentInvalidHandleException<SafeControlHandle>(this);
                NativeCalls.DateTimePickerTime(Handle, out UIDateTime time);
                dateTime = UIDateTime.ToDateTime(time);
                return (DateTime)dateTime;
            }
            set
            {
                if (dateTime == value) return;
                if (IsInvalid) throw new UIComponentInvalidHandleException<SafeControlHandle>(this);
                NativeCalls.DateTimePickerSetTime(Handle, UIDateTime.FromDateTime(value));
                dateTime = value;
            }
        }

        /// <summary>
        /// Initializes this UI component's events.
        /// </summary>
        protected sealed override void InitializeEvents()
        {
            if (IsInvalid) throw new UIComponentInvalidHandleException<SafeControlHandle>(this);
            NativeCalls.DateTimePickerOnChanged(Handle, (d, data) => { OnDateTimeChanged(); }, IntPtr.Zero);
        }

        /// <summary>
        /// Called when the <see cref="DateTimeChanged"/> event is raised.
        /// </summary>
        protected virtual void OnDateTimeChanged() => DateTimeChanged?.Invoke();
    }

    /// <summary>
    /// Represents a control that allows the user to select and display a date.
    /// </summary>
    public class DatePicker : DateTimePickerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        public DatePicker(int? year = null, int? month = null, int? day = null) : base()
        {
            Handle = NativeCalls.NewDatePicker();
            DateTime dt = DateTime.Now;
            if (year != null)
                dt = new DateTime((int)year, dt.Month, dt.Day);
            if (month != null)
                dt = new DateTime(dt.Year, (int)month, dt.Day);
            if (day != null)
                dt = new DateTime(dt.Year, dt.Month, (int)day);
            DateTime = dt;
            InitializeEvents();
        }

        /// <summary>
        /// Gets the year component from <see cref="DateTime"/>.
        /// </summary>
        public int Year => DateTime.Year;

        /// <summary>
        /// Gets the month component from <see cref="DateTime"/>.
        /// </summary>
        public int Month => DateTime.Month;

        /// <summary>
        /// Gets the day component from <see cref="DateTime"/>.
        /// </summary>
        public int Day => DateTime.Day;
    }

    /// <summary>
    /// Represents a control that allows the user to select and display a time.
    /// </summary>
    public class TimePicker : DateTimePickerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class.
        /// </summary>
        public TimePicker(int? hour = null, int? minute = null, int? second = null) : base()
        {
            Handle = NativeCalls.NewTimePicker();
            DateTime dt = DateTime.Now;
            if (hour != null)
                dt = new DateTime(dt.Year, dt.Month, dt.Day, (int)hour, dt.Minute, dt.Second);
            if (minute != null)
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, (int)minute, dt.Second);
            if (second != null)
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, (int)second);
            DateTime = dt;
            InitializeEvents();
        }

        /// <summary>
        /// Gets the hour component from <see cref="DateTime"/>.
        /// </summary>
        public int Hour => DateTime.Hour;

        /// <summary>
        /// Gets the minute component from <see cref="DateTime"/>.
        /// </summary>
        public int Minute => DateTime.Minute;

        /// <summary>
        /// Gets the second component from <see cref="DateTime"/>.
        /// </summary>
        public int Second => DateTime.Second;
    }

    /// <summary>
    /// Represents a control that allows the user to select and display a date and time.
    /// </summary>
    public class DateTimePicker : DateTimePickerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        public DateTimePicker(int? year = null, int? month = null, int? day = null, int? hour = null, int? minute = null, int? second = null) : base()
        {
            Handle = NativeCalls.NewDateTimePicker();
            DateTime dt = DateTime.Now;
            if (year != null)
                dt = new DateTime((int)year, dt.Month, dt.Day);
            if (month != null)
                dt = new DateTime(dt.Year, (int)month, dt.Day);
            if (day != null)
                dt = new DateTime(dt.Year, dt.Month, (int)day);
            if (hour != null)
                dt = new DateTime(dt.Year, dt.Month, dt.Day, (int)hour, dt.Minute, dt.Second);
            if (minute != null)
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, (int)minute, dt.Second);
            if (second != null)
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, (int)second);
            DateTime = dt;
            InitializeEvents();
        }

        /// <summary>
        /// Gets the year component from <see cref="DateTime"/>.
        /// </summary>
        public int Year => DateTime.Year;

        /// <summary>
        /// Gets the month component from <see cref="DateTime"/>.
        /// </summary>
        public int Month => DateTime.Month;

        /// <summary>
        /// Gets the day component from <see cref="DateTime"/>.
        /// </summary>
        public int Day => DateTime.Day;

        /// <summary>
        /// Gets the hour component from <see cref="DateTime"/>.
        /// </summary>
        public int Hour => DateTime.Hour;

        /// <summary>
        /// Gets the minute component from <see cref="DateTime"/>.
        /// </summary>
        public int Minute => DateTime.Minute;

        /// <summary>
        /// Gets the second component from <see cref="DateTime"/>.
        /// </summary>
        public int Second => DateTime.Second;
    }

    [NativeType("tm")]
    [StructLayout(LayoutKind.Sequential)]
    internal class UIDateTime
    {
#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0044 // Add readonly modifier
        private int sec, min, hour, day, mon, year;
        private readonly int wday, yday; // Must be uninitialized.
        private readonly int isdst = -1; //Must be -1.
#pragma warning restore IDE0032 // Use auto property
#pragma warning restore IDE0044 // Add readonly modifier

        public UIDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            sec = second;
            min = minute;
            this.hour = hour;
            this.day = day;
            mon = month;
            this.year = year;
        }

        public int Second
        {
            get => sec;
            set => sec = value;
        }

        public int Minute
        {
            get => min;
            set => min = value;
        }

        public int Hour
        {
            get => hour;
            set => hour = value;
        }

        public int Day
        {
            get => day;
            set => day = value;
        }

        public int Month
        {
            get => mon;
            set => mon = value;
        }

        public int Year
        {
            get => year;
            set => year = value;
        }

        public static DateTime ToDateTime(UIDateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        public static UIDateTime FromDateTime(DateTime dt) => new UIDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
    }
}