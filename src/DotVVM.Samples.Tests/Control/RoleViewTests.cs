﻿using System.Linq;
using DotVVM.Samples.Tests.Base;
using DotVVM.Testing.Abstractions;
using Xunit;
using Riganti.Selenium.Core;

namespace DotVVM.Samples.Tests.Control
{
    public class RoleViewTests : AppSeleniumTest
    {
        public RoleViewTests(Xunit.Abstractions.ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Control_RoleView_RoleViewTest()
        {
            RunInAllBrowsers(browser => {
                browser.NavigateToUrl(SamplesRouteUrls.ControlSamples_RoleView_RoleViewTest);

                void AssertInnerTextEquals(string selector, string text)
                {
                    AssertUI.InnerTextEquals(
                        browser.FindElements(selector).ThrowIfDifferentCountThan(1).First(),
                        text);
                }

                // make sure we are signed out (first should show IfNotMember, second should be hidden)
                browser.First("input[value='Sign Out']").Click().Wait();

                AssertInnerTextEquals(".result1", "I am not a member!");
                AssertUI.IsNotDisplayed(browser, ".result2");

                // sign in as admin (both should show IsMember content)
                browser.First("input[type=checkbox][value=admin]").Click();
                browser.First("input[value='Sign In']").Click().Wait();

                AssertInnerTextEquals(".result1", "I am a member!");
                AssertInnerTextEquals(".result2", "I am a member!");

                // sign in as moderator and headhunter (both should show IsMember content)
                browser.First("input[type=checkbox][value=moderator]").Click();
                browser.First("input[type=checkbox][value=headhunter]").Click();
                browser.First("input[value='Sign In']").Click().Wait();

                AssertInnerTextEquals(".result1", "I am a member!");
                AssertInnerTextEquals(".result2", "I am a member!");

                // sign in as headhunter only (both should be visible but show that user is not a member)
                browser.First("input[type=checkbox][value=headhunter]").Click();
                browser.First("input[value='Sign In']").Click().Wait();

                AssertInnerTextEquals(".result1", "I am not a member!");
                AssertInnerTextEquals(".result2", "I am not a member!");

                // sign in as tester only (both should show IsMember content)
                browser.First("input[type=checkbox][value=tester]").Click();
                browser.First("input[value='Sign In']").Click().Wait();

                AssertInnerTextEquals(".result1", "I am a member!");
                AssertInnerTextEquals(".result2", "I am a member!");

                // sign out (first should show IfNotMember, second should be hidden)
                browser.First("input[value='Sign Out']").Click().Wait();

                AssertInnerTextEquals(".result1", "I am not a member!");
                AssertUI.IsNotDisplayed(browser, ".result2");
            });
        }
    }
}
