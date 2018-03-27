﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using LibUISharp.Drawing;
using LibUISharp.Internal;

using static LibUISharp.Internal.LibUI;

namespace LibUISharp
{
    public class Window : Control
    {
        private Control child;
        private Size size;
        private string title;
        private static readonly Dictionary<ControlSafeHandle, Window> WindowCache = new Dictionary<ControlSafeHandle, Window>();

        public Window(int width = 500, int height = 300, string title = null, bool hasMenuBar = false)
        { 
            if (string.IsNullOrEmpty(title))
                title = "LibUI";
            Handle = uiNewWindow(title, width, height, hasMenuBar);
            WindowCache.Add(Handle, this);
            this.title = title;
            InitializeEvents();
            InitializeComponent();
        }

        public Window(Size size, string title = null, bool hasMenuBar = false) : this(size.Width, size.Height, title, hasMenuBar) { }

        public EventHandler<CancelEventArgs> Closing;
        public EventHandler SizeChanged;

        public string Title
        {
            get
            {
                title = uiWindowTitle(Handle);
                return title;
            }
            set
            {
                if (title != value)
                {
                    if (string.IsNullOrEmpty(value))
                        title = "LibUI";
                    else
                        title = value;
                    uiWindowSetTitle(Handle, title);
                }
            }
        }

        public Size Size
        {
            get => uiWindowContentSize(Handle);
            set => uiWindowSetContentSize(Handle, value);
        }

        public int Width => Size.Width;
        public int Height => Size.Height;

        public bool Margins
        {
            get => uiWindowMargined(Handle);
            set => uiWindowSetMargined(Handle, value);
        }

        public bool Fullscreen
        {
            get => uiWindowFullscreen(Handle);
            set => uiWindowSetFullscreen(Handle, value);
        }

        public bool Borderless
        {
            get => uiWindowBorderless(Handle);
            set => uiWindowSetBorderless(Handle, value);
        }
  
        public Control Child
        {
            get => child;
            set
            {
                if (!Handle.IsInvalid)
                {
                    if (value == null)
                        uiWindowSetChild(Handle, new ControlSafeHandle());
                    uiWindowSetChild(Handle, value.Handle);
                }
                child = value;
            }
        }

        protected sealed override void InitializeEvents()
        {
            if (Handle.IsInvalid)
                throw new TypeInitializationException(nameof(Window), new InvalidComObjectException());

            uiWindowOnClosing(Handle, (window, data) =>
            {
                CancelEventArgs args = new CancelEventArgs();
                OnClosing(args);
                bool cancel = args.Cancel;
                if (!cancel)
                {
                    if (WindowCache.Count > 1 && this != Application.MainWindow)
                        Close();
                    else
                        Application.Current.Exit();
                }
                return !cancel;
            });

            uiWindowOnContentSizeChanged(Handle, (window, data) => { OnResize(EventArgs.Empty); });
        }

        protected sealed override void InitializeComponent() { }

        protected virtual void OnClosing(CancelEventArgs e) => Closing?.Invoke(this, e);
        protected virtual void OnSizeChanged(EventArgs e) => SizeChanged?.Invoke(this, e);

        public void Close()
        {
            Hide();
            if (Child != null)
            {
                Child.Dispose();
                Child = null;
            }
            WindowCache.Remove(Handle);
            Dispose();
        }
    }
}