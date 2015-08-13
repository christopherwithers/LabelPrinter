using System;
using System.IO;
using System.Windows;
using LabelGenerator;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects;
using LightInject;

namespace FirstFloor.ModernUI.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        public static readonly ServiceContainer Container = new ServiceContainer();
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Container.Register<IFileManager, FileManager>();
            Container.Register<ILabelTemplateManager, LabelTemplateManager>();
            Container.Register<IBitmapGenerator, BitmapGenerator>();
            Container.Register<ILabelManager, LabelManager>();
            

            base.OnStartup(e);
        }




    }
}
