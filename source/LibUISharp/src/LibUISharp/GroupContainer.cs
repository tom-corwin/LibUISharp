﻿using LibUISharp.Internal;
using static LibUISharp.Internal.Libraries;

namespace LibUISharp
{
    /// <summary>
    /// Represents a control that creates a container that has a border and a title for user-interface (UI) content.
    /// </summary>
    [NativeType("uiGroup")]
    public class GroupContainer : SingleContainer<GroupContainer, Control>
    {
        private Control child;
        private string title;
        private bool isMargined;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupContainer"/> class with the specified title.
        /// </summary>
        /// <param name="title">The title of this <see cref="GroupContainer"/>.</param>
        public GroupContainer(string title) => Handle = Libui.Call<Libui.uiNewGroup>()(title);

        /// <summary>
        /// Gets or sets the title for this <see cref="GroupContainer"/> control.
        /// </summary>
        public string Title
        {
            get
            {
                title = Libui.Call<Libui.uiGroupTitle>()(this);
                return title;
            }
            set
            {
                if (title != value)
                {
                    Libui.Call<Libui.uiGroupSetTitle>()(this, value);
                    title = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this <see cref="TabPage"/> has outer margins.
        /// </summary>
        public bool IsMargined
        {
            get
            {
                isMargined = Libui.Call<Libui.uiGroupMargined>()(this);
                return isMargined;
            }
            set
            {
                if (isMargined != value)
                {
                    Libui.Call<Libui.uiGroupSetMargined>()(this, value);
                    isMargined = value;
                }
            }
        }

        /// <summary>
        /// Sets this <see cref="GroupContainer"/> object's child <see cref="Control"/>.
        /// </summary>
        public override Control Child
        {
            set
            {
                if (child != value)
                {
                    Libui.Call<Libui.uiGroupSetChild>()(this, value);
                    child = value;
                }
            }
        }
    }
}