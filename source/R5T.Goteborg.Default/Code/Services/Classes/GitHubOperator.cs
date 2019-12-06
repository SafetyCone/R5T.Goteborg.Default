using System;
using System.Drawing;

using Microsoft.Extensions.Options;

using OpenQA.Selenium;

using R5T.Polidea;
using R5T.Viborg;


namespace R5T.Goteborg.Default
{
    public class GitHubOperator : IGitHubOperator
    {
        private IOptions<GitHubAuthentication> GitHubAuthentication { get; }
        private IWebDriverProvider WebDriverProvider { get; }


        public GitHubOperator(IOptions<GitHubAuthentication> gitHubAuthentication, IWebDriverProvider webDriverProvider)
        {
            this.GitHubAuthentication = gitHubAuthentication;
            this.WebDriverProvider = webDriverProvider;
        }

        public void CreateRepository(GitHubRepository repository)
        {
            using (var webDriver = this.WebDriverProvider.GetWebDriver())
            {
                webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(3);

                // Make sure the window is the same size so that all links are shown (nothing hidden in a hamburger menu).
                webDriver.Manage().Window.Size = new Size(1600, 768);

                // Goto GitHub.com.
                webDriver.Url = "http://www.github.com";

                // Login.
                // Main page.
                var signIn = webDriver.FindElement(By.LinkText("Sign in"));
                signIn.Click();

                // Sign-in page.
                var userNameValue = this.GitHubAuthentication.Value.UserName;
                var passwordValue = this.GitHubAuthentication.Value.Password;

                var userName = webDriver.FindElement(By.Id("login_field"));
                userName.SendKeys(userNameValue);

                var password = webDriver.FindElement(By.Id("password"));
                password.SendKeys(passwordValue);

                var commit = webDriver.FindElement(By.Name("commit"));
                commit.Click();

                // Click the create new repository button.
                // Main page for user.
                var newRepository = webDriver.FindElement(By.LinkText("New"));
                newRepository.Click();

                // Fill in the details of the new repository.
                var repositoryName = webDriver.FindElement(By.Id("repository_name"));
                repositoryName.SendKeys(repository.Name);

                var repositoryDescription = webDriver.FindElement(By.Id("repository_description"));
                repositoryDescription.SendKeys(repository.Description);

                if(repository.Visibility == GitHubRepositoryVisibility.Private)
                {
                    var repositoryVisibilityPrivate = webDriver.FindElement(By.Id("repository_visibility_private"));
                    repositoryVisibilityPrivate.Click();
                }
                else
                {
                    // Otherwise public!
                    var repositoryVisibilityPublic = webDriver.FindElement(By.Id("repository_visibility_public"));
                    repositoryVisibilityPublic.Click();
                }

                if(repository.InitializeWithReadMe)
                {
                    var repositoryAutoInit = webDriver.FindElement(By.Id("repository_auto_init"));
                    repositoryAutoInit.Click();
                }

                var licenseContainer = webDriver.FindElement(By.CssSelector(".license-container > .btn"));
                licenseContainer.Click();

                var mitLicenseListItem = webDriver.FindElement(By.CssSelector("li:nth-child(4) .select-menu-item-text"));
                mitLicenseListItem.Click();

                System.Threading.Thread.Sleep(1000);

                // Click the create new repository button.
                var createRepositoryButton = webDriver.FindElement(By.CssSelector(".first-in-line"));
                createRepositoryButton.Click();

                // Success!
                string a = "A";
            }
        }
    }
}
