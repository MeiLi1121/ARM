using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelIllustrations;

namespace ModelIllustrations
{
    public class ModelIllustrator
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Illustrator());
        }

        //public static void InitializeAppWindow()
        //{
        //    //set default input level
        //    var optionToValue = BSBasic.InitializeOption(BSBasic.OptionType.EuroPut, DEFAULT_SPOT, DEFAULT_STRIKE, DEFAULT_VOL, DEFAULT_RATE, DEFAULT_DIVIDEND, DEFAULT_T);
        //}
    }
}
