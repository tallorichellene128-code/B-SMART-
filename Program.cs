namespace BSMART
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            BsmartDatabaseInitializer.EnsureSupportTables();
            Application.Idle += (s, e) => BsmartNotificationService.AttachToOpenForms();
            Application.Idle += (s, e) => SettingsNavigationService.AttachToOpenForms();
            Application.Idle += (s, e) => SearchSuggestionService.AttachToOpenForms();
            Application.Idle += (s, e) => BsmartUiService.AttachToOpenForms();
            Application.Run(new loginAS());
        }
    }
}
