using System;
using System.Windows.Forms;

public class Program
{
    public static Client form = new Client();
    [STAThread]
    static void Main(string[] args)
    {
        form.FormLayout(); ;
        Application.Run(form);
    }
}