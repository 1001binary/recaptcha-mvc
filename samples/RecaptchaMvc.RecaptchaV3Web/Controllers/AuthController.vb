Imports System.Web.Mvc

Namespace Controllers
    <RoutePrefix("auth")>
    Public Class AuthController
        Inherits Controller

        <Route("sign-in")>
        <HttpGet>
        Function SignIn() As ActionResult
            If Not ModelState.IsValid Then
                Return View(ModelState.Values.SelectMany(Function(v) v.Errors))
            End If
            Return View()
        End Function

        <Route("sign-in")>
        <HttpPost>
        Function SignIn(SignInDto As Object) As ActionResult
            If Not ModelState.IsValid Then
                Return View(ModelState.Values.SelectMany(Function(v) v.Errors))
            End If
            Return View(New List(Of ModelError))
        End Function
    End Class
End Namespace