namespace WSW
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wswProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.wswInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // wswProcessInstaller
            // 
            this.wswProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.wswProcessInstaller.Password = null;
            this.wswProcessInstaller.Username = null;
            this.wswProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.wswProcessInstaller_AfterInstall);
            // 
            // wswInstaller
            // 
            this.wswInstaller.ServiceName = "WSWService";
            this.wswInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.wswInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.wswInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.wswProcessInstaller,
            this.wswInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller wswProcessInstaller;
        private System.ServiceProcess.ServiceInstaller wswInstaller;
    }
}