using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecflowBrowserStack.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecflowBrowserStack.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly WebDriver _driver;
        private static bool passed = false;

        public LoginSteps(WebDriver driver)
        {
            _driver = driver;
        }

        [Given(@"I am on ""(.*)""")]
        public void GivenIAmOnWebsite(string url)
        {
            _driver.Current.Navigate().GoToUrl(url);
            _driver.Current.Manage().Window.Maximize();
        }

        [When(@"I login with the username (.*) using the password (.*)")]
        public void WhenIEnterCorrectLoginDetails(string username, string password)
        {
            IWebElement usernameField = _driver.Current.FindElement(By.Id("username"));
            usernameField.SendKeys(username);

            IWebElement passwordField = _driver.Current.FindElement(By.Id("password"));
            passwordField.SendKeys(password);

            IWebElement loginButton = _driver.Current.FindElement(By.Id("submit"));
            loginButton.Click();
        }

        [Then(@"The login should have (.*)")]
        public void ThenLoginSuccessful(String status)
        {
            if (status == "passed")
            {
                bool isDisplayed = _driver.Wait.Until(heading => heading.FindElement(By.CssSelector(".post-title")).Displayed);
                isDisplayed.Should().BeTrue();

                if (isDisplayed)
                {
                    ((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"The login was successful\"}}");
                }
                else
                {
                    ((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"The login was not successful\"}}");
                }
            }
            else if (status == "failed")
            {
                bool isDisplayed = _driver.Wait.Until(errorMessage => errorMessage.FindElement(By.CssSelector("#error")).Displayed);
                isDisplayed.Should().BeTrue();

                if (isDisplayed)
                {
                    ((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"The login error message was displayed\"}}");
                }
                else
                {
                    ((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"The error message was not displayed as expected\"}}");
                }
            }
        }
    }
}
