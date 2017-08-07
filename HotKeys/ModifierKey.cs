using System;
using System.Collections.Generic;
using System.Text;

namespace Resolve.HotKeys
{
    [Flags]
    public enum ModifierKey
    {
        None=0,
        /// <summary>
        /// Either ALT key must be held down.
        /// </summary>
        Alt = 0x0001,
        /// <summary>
        /// Either CTRL key must be held down.
        /// </summary>
        Control = 0x0002,
        /// <summary>
        /// Changes the hotkey behavior so that the keyboard auto-repeat does not yield multiple hotkey notifications. 
        /// WindowsVista:  This flag is not supported.
        /// </summary>
        NoRepeat = 0x4000,
        /// <summary>
        /// Either SHIFT key must be held down.
        /// </summary>
        Shift =  0x0004,
        /// <summary>
        /// Either WINDOWS key was held down. These keys are labeled with the Windows logo. Keyboard shortcuts that involve the WINDOWS key are reserved for use by the operating system.
        /// </summary>
        Win = 0x0008
    }
}
