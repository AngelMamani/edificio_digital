namespace edificio_digital.Models.Common;

public static class AppConstants
{
    public static class ApiRoutes
    {
        public const string ApiBase = "/api";

        public static class Auth
        {
            public const string Group = ApiBase + "/auth";
            public const string Login = Group + "/login";
            public const string Logout = Group + "/logout";
        }
    }

    public static class Pages
    {
        public const string Home = "/Index";
        public const string Login = "/Login";
        public const string AccessDenied = "/AccessDenied";
        public const string AdminHome = "/Admin/Index";
        public const string SolicitanteHome = "/Solicitante/Index";
    }

    public static class Layouts
    {
        public const string Public = "_LayoutPublic";
        public const string Admin = "_LayoutAdmin";
        public const string Solicitante = "_LayoutSolicitante";
        public const string Login = "_LayoutLogin";
    }

    public static class Auth
    {
        public const string CookieScheme = "EdificioDigitalAuth";
        public const string CookieName = "edificio_digital.auth";
    }

    public static class Roles
    {
        public const string Admin = "admin";
        public const string Solicitante = "solicitante";

        public static class Display
        {
            public const string Admin = "Administrador";
            public const string Solicitante = "Solicitante";
        }
    }

    public static class Policies
    {
        public const string AdminOnly = "AdminOnly";
        public const string SolicitanteOnly = "SolicitanteOnly";
    }

    public static class Claims
    {
        public const string UserId = "uid";
        public const string Email = "email";
        public const string NombreCompleto = "nombre_completo";
    }
}
