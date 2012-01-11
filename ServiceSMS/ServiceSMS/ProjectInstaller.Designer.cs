namespace ServiceSMS
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceSMSProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceSMSInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceSMSProcessInstaller
            // 
            this.serviceSMSProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.serviceSMSProcessInstaller.Password = null;
            this.serviceSMSProcessInstaller.Username = null;
            // 
            // serviceSMSInstaller
            // 
            this.serviceSMSInstaller.Description = "Le service de gestion SMS";
            this.serviceSMSInstaller.DisplayName = "SMS Service";
            this.serviceSMSInstaller.ServiceName = "ServiceSMS";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceSMSProcessInstaller,
            this.serviceSMSInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceSMSProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceSMSInstaller;
    }
}