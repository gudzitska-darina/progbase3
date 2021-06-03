using System;
using Terminal.Gui;

namespace ConsoleApp
{
    public class ErrorDBWindow : Window
    {
        public ErrorDBWindow()
        {
            MessageBox.ErrorQuery("Error DB","Database not found", "OK");
            Environment.Exit(0);
        }
        
    }
}