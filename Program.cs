using System;
using System.Windows.Forms;

namespace Kursach
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create an instance of Form1 *before* running the application
            Form1 mainForm = new Form1(); // This initializes Form1.Instance
            Application.Run(mainForm); // Run Form1
        }
    }
}
