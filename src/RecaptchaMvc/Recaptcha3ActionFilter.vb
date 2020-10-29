Imports System.Text.RegularExpressions
Imports System.Web.Mvc

''' <summary>
''' Represents the condition to apply the recaptcha check.
''' </summary>
Public Class RecaptchaFilterItem

    ''' <summary>
    ''' Initializes a new instance of <seealso cref="RecaptchaFilterItem"/> class with two provided parameters <paramref name="UrlPattern"/> and <paramref name="HttpMethod"/>.
    ''' </summary>
    ''' <param name="UrlPattern"></param>
    ''' <param name="HttpMethod"></param>
    Public Sub New(UrlPattern As String, HttpMethod As String)
        Me.UrlPattern = UrlPattern
        Me.HttpMethod = HttpMethod
    End Sub

    ''' <summary>
    ''' Gets the pattern to check whether URL is valid for checking recaptcha.
    ''' </summary>
    ''' <example>/account/sign-in|/user/sign-up</example>
    ''' <returns></returns>
    Public Property UrlPattern As String

    ''' <summary>
    ''' Gets the HttpMethod to check whether URL is valid for checking recaptcha.
    ''' </summary>
    ''' <example>POST</example>
    ''' <returns></returns>
    Public Property HttpMethod As String
End Class

''' <summary>
''' Represents the recaptcha result from Google service.
''' </summary>
Friend Class RecaptchaResult
    ''' <summary>
    ''' Returns the recaptcha result whether it's successful.
    ''' </summary>
    ''' <returns></returns>
    Property Success As Boolean
End Class

''' <summary>
''' Represents the recaptcha v3 action filter.
''' </summary>
Public Class Recaptcha3ActionFilterAttribute
    Inherits ActionFilterAttribute

    ''' <summary>
    ''' Gets the Recaptcha field name of HTML.
    ''' </summary>
    Private ReadOnly RecaptchaFieldName As String

    ''' <summary>
    ''' Gets the secret key of Google Recaptcha v3.
    ''' </summary>
    Private ReadOnly SecretKey As String

    ''' <summary>
    ''' Gets a number of filter items so that the <seealso cref="Recaptcha3ActionFilterAttribute"/> filter only verifies Recaptcha in a specific scope.
    ''' </summary>
    Private ReadOnly FilterItems As IEnumerable(Of RecaptchaFilterItem)

    ''' <summary>
    ''' Gets the failure action after the Recaptcha verification.
    ''' </summary>
    Private ReadOnly OnFailure As Action(Of ActionExecutingContext)

    ''' <summary>
    ''' Initializes a new instance of <seealso cref="Recaptcha3ActionFilterAttribute"/> class.
    ''' </summary>
    ''' <param name="SecretKey">The secret key of Recaptcha 3</param>
    ''' <param name="FilterItems"></param>
    ''' <param name="RecaptchaFieldName"></param>
    Public Sub New(SecretKey As String, FilterItems As IEnumerable(Of RecaptchaFilterItem), Optional OnError As Action(Of ActionExecutingContext) = Nothing, Optional RecaptchaFieldName As String = "recaptcha_response")
        If String.IsNullOrWhiteSpace(SecretKey) Then Throw New ArgumentNullException(NameOf(SecretKey))
        If FilterItems Is Nothing Then Throw New ArgumentNullException(NameOf(FilterItems))
        If String.IsNullOrWhiteSpace(RecaptchaFieldName) Then Throw New ArgumentNullException(NameOf(RecaptchaFieldName))

        Me.SecretKey = SecretKey
        Me.OnFailure = OnError
        Me.RecaptchaFieldName = RecaptchaFieldName
        Me.FilterItems = FilterItems
    End Sub

    Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
        filterContext.Controller.ViewData.ModelState.AddModelError("", "")
        Dim CurrentRequest As Web.HttpRequestBase = filterContext.HttpContext.Request
        For Each FilterItem As RecaptchaFilterItem In FilterItems
            ' Check against UrlPattern and HttpMethod.
            If Regex.IsMatch(CurrentRequest.Url.AbsolutePath, FilterItem.UrlPattern, RegexOptions.Compiled) _
                    AndAlso Regex.IsMatch(CurrentRequest.HttpMethod, FilterItem.HttpMethod, RegexOptions.Compiled) Then
                If Not CurrentRequest.Form.Keys.OfType(Of String).Any(Function(K) K = Me.RecaptchaFieldName) OrElse
                    Not CommonHelper.ValidateRecaptchaV3(Me.SecretKey, CurrentRequest.Form(Me.RecaptchaFieldName)) Then
                    If OnFailure Is Nothing Then
                        CommonHelper.AddErrorToModelState(filterContext, "Recaptcha", "The captcha is invalid!")
                    Else
                        OnFailure.Invoke(filterContext)
                    End If
                End If
                Exit For
            End If
        Next
        MyBase.OnActionExecuting(filterContext)
    End Sub

End Class
