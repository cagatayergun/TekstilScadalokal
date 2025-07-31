namespace TekstilScada.Services
{
    /// <summary>
    /// Uygulama genelindeki tüm yetki kurallarını merkezi olarak yönetir.
    /// </summary>
    public static class PermissionService
    {
        // AYARLAR EKRANI
        public static bool CanViewSettings => CurrentUser.HasRole("Admin");

        // PROSES KONTROL (REÇETE) EKRANI
        public static bool CanEditRecipes => CurrentUser.HasRole("Admin") || CurrentUser.HasRole("Mühendis");
        public static bool CanDeleteRecipes => CurrentUser.HasRole("Admin");
        public static bool CanTransferToPlc => CurrentUser.HasRole("Admin") || CurrentUser.HasRole("Mühendis");

        // RAPORLAR EKRANI
        public static bool CanViewReports => CurrentUser.HasRole("Admin") || CurrentUser.HasRole("Mühendis");

        // Diğer tüm yetki kuralları buraya eklenebilir.
        // Örneğin:
        // public static bool CanAcknowledgeAlarms => CurrentUser.HasRole("Admin") || CurrentUser.HasRole("Mühendis") || CurrentUser.HasRole("Operatör");
    }
}