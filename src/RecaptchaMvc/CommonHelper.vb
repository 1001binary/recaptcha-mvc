Imports System.Net.Http
Imports System.Text
Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Public Class CommonHelper
    ''' <summary>
    ''' Validates the recaptcha v3 from Google service based on the provided <paramref name="SecretKey"/> and <paramref name="Token"/>.
    ''' </summary>
    ''' <param name="SecretKey"></param>
    ''' <param name="Token"></param>
    ''' <returns></returns>
    Public Shared Function ValidateRecaptchaV3(SecretKey As String, Token As String) As Boolean
        Using Client As New HttpClient
            Dim Secret As String = SecretKey
            Dim JsSerializer As New JavaScriptSerializer

            Dim HttpContent = New StringContent(JsSerializer.Serialize(New With {
                .secret = Secret,
                .response = Token
            }), Encoding.UTF8, "application/json")
            Dim Result = Client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={Secret}&response={Token}", Nothing).GetAwaiter.GetResult

            Dim JsonResult As String = Result.Content.ReadAsStringAsync().GetAwaiter.GetResult
            Dim CaptchaResult = JsSerializer.Deserialize(Of RecaptchaResult)(JsonResult)

            Return If(CaptchaResult?.Success, False)
        End Using
    End Function

    ''' <summary>
    ''' Adds error to TempData and redirects to action.
    ''' </summary>
    ''' <param name="filterContext"></param>
    ''' <param name="message"></param>
    Public Shared Sub AddErrorAndRedirectToGetAction(ByVal filterContext As ActionExecutingContext, ByVal Optional message As String = Nothing)
        filterContext.Controller.TempData("InvalidCaptcha") = If(message, "The captcha is invalid.")
        filterContext.Result = New RedirectToRouteResult(filterContext.RouteData.Values)
    End Sub

    ''' <summary>
    ''' Redirects to a specific URL.
    ''' </summary>
    Public Shared Sub RedirectToUrl(ByVal filterContext As ActionExecutingContext, ByVal RedirectUrl As String)
        Dim CtrlBase As ControllerBase = filterContext.Controller
        filterContext.Result = New RedirectResult(RedirectUrl)
        filterContext.Result.ExecuteResult(CtrlBase.ControllerContext)
    End Sub

    ''' <summary>
    ''' Adds an error message to ModelState.
    ''' </summary>
    Public Shared Sub AddErrorToModelState(ByVal filterContext As ActionExecutingContext, ByVal Key As String, ByVal ErrorMsg As String)
        Dim CtrlBase As ControllerBase = filterContext.Controller
        filterContext.Controller.ViewData.ModelState.AddModelError(Key, ErrorMsg)
    End Sub
End Class
