@ModelType System.Collections.Generic.IEnumerable(Of ModelError)

@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Sign In</title>
</head>
<body>
    <form action="/auth/sign-in" method="post">
        @For Each ErrorObj In Model
            @<div style="color:red">@ErrorObj.ErrorMessage</div>
        Next
        @RecaptchaMvc.HtmlHelper.SetupHiddenRecaptcha()
        <button type="submit">Sign in</button>
    </form>
    @RecaptchaMvc.HtmlHelper.SetupClientScript("[YOUR_CLIENT_KEY]")
</body>
</html>
