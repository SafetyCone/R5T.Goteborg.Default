using System;

using R5T.Viborg;


namespace R5T.Goteborg.Default
{
    public class GitHubOperator : IGitHubOperator
    {
        private IWebDriverProvider WebDriverProvider { get; }


        public GitHubOperator(IWebDriverProvider webDriverProvider)
        {
            this.WebDriverProvider = webDriverProvider;
        }

        public void CreateRepository(GitHubRepository repository)
        {
            using (var webDriver = this.WebDriverProvider.GetWebDriver())
            {
                // Login.

                // Click the create new repository button.

                // Fill in the details of the new repository.

                // Click the create new repository button.

                // Success!
            }
        }
    }
}
