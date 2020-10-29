# Recaptcha-mvc
The .NET library for integrating Google Recaptcha V3 into ASP.NET MVC 3 and higher.

<p align="center">
  <img src="https://www.google.com/recaptcha/intro/images/hero-recaptcha-invisible.gif">
</p>
<p align="center">
  Source: image from Google
</p>


### Usage

- Suppose you have a ASP.NET MVC project.
- Go to Google Recaptcha V3 to get client key and secret key.
- Install RecaptchaMvc.NET via Nuget.
- Download the sample project from this repository.
- Follow it to integrate Google Recaptcha V3 into your web application.

### API

#### HtmlHelper static class
- **HtmlHelper.SetupHiddenRecaptcha**: insert the recaptcha hidden field.
- **HtmlHelper.SetupClientScript**: insert the Google Recaptcha v3 script and lib's script to display captcha.

#### CommonHelper static class
- **CommonHelper.ValidateRecaptchaV3**: validate the captcha against secret key and token.
- **CommonHelper.AddErrorAndRedirectToGetAction**: add error to TempData and redirects to the current action.
- **CommonHelper.RedirectToUrl**: redirects to a specific action.
- **CommonHelper.AddErrorToModelState**(by default): adds an error message to ModelState in the current action.

#### Recaptcha3ActionFilterAttribute filter

This filter is used for verifying the captcha for a number of required URLs. This accepts three parameters:

- SecretKey: the secret key of Google Recaptcha V3 service.
- FilterItems: a number of conditions which are valid for the catpcha verification.
- OnFailure: in case the catpcha verification is failed, this action will be executed. By default, **CommonHelper.AddErrorToModelState**
- RecaptchaFieldName: the HTML field name of the captcha. By default, **recaptcha_response**.

### Feedback
Any feedback are appreciated a lot.

### Change Logs

#### v0.1.0

- Supports ASP.NET MVC 3 and higher.

### Copyright and License
&copy;Copyright 2020 by 1001binary
MIT License
