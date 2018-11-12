# ASKme

ASKme is demo application for .NET Core technologies. It's simple to be easily understood, yet reasonably complex to utilize real-world techniques.

## Projects

* **Altairis.AskMe.Data** is data access layer. It's built using Entity Framework Core and includes migrations and data seeding.
* **Altairis.AskMe.Web.Mvc** is web application built using ASP.NET MVC Core. It uses for example authentication with ASP.NET Identity, view components and custom tag helpers.
* **Altairis.AskMe.Web.RazorPages** is exactly the same application, but built using ASP.NET Razor Pages.

> _Please note:_ The web applications share lots of code, which is copy-pasted between projects, sometimes with minor modifications. Normally this is to be discouraged and the common code extracted to separate shared project. In this case it's intentional, to show the similarities and differences between the technologies used, as in real world there won't be case of two exactly duplicate applications written in different technologies.

## Contributor Code of Conduct

This project adheres to No Code of Conduct. We are all adults. We accept anyone's contributions. Nothing else matters.

For more information please visit the [No Code of Conduct](https://github.com/domgetter/NCoC) homepage.