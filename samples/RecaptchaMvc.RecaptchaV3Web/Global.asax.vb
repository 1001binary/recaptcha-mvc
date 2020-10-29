Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        Register()
    End Sub

    Protected Sub Register()
        GlobalFilters.Filters.Add(New Recaptcha3ActionFilterAttribute("[YOUR_SECRET_KEY]",
            New List(Of RecaptchaFilterItem) From {
                New RecaptchaFilterItem("\/auth\/sign-in", "POST")
            }))
    End Sub
End Class
