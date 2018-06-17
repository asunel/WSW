namespace WSW
{
    using System.ComponentModel;
    using System.Configuration.Install;

    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void wswProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
        }

        private void wswInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
        }
    }
}
