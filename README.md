# HotKeys
Tool for registering hot keys
## Installation
NuGet Package: [Resolve.HotKeys](https://preview.nuget.org/packages/Resolve.HotKeys)
## Usage
1. Import the namespace

       using Resolve.HotKeys;
2. Declare the variable

       HotKey xHotKey;
3. Create instance, add event handler and register
       
       try
       {
          xHotKey = new HotKey(Keys.X);
          xHotKey.Pressed += XHotKey_Pressed;
          xHotKey.Register();
       }
       catch (Exception ex)
       {
          Console.WriteLine(ex);
       }
4. Handle the event

       private void XHotKey_Pressed(object sender, EventArgs e)
       {
            Console.WriteLine("X pressed.");
       }
5. Dispose

       xHotKey.Pressed -= XHotKey_Pressed;
       xHotKey.Dispose();
