using System;
using System.Windows.Forms;

public class Program
{
    [STAThread] // Ensures the application runs in a single-threaded apartment state (necessary for Windows Forms)
    static void Main(string[] args)
    {
        // Create an instance of the Client form
        Client form = new Client();

        // Optionally set up the form's layout or any other custom configurations
        form.FormLayout(); // Make sure FormLayout() does not conflict with the layout or controls

        // Start the application and show the form
        Application.Run(form);
    }
}