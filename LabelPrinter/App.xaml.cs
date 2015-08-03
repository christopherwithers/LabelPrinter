using System.Windows;
using LabelGenerator;
using LabelGenerator.Interfaces;

namespace FirstFloor.ModernUI.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LightInject.ServiceContainer Container = new LightInject.ServiceContainer();
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Container.Register<IBitmapGenerator, BitmapGenerator>();
            Container.Register<ILabelTemplateManager, LabelTemplateManager>();
            Container.Register<ILabelManager, LabelManager>();

            base.OnStartup(e);
        }
    }
}
