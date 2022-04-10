using System;
using EasyWay.Core;

namespace EasyWay.UI;

// start elmish loop
public partial class App
{
    public App() => Activated += StartElmish;

    private void StartElmish(object? sender, EventArgs e)
    {
        Activated -= StartElmish;
        Program.Run(MainWindow);
    }
}