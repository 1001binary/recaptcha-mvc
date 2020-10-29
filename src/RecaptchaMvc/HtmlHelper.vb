Imports System.Web

''' <summary>
''' Represents the custom HTML Helper for setting up Google recaptcha v3.
''' </summary>
Public Class HtmlHelper

    ''' <summary>
    ''' Setup the hidden field for Google Recaptcha v3.
    ''' </summary>
    ''' <param name="RecaptchaFieldName"></param>
    ''' <param name="RecaptchaFieldId"></param>
    ''' <returns></returns>
    Public Shared Function SetupHiddenRecaptcha(Optional RecaptchaFieldName As String = "recaptcha_response", Optional RecaptchaFieldId As String = "recaptchaResponse") As IHtmlString
        Dim RecaptchaHiddenHtml = $"<input type=""hidden"" name=""{RecaptchaFieldName}"" id=""{RecaptchaFieldId}"">"
        Return New HtmlString(RecaptchaHiddenHtml)
    End Function

    ''' <summary>
    ''' Setup the client script for Google Recaptcha v3.
    ''' </summary>
    ''' <param name="ClientKey"></param>
    ''' <param name="RecaptchaFieldId"></param>
    ''' <returns></returns>
    Public Shared Function SetupClientScript(ClientKey As String, Optional RecaptchaFieldId As String = "recaptchaResponse") As IHtmlString
        Dim RecaptchaInstallHtml = "<script src=""https://www.google.com/recaptcha/api.js?render=6Lc7H9MZAAAAAFZPhmpczeFjvx10CBH1vFmgG4LO"" type=""text/javascript""></script>
<script>
        function reloadRecaptcha() {
            grecaptcha.ready(function () {
                grecaptcha.execute('" & ClientKey & "', { action: '' }).then(function (token) {
                    var recaptchaResponse = document.getElementById('" & RecaptchaFieldId & "');
                    recaptchaResponse.value = token;
                });
            });
        }
        reloadRecaptcha();
    </script>"
        Return New HtmlString(RecaptchaInstallHtml)
    End Function
End Class
