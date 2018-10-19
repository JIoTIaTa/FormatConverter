using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject;
using ObserverReaderWriter.IoC;

namespace ObserverReaderWriter
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IKernel iocKernel = args.Length == 0 ? new StandardKernel(new NinjectLoadConfig()) : new StandardKernel(new NinjectLoadConfig(args[0]));
            Application.Run(iocKernel.Get<Form1>());
        }
    }
}
