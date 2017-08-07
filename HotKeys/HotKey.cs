using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Resolve.HotKeys
{
    public class HotKey : IMessageFilter, IDisposable
    {
                private Keys _key;
        private ModifierKey _modifiers;
        private short? _id;
        private IntPtr _handle;
        private bool _disposed;

        public event EventHandler Pressed;

        private Keys Key
        {
            get
            {
                return _key;
            }
        }

        public ModifierKey Modifiers
        {
            get
            {
                return _modifiers;
            }
        }

        public short? Id
        {
            get
            {
                return _id;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public HotKey(Keys key):this(key, ModifierKey.None, IntPtr.Zero) { }

        public HotKey(Keys key, ModifierKey modifiers) : this(key, modifiers, IntPtr.Zero)
        {
        }

        public HotKey(Keys key, ModifierKey modifiers, IntPtr handle) : base()
        {
            _key = key;
            _modifiers = modifiers;
            _handle = handle;
            _disposed = true;
        }

        public void Register()
        {
            if (_id.HasValue)
            {
                return;
            }
            NativeMethods.SetLastError(NativeMethods.ERROR_SUCCESS);
            _id = NativeMethods.GlobalAddAtom(GetHashCode().ToString());

            var error = Marshal.GetLastWin32Error();


            if (error != NativeMethods.ERROR_SUCCESS)
            {
                _id = null;
                throw new Win32Exception(error);
            }
            var vk = unchecked((uint)(Key & ~Keys.Modifiers));
            NativeMethods.SetLastError(NativeMethods.ERROR_SUCCESS);
            var result = NativeMethods.RegisterHotKey(_handle, _id.Value, (uint)Modifiers, vk);

            error = Marshal.GetLastWin32Error();

            if (error != 0)
            {
                _id = null;
                throw new Win32Exception(error);
            }
            if (result)
            {
                Application.AddMessageFilter(this);
            }
            else
            {
                _id = null;
            }
        }

        public void Unregister()
        {
            if (_id == null)
            {
                return;
            }
            NativeMethods.SetLastError(NativeMethods.ERROR_SUCCESS);
            var result = NativeMethods.UnregisterHotKey(_handle, Id.Value);
            var error = Marshal.GetLastWin32Error();
            if (error != NativeMethods.ERROR_SUCCESS)
            {
                throw new Win32Exception(error);
            }
            NativeMethods.SetLastError(NativeMethods.ERROR_SUCCESS);
            NativeMethods.GlobalDeleteAtom(_id.Value);
            error = Marshal.GetLastWin32Error();
            if (error != NativeMethods.ERROR_SUCCESS)
            {
                throw new Win32Exception(error);
            }
            _id = null;
            Application.RemoveMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_HOTKEY:
                    if (m.HWnd == Handle && m.WParam == (IntPtr)Id && Pressed != null)
                    {
                        Pressed(this, EventArgs.Empty);
                        return true;
                    }
                    break;
            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary> 
                /// Unregister the hotkey. 
                /// </summary> 
        protected virtual void Dispose(bool disposing)
        {
            // Protect from being called multiple times. 
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {

                // Removes a message filter from the message pump of the application. 


                Unregister();
            }

            _disposed = true;
        }
    }
}
